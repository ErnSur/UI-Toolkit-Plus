using System.IO;
using System.Linq;
using UnityEditor;

namespace QuickEye.UxmlBridgeGen
{
    internal class UxmlPostprocessor : AssetPostprocessor
    {
        public static bool ShouldGenerateCsFile(string uxmlPath)
        {
            var genCsFilePath = GenCsClassGenerator.GetGenCsFilePath(uxmlPath);
            return File.Exists(genCsFilePath);
        }
        
        

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var uxmlPath in importedAssets.Where(p => p.EndsWith(".uxml")))
            {
                if (ShouldGenerateCsFile(uxmlPath))
                {
                    GenCsClassGenerator.GenerateGenCs(uxmlPath, false);
                }
            }
        }
    }
}