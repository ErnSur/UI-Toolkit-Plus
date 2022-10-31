using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UssExporter
{
    internal static class StyleSheetExporterDatastore
    {
        private static readonly AssetBundle _EditorAssetBundle;

        static StyleSheetExporterDatastore()
        {
            _EditorAssetBundle = GetEditorAssetBundle();
        }

        public static string[] GetStyleSheetPaths()
        {
            return (from path in _EditorAssetBundle.GetAllAssetNames()
                let asset = _EditorAssetBundle.LoadAsset<StyleSheet>(path)
                where asset != null
                select path).ToArray();
        }

        public static void ExportToFile(string sheetPath)
        {
            var fileName = Path.GetFileName(sheetPath);
            if (Path.GetExtension(fileName) == ".asset")
                fileName = Path.ChangeExtension(fileName, null);
            var projectPath = EditorUtility.SaveFilePanel("Save Style Sheet", "Assets/", fileName, ".uss");
            if (string.IsNullOrEmpty(projectPath))
                return;
            
            var sheet = EditorGUIUtility.Load(sheetPath) as StyleSheet;
            WriteStyleSheet(sheet, projectPath);
            AssetDatabase.ImportAsset(projectPath,ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();
        }

        public static void WriteStyleSheet(StyleSheet sheet, string path)
        {
            var ass = typeof(EditorWindow).Assembly;
            var type = ass.GetType("UnityEditor.StyleSheets.StyleSheetToUss");
            var method = type.GetMethod(
                "WriteStyleSheet",
                BindingFlags.Public | BindingFlags.Static);
            method.Invoke(null, new object[] { sheet, path, null });
        }

        private static AssetBundle GetEditorAssetBundle()
        {
            var editorGUIUtility = typeof(EditorGUIUtility);
            var getEditorAssetBundle = editorGUIUtility.GetMethod(
                "GetEditorAssetBundle",
                BindingFlags.NonPublic | BindingFlags.Static);

            return (AssetBundle)getEditorAssetBundle.Invoke(null, null);
        }
    }
}