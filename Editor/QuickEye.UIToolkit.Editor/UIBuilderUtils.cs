#if UI_BUILDER || UNITY_2021_1_OR_NEWER
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using System.Xml.Linq;
using QuickEye.UIToolkit.Editor;
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

            if (TryTranslateUxmlToFieldDeclarations(GUIUtility.systemCopyBuffer, out var result, out var count))
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
            // TODO: test if the asset is actually a text asset in a better way, VisualTreeAsset can exists in a ".asset" format
            return path.EndsWith(".uxml");
        }

        [MenuItem(CopyFieldsMenuPath)]
        private static void CopyFieldDeclarationsMenu()
        {
            var obj = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(obj);
            var uxml = File.ReadAllText(path);
            if (TryTranslateUxmlToFieldDeclarations(uxml, out var res, out _))
                GUIUtility.systemCopyBuffer = res;
        }

        private static bool TryTranslateUxmlToFieldDeclarations(string uxml, out string result, out int count)
        {
            if (UxmlParser.TryGetElementsWithName(uxml, out var elements))
            {
                var sb = new StringBuilder();
                foreach (var e in elements)
                {
                    sb.AppendLine($"[Q(\"{e.NameAttribute}\")]");
                    sb.AppendLine($"private {e.TypeName} {UssNameToVariableName(e.NameAttribute)};");
                }

                count = elements.Length;
                result = sb.ToString();
                return true;
            }

            result = null;
            count = 0;
            return false;
        }

        private static string UssNameToVariableName(string input)
        {
            return Regex.Replace(input, "-+.", m => char.ToUpper(m.Value[m.Length - 1]).ToString());
        }
    }
    
    internal static class UxmlParser
    {
        public static bool TryGetElementsWithName(string uxml, out UxmlElement[] elements)
        {
            try
            {
                elements = (from ele in XDocument.Parse(uxml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null
                    select new UxmlElement(ele)).ToArray();

                return true;
            }
            catch (XmlException)
            {
                elements = null;
                return false;
            }
        }
    }
    
    internal class UxmlElement
    {
        public readonly XElement XElement;
        public readonly string Namespace;
        public readonly string TypeName;
        public readonly string NameAttribute;
        public string FullyQualifiedTypeName => $"{Namespace}.{TypeName}";
        public bool IsUnityEngineType => Namespace == "UnityEngine.UIElements";
        
        public UxmlElement(XElement xElement)
        {
            XElement = xElement;
            var localName = xElement.Name.LocalName;

            Namespace = xElement.Name.NamespaceName != XNamespace.None
                ? xElement.Name.NamespaceName
                : localName[..localName.LastIndexOf('.')];
            TypeName = localName.Contains('.')
                ? localName.Split('.').Last()
                : localName;
            NameAttribute = xElement.Attribute("name")?.Value;
            if (FullyQualifiedTypeName == "UnityEngine.UIElements.Instance")
                TypeName = "TemplateContainer";
        }
    }
}
#endif