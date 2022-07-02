using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    internal static class PackageResources
    {
        private const string BaseDir = "com.quickeye.ui-toolkit-plus/";

        private static StyleSheet _baseTheme;

        private static T LoadAsset<T>(string resourcesRelativePath) where T : Object
        {
            var dir = typeof(T) == typeof(StyleSheet) ? "uss" : "uxml";
            return Resources.Load<T>(Path.Combine(BaseDir, dir, resourcesRelativePath));
        }

        public static bool TryLoadTree<T>(out VisualTreeAsset tree)
        {
            tree = LoadAsset<VisualTreeAsset>(typeof(T).Name);
            return tree != null;
        }

        private static bool TryLoadStyle<T>(out StyleSheet styleSheet) where T : VisualElement
        {
            styleSheet = LoadAsset<StyleSheet>(typeof(T).Name);
            return styleSheet != null;
        }

        public static void InitResources<T>(this T ve) where T : VisualElement
        {
            ve.RegisterCallback<AttachToPanelEvent>(AddStyleSheetsToRootElement);
            if (TryLoadTree<T>(out var tree))
            {
                tree.CloneTree(ve);
                ve.AssignQueryResults(ve);
            }

            if (TryLoadStyle<T>(out var styleSheet))
                ve.styleSheets.Add(styleSheet);
        }

        public static void AddStyleSheetsToRootElement(AttachToPanelEvent evt)
        {
            var root = ((VisualElement)evt.target)?.panel.visualTree;
            if (root == null)
                return;
            ThemeStyleSheet s;
            
            if (_baseTheme == null)
                _baseTheme = LoadAsset<StyleSheet>("QuickEye UTPlus Theme");
            if (!root.styleSheets.Contains(_baseTheme))
            {
                root.styleSheets.Add(_baseTheme);
            }
        }
    }
}