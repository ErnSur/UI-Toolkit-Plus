using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    internal static class PackageResources
    {
        private static Dictionary<string, StyleSheet> themes = new Dictionary<string, StyleSheet>()
        {
            { ThemeClassNameEditor, LoadAsset<StyleSheet>("QuickEye UTPlus Theme Editor") },
            { ThemeClassNameRuntime, LoadAsset<StyleSheet>("QuickEye UTPlus Theme Runtime") },
        };

        public const string ThemeClassNameEditor = "qe-uiplus-editor";
        public const string ThemeClassNameRuntime = "qe-uiplus-runtime";

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
            var isEditorPanel = root[0].name != "UIDocument-container";
            if (!ContainsThemeClass(root))
                root.AddToClassList(GetDefaultTheme(isEditorPanel));
            TryLoadPackageStyleSheet(root);
        }

        private static string GetDefaultTheme(bool isEditorPanel)
        {
            return isEditorPanel ? ThemeClassNameEditor : ThemeClassNameRuntime;
        }

        private static void TryLoadPackageStyleSheet(VisualElement panelRoot)
        {
            foreach (var theme in themes)
            {
                if (panelRoot.ClassListContains(theme.Key))
                {
                    panelRoot.styleSheets.Add(theme.Value);
                    return;
                }
            }
        }

        private static bool ContainsThemeClass(VisualElement panelRoot)
        {
            return themes.Any(kvp => panelRoot.ClassListContains(kvp.Key));
        }
    }
}