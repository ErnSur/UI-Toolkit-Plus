#if UI_BUILDER
using System;
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
        private static readonly Type UIBuilderType = Type.GetType("Unity.UI.Builder.Builder, Unity.UI.Builder.Editor");

        [Shortcut("UI Builder/Copy Field Declarations", KeyCode.C, ShortcutModifiers.Alt)]
        private static void CopyFieldDeclarations()
        {
            if (!TryGetUIBuilderWindow(out var window))
                return;
            
            window.SendEvent(EditorGUIUtility.CommandEvent("Copy"));
            
            if (TryTranslateXmlToFieldDeclarations(GUIUtility.systemCopyBuffer, out var result))
            {
                GUIUtility.systemCopyBuffer = result;
                window.ShowNotification(new GUIContent("Field declarations copied"), 0.2);
            }
            else
            {
                Debug.LogError($"Could not parse selected elements.");
            }
        }

        private static bool TryGetUIBuilderWindow(out EditorWindow window)
        {
            if (UIBuilderType == null)
            {
                Debug.LogError("Could not find UI Builder type.");
                window = null;
                return false;
            }
            var objectsOfTypeAll = Resources.FindObjectsOfTypeAll(UIBuilderType);
            window = objectsOfTypeAll.Cast<EditorWindow>().FirstOrDefault();

            return window;
        }

        private static bool TryTranslateXmlToFieldDeclarations(string xml, out string result)
        {
            try
            {
                var fieldInfos = from ele in XDocument.Parse(xml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null
                    select (type: ele.Name.LocalName, name);

                var sb = new StringBuilder();
                foreach (var (type, name) in fieldInfos)
                {
                    sb.AppendLine($"[Q(\"{name}\")]");
                    sb.AppendLine($"private {type} {UssNameToVariableName(name)};");
                }

                result = sb.ToString();
                return true;
            }
            catch(Exception e)
            {
                result = null;
                return false;
            }

            static string UssNameToVariableName(string input)
            {
                return Regex.Replace(input, "-.", m => char.ToUpper(m.Value[1]).ToString());
            }
        }
    }
}
#endif