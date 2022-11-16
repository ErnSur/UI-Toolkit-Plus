using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    internal class UxmlChangesWatcher : AssetPostprocessor
    {
        // TODO: update this
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths, bool didDomainReload)
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