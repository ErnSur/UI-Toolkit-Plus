using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    using UnityEditor;

    internal class CodeGenHeader : PostHeaderDrawer
    {
        public const string UxmlImporterClassName = "UIElementsViewImporter";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            PersistentPostHeaderManager.EditorCreated += editor =>
            {
                if (editor.target.GetType().Name == UxmlImporterClassName)
                    PersistentPostHeaderManager.RegisterPostHeaderDrawer(new CodeGenHeader(editor));
            };
        }

        private string _uxmlPath;
        private string _currentNamespace;
        private string _textFieldString;

        public CodeGenHeader(Editor editor) : base(editor)
        {
            _uxmlPath = ((ScriptedImporter)editor.target).assetPath;
            _currentNamespace = _textFieldString = CodeGeneration.GetNamespaceForFile(_uxmlPath);
        }

        public override void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                using (new OverrideFieldScope(IsTextFieldModified()))
                {
                    SetShowMixedValues();
                    _textFieldString = EditorGUILayout.TextField("C# Namespace", _textFieldString);
                    EditorGUI.showMixedValue = false;
                }

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
                    UpdateInlineNamespace();
                }
            }
        }

        private void UpdateInlineNamespace()
        {
            foreach (var uxmlPath in Editor.targets.Cast<ScriptedImporter>()
                         .Select(i => i.assetPath))
            {
                InlineCodeGenSettings.SetSetting(uxmlPath, InlineCodeGenSettings.CsNamespaceAttributeName,
                    _textFieldString);
                if (string.IsNullOrEmpty(_textFieldString))
                    _textFieldString = CodeGeneration.GetNamespaceForFile(_uxmlPath);
                _currentNamespace = _textFieldString;
                EditorApplication.delayCall += () =>
                {
                    // do this for all targets
                    var uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
                    UxmlClassGenerator.GenerateGenCs(uxmlAsset, true);
                    Selection.activeObject = uxmlAsset;
                };
            }
        }

        private bool IsTextFieldModified() => _textFieldString != _currentNamespace;

        private void SetShowMixedValues()
        {
            EditorGUI.showMixedValue = Editor.targets.Length > 1 &&
                                       Editor.targets
                                           .Cast<ScriptedImporter>()
                                           .Select(i => i.assetPath)
                                           .Select(CodeGeneration.GetNamespaceForFile)
                                           .Any(n => n != _currentNamespace);
        }

        private static Rect _generateScriptDropdownRect;

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