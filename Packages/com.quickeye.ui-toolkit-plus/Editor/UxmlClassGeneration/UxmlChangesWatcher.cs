using System.IO;
using System.Linq;
using UnityEditor;

namespace QuickEye.UIToolkit.Editor
{
    internal class UxmlChangesWatcher : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var uxmlPath in importedAssets.Where(p => p.EndsWith(".uxml")))
            {
                var genCsFilePath = GenCsClassGenerator.GetGenCsFilePath(uxmlPath);
                if (File.Exists(genCsFilePath))
                {
                    GenCsClassGenerator.GenerateGenCs(uxmlPath, false);
                }
            }
        }
    }
}