using System.Collections.Generic;
using System.Linq;
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    using UnityEditor;

    [InitializeOnLoad]
    public static class CodeGenHeader
    {
        public const string UxmlImporterClassName = "UIElementsViewImporter";

        private static readonly Dictionary<Editor, (string original, string dirty)> _EditorCsNamespaceState = new();

        static CodeGenHeader()
        {
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static (string original, string dirty) _namespaceFieldData;
        private static void OnPostHeaderGUI(Editor editor)
        {
            if (editor.target.GetType().Name != UxmlImporterClassName)
                return;
            if (!_EditorCsNamespaceState.TryGetValue(editor, out var targetCsNamespace))
            {
                var ns = CodeGeneration.GetNamespaceForFile(((ScriptedImporter)editor.target).assetPath);
                _EditorCsNamespaceState[editor] = targetCsNamespace = (ns, ns);
            }

            EditorGUI.showMixedValue = editor.targets.Length > 1 &&
                                       editor.targets
                                           .Cast<ScriptedImporter>()
                                           .Select(i => i.assetPath)
                                           .Select(CodeGeneration.GetNamespaceForFile)
                                           .Any(n => n != targetCsNamespace.original);
            // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
            foreach (ScriptedImporter importer in editor.targets)
            {
                CodeGeneration.GetNamespaceForFile(importer.assetPath);
            }

            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                using (new OverrideFieldScope(IsTextFieldModified()))
                {
                    targetCsNamespace.dirty = EditorGUILayout.TextField("C# Namespace", targetCsNamespace.dirty);
                    _EditorCsNamespaceState[editor] = targetCsNamespace;
                }

                using (new EditorGUI.DisabledScope(IsTextFieldModified()))
                {
                    GenerateScriptDropdown();
                    if (GUILayout.Button("Generate", GUILayout.Width(65)))
                    {
                        foreach (ScriptedImporter target in editor.targets)
                        {
                            UxmlClassGenerator.GenerateGenCs(
                                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(target.assetPath), true);
                        }
                    }
                }
            }

            using (new EditorGUILayout.HorizontalScope(new GUIStyle()))
            {
                GUILayout.FlexibleSpace();
                if (IsTextFieldModified() && GUILayout.Button("Apply", GUILayout.Width(50)))
                {
                    foreach (var uxmlPath in editor.targets.Cast<ScriptedImporter>()
                                 .Select(i => i.assetPath))
                    {
                        InlineCodeGenSettings.SetSettings(uxmlPath, InlineCodeGenSettings.CsNamespaceAttributeName,
                            targetCsNamespace.dirty);
                        if (string.IsNullOrEmpty(targetCsNamespace.dirty))
                            targetCsNamespace.dirty =
                                CodeGeneration.GetNamespaceForFile(((ScriptedImporter)editor.target).assetPath);
                        targetCsNamespace.original = targetCsNamespace.dirty;
                        _EditorCsNamespaceState[editor] = targetCsNamespace;
                        EditorApplication.delayCall += () =>
                        {
                            var uxmlAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
                            UxmlClassGenerator.GenerateGenCs(uxmlAsset, true);
                            Selection.activeObject = uxmlAsset;
                        };
                    }
                }
            }

            bool IsTextFieldModified() => targetCsNamespace.original != targetCsNamespace.dirty;
        }

        private static Rect _generateScriptDropdownRect;

        private static void GenerateScriptDropdown()
        {
            if (EditorGUILayout.DropdownButton(new GUIContent("Generate"), FocusType.Keyboard, GUILayout.Width(70)))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("hejpo"), false, null);
                menu.DropDown(_generateScriptDropdownRect);
            }

            if (Event.current.type == EventType.Repaint)
                _generateScriptDropdownRect = GUILayoutUtility.GetLastRect();
        }

        struct NamespaceData
        {
            public string fromDirectory;
            public string fromInlineAttribute;
            public string textFieldString;
        }
    }
}