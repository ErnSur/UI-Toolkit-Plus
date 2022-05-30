#if UI_BUILDER || UNITY_2021_1_OR_NEWER
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using System.Xml.Linq;
using UnityEditor;

namespace QuickEye.UIToolkit
{
    public static class UIBuilderUtils
    {
        private static readonly Type UIBuilderType =
            Type.GetType("Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule");

        [Shortcut("UI Builder/Copy Field Declarations", KeyCode.C, ShortcutModifiers.Alt)]
        private static void CopyFieldDeclarations()
        {
            var foundWindowWithReflection = TryGetUIBuilderWindow(out var window);
            if (!foundWindowWithReflection)
                window = EditorWindow.focusedWindow;

            window.SendEvent(EditorGUIUtility.CommandEvent("Copy"));

            if (TryTranslateXmlToFieldDeclarations(GUIUtility.systemCopyBuffer, out var result, out var count))
            {
                GUIUtility.systemCopyBuffer = result;
                window.ShowNotification(new GUIContent($"Field declarations copied. ({count})"), 0.2);
            }
            else if (foundWindowWithReflection)
            {
                window.ShowNotification(new GUIContent("Nothing to copy."), 0.2);
            }
        }

        private static bool TryGetUIBuilderWindow(out EditorWindow window)
        {
            if (UIBuilderType == null)
            {
                Debug.LogWarning("Could not find UI Builder type.");
                window = null;
                return false;
            }

            var objectsOfTypeAll = Resources.FindObjectsOfTypeAll(UIBuilderType);
            window = objectsOfTypeAll.Cast<EditorWindow>().FirstOrDefault();

            return window;
        }

        private const string CopyFieldsMenuPath = "Assets/Copy UXML Field Declarations";

        [MenuItem(CopyFieldsMenuPath, true)]
        private static bool CopyFieldDeclarationsValidation()
        {
            var obj = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(obj);
            return path.EndsWith(".uxml");
        }

        [MenuItem(CopyFieldsMenuPath)]
        private static void CopyFieldDeclarationsMenu()
        {
            var obj = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(obj);
            var text = File.ReadAllText(path);
            if (TryTranslateXmlToFieldDeclarations(text, out var res, out _))
                GUIUtility.systemCopyBuffer = res;
        }

        private static bool TryTranslateXmlToFieldDeclarations(string xml, out string result, out int count)
        {
            try
            {
                var fieldInfos = (from ele in XDocument.Parse(xml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null
                    select (type: ele.Name.LocalName, name)).ToArray();

                var sb = new StringBuilder();
                foreach (var (type, name) in fieldInfos)
                {
                    sb.AppendLine($"[Q(\"{name}\")]");
                    sb.AppendLine($"private {type} {UssNameToVariableName(name)};");
                }

                count = fieldInfos.Length;
                result = sb.ToString();
                return true;
            }
            catch (Exception e)
            {
                result = null;
                count = 0;
                return false;
            }

            string UssNameToVariableName(string input)
            {
                return Regex.Replace(input, "-+.", m => char.ToUpper(m.Value[m.Length - 1]).ToString());
            }
        }
    }
}
#endif