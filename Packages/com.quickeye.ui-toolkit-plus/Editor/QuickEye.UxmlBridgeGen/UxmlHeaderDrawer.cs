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
        
        private readonly GUIContent _csNamespaceFieldLabel = new GUIContent("C# Namespace","The namespace of a class is determined by the following factors:\n\n1. If this field is populated, its value will be used.\n2. If `AssemblyDefinitionAsset` or `AssemblyDefinitionReferenceAsset` exists in the UXML directory or parent directory: `AssemblyDefinitionAsset.rootNamespace` will be used.\n3. If `AssemblyDefinitionAsset.rootNamespace` is empty `AssemblyDefinitionAsset.name` will be used instead.\n4. If the UXML file is inside the Assets folder, the `EditorSettings.projectGenerationRootNamespace` will be used.\n5. If none of the above conditions are met, the class will have no namespace.");
        
        private readonly GUIContent _genScriptFieldLabel = new GUIContent("Gen C# Script","The generated C# script that gets updated when the UXML file changes. Use the 'Generate Code...' dropdown to generate a new script.");

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
        private string _firstTargetUxmlPath;
        private MonoScript _firstTargetGenCs;
        private string _namespaceFieldString;
        private bool _showOverrideField;
        private Rect _generateScriptDropdownRect;
        private Rect _textFieldDropdownRect;
        private InlineSettings _inlineSettings;
        private readonly GUIContent _optionsDropdownLabel = new GUIContent("Generate Code...");

        public UxmlHeaderDrawer(Editor editor) : base(editor)
        {
            Setup(editor);
        }

        private void Setup(Editor editor)
        {
            _firstTargetUxmlPath = ((ScriptedImporter)editor.target).assetPath;
            _inlineSettings = InlineSettings.FromXmlFile(_firstTargetUxmlPath);
            InlineSettingsUtils.TryGetGenCsFilePath(_firstTargetUxmlPath, out var firstTargetGenCsPath,
                out _);
            _firstTargetGenCs = AssetDatabase.LoadAssetAtPath<MonoScript>(firstTargetGenCsPath);
            _firstTargetNamespace = _namespaceFieldString =
                CsNamespaceUtils.GetCsNamespace(_firstTargetUxmlPath, out _showOverrideField);
        }

        public override void OnGUI()
        {
            SetShowMixedValuesAndFieldOverride();
            GenerateScriptDropdown();
            NamespaceField();
            GenCsField();
        }

        // TODO: Add a tooltip: how this field works
        private void GenCsField()
        {
            if (Editor.targets.Length > 1)
                return;

            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUIUtility.labelWidth = 100;

                EditorGUILayout.PrefixLabel(_genScriptFieldLabel);
                var newFile = EditorGUILayout.ObjectField(_firstTargetGenCs, typeof(MonoScript), false);
                EditorGUIUtility.labelWidth = 0;
                if (changeScope.changed && newFile != null)
                {
                    var newFilePath = AssetDatabase.GetAssetPath(newFile);
                    if (EditorUtility.DisplayDialog("Dangerous action!",
                            "The content of this file can be overwritten by the code generation system. Do you want to proceed?",
                            "Yes", "No") && ShouldSaveWriteToFile())
                    {
                        _inlineSettings.GenCsGuid = AssetDatabase.AssetPathToGUID(newFilePath);
                        _inlineSettings.WriteTo(_firstTargetUxmlPath, true);
                        Setup(Editor);
                    }
                }
                else if (changeScope.changed && newFile == null)
                {
                    _inlineSettings.GenCsGuid = null;
                    _inlineSettings.WriteTo(_firstTargetUxmlPath, true);
                    Setup(Editor);
                }
            }
        }

        private void NamespaceField()
        {
            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            using (new OverrideFieldScope(_showOverrideField))
            {
                EditorGUILayout.PrefixLabel(_csNamespaceFieldLabel);

                var evt = Event.current;
                if (evt.type == EventType.Repaint)
                {
                    _textFieldDropdownRect = GUILayoutUtility.GetLastRect();
                }

                if (_showOverrideField && evt.type == EventType.MouseDown && evt.button == 1 &&
                    _textFieldDropdownRect.Contains(evt.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Revert"), false, () => TryUpdateInlineNamespace(null));
                    GUIUtility.keyboardControl = 0;
                    menu.DropDown(_textFieldDropdownRect);
                }

                using (var changeScope = new EditorGUI.ChangeCheckScope())
                {
                    var newValue = EditorGUILayout.DelayedTextField(_namespaceFieldString);
                    if (changeScope.changed && TryUpdateInlineNamespace(_namespaceFieldString))
                    {
                        _namespaceFieldString = newValue;
                    }

                }

                EditorGUI.showMixedValue = false;
            }
        }

        /// <summary>
        /// Tries to update the inline namespace of the UXML file.
        /// </summary>
        /// <param name="newValue">Set to null to remove the inline namespace</param>
        private bool TryUpdateInlineNamespace(string newValue)
        {
            if (!ShouldSaveWriteToFile())
                return false;
            GUIUtility.keyboardControl = 0;
            var uxmlPaths = GetTargetPaths().ToArray();
            foreach (var uxmlPath in uxmlPaths)
            {
                CsNamespaceUtils.SetInlineNamespace(uxmlPath, newValue);
            }

            _firstTargetNamespace = _namespaceFieldString = CsNamespaceUtils.GetCsNamespace(uxmlPaths[0], out _);
            EditorApplication.delayCall += () =>
            {
                AssetDatabase.StartAssetEditing();
                try
                {
                    foreach (var uxmlPath in uxmlPaths)
                    {
                        if (UxmlPostprocessor.ShouldGenerateCsFile(uxmlPath))
                            GenCsClassGenerator.GenerateGenCs(uxmlPath, true);
                    }
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
            };
            return true;
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
            var style = new GUIStyle("MiniPullDown");
            style.alignment = TextAnchor.MiddleCenter;

            if (EditorGUILayout.DropdownButton(_optionsDropdownLabel, FocusType.Keyboard, style))
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
                menu.AddItem(new GUIContent("Open documentation"), false, () =>
                {
                    EditorUtility.OpenWithDefaultApp("Packages/com.quickeye.ui-toolkit-plus/Documentation~/UxmlCodeGeneration.md");
                });
                GUIUtility.keyboardControl = 0;
                menu.DropDown(_generateScriptDropdownRect);
            }

            void RegenerateGenCsFile()
            {
                if (!ShouldSaveWriteToFile())
                    return;
                // I start the StartAssetEditing because the GenCsClassGenerator.GenerateGenCs can cause asset import
                AssetDatabase.StartAssetEditing();
                try
                {
                    foreach (var target in Editor.targets.OfType<ScriptedImporter>())
                    {
                        GenCsClassGenerator.GenerateGenCs(target.assetPath, true);
                    }

                    Setup(Editor);
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }
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

        private bool ShouldSaveWriteToFile()
        {
            if (!IsUIBuilderWindowOpen())
                return true;

            var message =
                @"UI Builder Window is open!
Unsaved changes to this UXML file in the UI Builder will be lost.
Save changes in the UI Builder window before modifying this file.";
            
            return EditorUtility.DisplayDialog("UI Builder Window is open!", message, "Proceed", "Cancel");
        }

        private static bool IsUIBuilderWindowOpen()
        {
            return UIBuilderUtils.TryGetUIBuilderWindow(out _);
        }
    }
}