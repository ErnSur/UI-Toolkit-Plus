using System.IO;
using System.Linq;
using UnityEditor;

namespace QuickEye.UxmlBridgeGen
{
    internal class UxmlPostprocessor : AssetPostprocessor
    {
        public static bool ShouldGenerateCsFile(string uxmlPath)
        {
            return InlineSettingsUtils.TryGetGenCsFilePath(uxmlPath, out var genCsFilePath, out _) &&
                   File.Exists(genCsFilePath);
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