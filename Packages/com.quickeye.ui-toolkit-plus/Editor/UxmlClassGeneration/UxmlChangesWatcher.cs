using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor.UxmlClassGeneration
{
    internal class UxmlChangesWatcher : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths, bool didDomainReload)
        {
            foreach (var uxmlPath in importedAssets.Where(p => p.EndsWith(".uxml")))
            {
                var genCsFilePath = UxmlClassGenerator.GetGenCsFilePath(uxmlPath);
                if (File.Exists(genCsFilePath))
                {
                    UxmlClassGenerator.GenerateGenCs(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath));
                }
            }
        }
    }
}