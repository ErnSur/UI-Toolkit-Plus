using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    internal static class PackageResources
    {
        private const string BaseDir = "com.quickeye.ui-toolkit-plus/";

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
            if (TryLoadTree<T>(out var tree))
            {
                tree.CloneTree(ve);
                ve.AssignQueryResults(ve);
            }

            if (TryLoadStyle<T>(out var styleSheet))
                ve.styleSheets.Add(styleSheet);
        }
    }
}