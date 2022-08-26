using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CodeGenUtility
    {
        public static bool TryGetNamedElementsFromUxml(string uxml, out (string type, string name)[] elements)
        {
            try
            {
                elements = (from ele in XDocument.Parse(uxml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null
                    select (type: ele.Name.LocalName, name)).ToArray();

                return true;
            }
            // TODO: See what exception can occur here
            catch (Exception e)
            {
                elements = null;
                return false;
            }
        }

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
                        .First(def => def.name == reference);
            }

            return asmDef != null;
        }

        private static AsmDefinition LoadAsmDef(string guid)
        {
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