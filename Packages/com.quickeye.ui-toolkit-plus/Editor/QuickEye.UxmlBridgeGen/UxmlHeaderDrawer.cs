using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AssetImporters;

namespace QuickEye.UxmlBridgeGen
{
    using UnityEditor;

    internal class UxmlHeaderDrawer : PostHeaderDrawer
    {
        private const string UxmlImporterClassName = "UIElementsViewImporter";

        [InitializeOnLoadMethod]
        private static void Init()
        {
            PersistentPostHeaderManager.EditorCreated += editor =>
            {
                if (editor.target.GetType().Name == UxmlImporterClassName)
                    PersistentPostHeaderManager.RegisterPostHeaderDrawer(new UxmlHeaderDrawer(editor));
            };
        }

        private string _firstTargetNamespace;
        private string _textFieldString;
        private bool _showOverrideField;
        private Rect _generateScriptDropdownRect;
        private Rect _textFieldDropdownRect;

        public UxmlHeaderDrawer(Editor editor) : base(editor)
        {
            var firstTargetUxmlPath = ((ScriptedImporter)editor.target).assetPath;
            _firstTargetNamespace = _textFieldString =
                CsNamespaceUtils.GetCsNamespace(firstTargetUxmlPath, out _showOverrideField);
        }

        public override void OnGUI()
        {
            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                SetShowMixedValuesAndFieldOverride();
                TextField();
                GenerateScriptDropdown();
            }
        }

        private void ApplyNamespaceChanges()
        {
            UpdateInlineNamespace(false);
        }

        private void TextField()
        {
            using (new OverrideFieldScope(_showOverrideField))
            {
                EditorGUILayout.PrefixLabel("C# Namespace");

                var evt = Event.current;
                if (evt.type == EventType.Repaint)
                {
                    _textFieldDropdownRect = GUILayoutUtility.GetLastRect();
                }

                if (_showOverrideField && evt.type == EventType.MouseDown && evt.button == 1 &&
                    _textFieldDropdownRect.Contains(evt.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Revert"), false, () => UpdateInlineNamespace(true));
                    GUIUtility.keyboardControl = 0;
                    menu.DropDown(_textFieldDropdownRect);
                }

                using (var changeScope = new EditorGUI.ChangeCheckScope())
                {
                    _textFieldString = EditorGUILayout.DelayedTextField(_textFieldString);
                    if (changeScope.changed)
                        ApplyNamespaceChanges();
                }


                EditorGUI.showMixedValue = false;
            }
        }

        private void UpdateInlineNamespace(bool removeSetting)
        {
            GUIUtility.keyboardControl = 0;
            var uxmlPaths = GetTargetPaths().ToArray();
            foreach (var uxmlPath in uxmlPaths)
            {
                CsNamespaceUtils.SetInlineNamespace(uxmlPath, removeSetting ? null : _textFieldString);
            }

            _firstTargetNamespace = _textFieldString = CsNamespaceUtils.GetCsNamespace(uxmlPaths[0], out _);
            EditorApplication.delayCall += () =>
            {
                AssetDatabase.StartAssetEditing();
                try
                {
                    foreach (var uxmlPath in uxmlPaths)
                    {
                        GenCsClassGenerator.GenerateGenCs(uxmlPath, true);
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

        private void SetShowMixedValuesAndFieldOverride()
        {
            _showOverrideField = false;
            foreach (var filePath in GetTargetPaths())
            {
                var n = CsNamespaceUtils.GetCsNamespace(filePath, out var isInline);
                _showOverrideField |= isInline;
                EditorGUI.showMixedValue = n != _firstTargetNamespace;
                if (_showOverrideField && EditorGUI.showMixedValue)
                    break;
            }
        }
        
        private void GenerateScriptDropdown()
        {
            if (EditorGUILayout.DropdownButton(new GUIContent("Options"), FocusType.Keyboard, GUILayout.Width(70)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Generate .gen.cs"), false, RegenerateGenCsFile);
                menu.AddItem(new GUIContent("Generate .gen.cs + .cs"), false, () =>
                {
                    RegenerateGenCsFile();
                    CreateCsFile();
                });
                menu.AddSeparator("");
                menu.AddItem(new GUIContent("Open code gen settings"), false,
                    CodeGenProjectSettingsEditor.OpenSettings);
                GUIUtility.keyboardControl = 0;
                menu.DropDown(_generateScriptDropdownRect);
            }

            void RegenerateGenCsFile()
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (ScriptedImporter target in Editor.targets)
                    GenCsClassGenerator.GenerateGenCs(target.assetPath, true);
            }

            void CreateCsFile()
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (ScriptedImporter target in Editor.targets)
                    GenCsClassGenerator.GenerateCs(target.assetPath, true);
            }

            if (Event.current.type == EventType.Repaint)
                _generateScriptDropdownRect = GUILayoutUtility.GetLastRect();
        }
    }
}