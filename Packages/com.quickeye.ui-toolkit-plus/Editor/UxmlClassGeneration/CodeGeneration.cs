using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CodeGeneration
    {
        public static string UssNameToVariableName(string input)
        {
            return Regex.Replace(input, "-+.", m => char.ToUpper(m.Value[m.Length - 1]).ToString());
        }

        public static string GetNamespaceForFile(string filePath)
        {
            return GetNamespaceForDir(Path.GetDirectoryName(filePath));
        }

        private static string GetNamespaceForDir(string directoryName)
        {
            if (string.IsNullOrWhiteSpace(directoryName))
                return string.Empty;

            if (directoryName == "Assets")
                return EditorSettings.projectGenerationRootNamespace;

            foreach (var file in Directory.GetFiles(directoryName, "*"))
            {
                if (!TryLoadAssemblyDefinition(file, out var asmDef))
                    continue;

                if (!string.IsNullOrEmpty(asmDef.rootNamespace))
                    return asmDef.rootNamespace;

                if (!string.IsNullOrEmpty(asmDef.name))
                    return asmDef.name;
            }

            return GetNamespaceForDir(Path.GetDirectoryName(directoryName));
        }

        private static bool TryLoadAssemblyDefinition(string filePath, out AsmDefinition asmDef)
        {
            asmDef = null;

            if (filePath.EndsWith(".asmdef"))
            {
                asmDef = JsonUtility.FromJson<AsmDefinition>(File.ReadAllText(filePath));
            }
            else if (filePath.EndsWith(".asmref"))
            {
                asmDef = JsonUtility.FromJson<AsmDefinition>(File.ReadAllText(filePath));
                var reference = asmDef.reference;
                asmDef = reference.StartsWith("GUID:")
                    ? LoadAsmDef(reference[5..])
                    : AssetDatabase.FindAssets($"t:{nameof(AssemblyDefinitionAsset)}")
                        .Select(LoadAsmDef)
                        .FirstOrDefault(def => def.name == reference);
                if (asmDef == null)
                    Debug.LogError($"Assembly reference is missing: {filePath}");
            }

            return asmDef != null;
        }

        private static AsmDefinition LoadAsmDef(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return null;
            var json = AssetDatabase.LoadAssetAtPath<TextAsset>(AssetDatabase.GUIDToAssetPath(guid)).text;
            return JsonUtility.FromJson<AsmDefinition>(json);
        }

        [Serializable]
        private class AsmDefinition
        {
            public string name;
            public string reference;
            public string rootNamespace;
        }
    }
}