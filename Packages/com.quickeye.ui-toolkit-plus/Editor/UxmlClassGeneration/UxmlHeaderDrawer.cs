using System.Collections.Generic;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    using UnityEditor;

    internal class UxmlHeaderDrawer : PostHeaderDrawer
    {
        public const string UxmlImporterClassName = "UIElementsViewImporter";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            PersistentPostHeaderManager.EditorCreated += editor =>
            {
                if (editor.target.GetType().Name == UxmlImporterClassName)
                    PersistentPostHeaderManager.RegisterPostHeaderDrawer(new UxmlHeaderDrawer(editor));
            };
        }

        private readonly string _firstTargetUxmlPath;
        private string _firstTargetNamespace;
        private string _textFieldString;
        private Rect _generateScriptDropdownRect;
        private bool _showOverrideField;

        public UxmlHeaderDrawer(Editor editor) : base(editor)
        {
            _firstTargetUxmlPath = ((ScriptedImporter)editor.target).assetPath;
            _firstTargetNamespace = _textFieldString =
                CodeGeneration.GetNamespaceForFile(_firstTargetUxmlPath, out _showOverrideField);
        }

        public override void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                SetShowMixedValuesAndFieldOverride();
                TextField();

                using (new EditorGUI.DisabledScope(IsTextFieldModified()))
                {
                    GenerateScriptDropdown();
                }
            }

            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                GUILayout.FlexibleSpace();
                if (IsTextFieldModified() && GUILayout.Button("Apply", GUILayout.Width(50)))
                {
                    GUIUtility.keyboardControl = 0;
                    UpdateInlineNamespace();
                    if (string.IsNullOrEmpty(_textFieldString))
                        _textFieldString = CodeGeneration.GetNamespaceForFile(_firstTargetUxmlPath, out _);
                    _firstTargetNamespace = _textFieldString;
                }
            }
        }

        private int _textFieldControlId;

        private void TextField()
        {
            using (new OverrideFieldScope(_showOverrideField))
            {
                EditorGUILayout.PrefixLabel("C# Namespace");
                var e = Event.current;
                if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return &&
                    GUIUtility.hotControl == _textFieldControlId)
                {
                    Debug.Log($"KeyDown ");
                }

                _textFieldString = EditorGUILayout.TextField(_textFieldString);
                _textFieldControlId = GUIUtility.GetControlID(FocusType.Keyboard);
                
                if (string.IsNullOrEmpty(_textFieldString))
                {
                    if (Event.current.type == EventType.Repaint)
                    {
                        GUI.color = new Color(1, 1, 1, 0.3f);

                        EditorStyles.textField.Draw(GUILayoutUtility.GetLastRect(),
                            CodeGeneration.GetNamespaceForFileDirectory(_firstTargetUxmlPath)
                            , false, false, false,
                            false);
                        GUI.color = Color.white;
                    }
                }

                EditorGUI.showMixedValue = false;
            }
        }

        private void UpdateInlineNamespace()
        {
            var uxmlPaths = GetTargetPaths().ToArray();
            foreach (var uxmlPath in uxmlPaths)
            {
                InlineCodeGenSettings.SetSetting(uxmlPath, InlineCodeGenSettings.CsNamespaceAttributeName,
                    _textFieldString);
            }

            EditorApplication.delayCall += () =>
            {
                AssetDatabase.StartAssetEditing();
                try
                {
                    foreach (var uxmlPath in uxmlPaths)
                    {
                        var uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
                        UxmlClassGenerator.GenerateGenCs(uxmlAsset, true);
                    }
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
            };
        }

        private IEnumerable<string> GetTargetPaths()
        {
            return Editor.targets
                .Cast<ScriptedImporter>()
                .Select(i => i.assetPath);
        }

        private bool IsTextFieldModified() => _textFieldString != _firstTargetNamespace;

        private void SetShowMixedValuesAndFieldOverride()
        {
            // mixedvalues = if more then 1 target and they have different namespaces
            // nampespaceOverride = any of the targets has a inline namespace
            foreach (var filePath in GetTargetPaths())
            {
                var n = CodeGeneration.GetNamespaceForFile(filePath, out var isInline);
                _showOverrideField |= isInline;
                EditorGUI.showMixedValue = n != _firstTargetNamespace;
                if (_showOverrideField && EditorGUI.showMixedValue)
                    break;
            }
        }


        private void GenerateScriptDropdown()
        {
            if (EditorGUILayout.DropdownButton(new GUIContent("Generate"), FocusType.Keyboard, GUILayout.Width(70)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("C# gen script"), false, Generate);
                menu.DropDown(_generateScriptDropdownRect);
            }

            void Generate()
            {
                foreach (ScriptedImporter target in Editor.targets)
                {
                    UxmlClassGenerator.GenerateGenCs(
                        AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(target.assetPath), true);
                }
            }

            if (Event.current.type == EventType.Repaint)
                _generateScriptDropdownRect = GUILayoutUtility.GetLastRect();
        }
    }
}