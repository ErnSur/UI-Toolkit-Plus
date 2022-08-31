using System;
using System.IO;
using QuickEye.UIToolkit;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Editor
{
    internal class PackageInfoGenerator : AssetPostprocessor
    {
        private const string PackageManifestGuid = "9567f359ac77aee4c930ff2374f3d84e";
        private const string PackageInfoDirectoryName = "Packages/com.quickeye.ui-toolkit-plus/Runtime/";
        private static string PackageInfoNamespace => typeof(QAttribute).Namespace;

        private const string PackageInfoPath = PackageInfoDirectoryName + "PackageInfo.cs";
        private const string PackageInfoClassTemplate = @"
// auto-generated
namespace #NAMESPACE#
{
    internal static class PackageInfo
    {
        public const string Version = ""#VERSION#"";
        public const string Name = ""#NAME#"";
        public const string DisplayName = ""#DISPLAY_NAME#"";
    }
}
";

        private static string PackageManifestPath => AssetDatabase.GUIDToAssetPath(PackageManifestGuid);

        private static void OnPostprocessAllAssets(string[] _, string[] __, string[] ___, string[] ____)
        {
            if (!ShouldRegeneratePackageInfo(out var packageManifest))
                return;
            GeneratePackageInfoScript(packageManifest);
        }

        private static bool ShouldRegeneratePackageInfo(out PackageManifest manifest)
        {
            return TryGetPackageManifest(out manifest) && !IsPackageInfoUpToDate(manifest);
        }

        private static bool TryGetPackageManifest(out PackageManifest manifest)
        {
            var jsonAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(PackageManifestPath);
            if (jsonAsset == null)
            {
                Debug.LogWarning($"Can't find package manifest at: {PackageManifestPath}");
                manifest = null;
                return false;
            }

            manifest = PackageManifest.FromTextAsset(jsonAsset);
            if (manifest == null)
            {
                Debug.LogError($"Can't deserialize manifest: {PackageManifestPath}");
                return false;
            }

            return true;
        }

        private static bool IsPackageInfoUpToDate(PackageManifest manifest)
        {
            if (!File.Exists(PackageInfoPath))
                return false;
            var script = File.ReadAllText(PackageInfoPath);
            return script.Contains($"\"{manifest.version}\"")
                   && script.Contains($"\"{manifest.name}\"")
                   && script.Contains($"\"{manifest.displayName}\"");
        }

        private static void GeneratePackageInfoScript(PackageManifest manifest)
        {
            if (manifest == null)
                throw new NullReferenceException("Package Manifest is null");

            Directory.CreateDirectory(PackageInfoDirectoryName);
            GenerateCSharpCode(manifest);
        }

        private static void GenerateCSharpCode(PackageManifest manifest)
        {
            var content = PackageInfoClassTemplate
                .Replace("#NAMESPACE#", PackageInfoNamespace)
                .Replace("#VERSION#", manifest.version)
                .Replace("#NAME#", manifest.name)
                .Replace("#DISPLAY_NAME#", manifest.displayName);

            File.WriteAllText(PackageInfoPath, content);
            PingFile(PackageInfoPath);
        }

        private static void PingFile(string filePath)
        {
            AssetDatabase.ImportAsset(filePath);
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
            EditorGUIUtility.PingObject(asset);
        }
        
        [Serializable]
        // package.json representation
        private class PackageManifest
        {
            public string name;
            public string displayName;
            public string version;

            public static PackageManifest FromTextAsset(TextAsset asset)
            {
                return JsonUtility.FromJson<PackageManifest>(asset.text);
            }
        }
    }
}