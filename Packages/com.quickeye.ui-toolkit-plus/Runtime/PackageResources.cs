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
            { ThemeClassNameEditorDark, LoadAsset<StyleSheet>("UTKPlus Editor Dark") },
            { ThemeClassNameEditorLight, LoadAsset<StyleSheet>("UTKPlus Editor Light") },
            { ThemeClassNameRuntime, LoadAsset<StyleSheet>("UTKPlus Runtime") },
        };

        public const string ThemeClassNameEditorDark = "qe-uiplus-editor-dark";
        public const string ThemeClassNameEditorLight = "qe-uiplus-editor-light";
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
            var target = (VisualElement)evt.target;
            var root = target.panel.visualTree;
            if (root == null)
                return;
            var isRuntimePanel = root.childCount > 0 && root[0].name == "UIDocument-container";
            if (!ContainsThemeClass(root))
                root.AddToClassList(GetDefaultTheme(!isRuntimePanel));
            TryLoadPackageStyleSheet(root, target);
        }

        private static string GetDefaultTheme(bool isEditorPanel)
        {
#if UNITY_EDITOR
            if (isEditorPanel)
                return UnityEditor.EditorGUIUtility.isProSkin ? ThemeClassNameEditorDark : ThemeClassNameEditorLight;
#endif
            return ThemeClassNameRuntime;
        }

        private static void TryLoadPackageStyleSheet(VisualElement panelRoot, VisualElement target)
        {
            foreach (var theme in themes)
            {
                if (panelRoot.ClassListContains(theme.Key))
                {
                    target.styleSheets.Add(theme.Value);
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