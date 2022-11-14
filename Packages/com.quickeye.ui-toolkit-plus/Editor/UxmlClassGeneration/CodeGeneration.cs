using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CodeGeneration
    {
        public static CodeGenSettings GetSettingsFor(string uxmlFilePath)
        {
            var uxml = File.ReadAllText(uxmlFilePath);
            var settings= CodeGenSettingsSerializer.FromUxml(uxml).AddChangesTo(CodeGenProjectSettings.Instance);
            settings.csNamespace ??= GetNamespaceForFile(uxmlFilePath);
            return settings;
        }

        public static string UssNameToVariableName(string input)
        {
            return Regex.Replace(input, "-+.", m => char.ToUpper(m.Value[m.Length - 1]).ToString());
        }

        public static string GetCsNamespace(string uxmlOrGenCsFilePath, out bool isInline)
        {
            if (TryGetInlineNamespace(uxmlOrGenCsFilePath, out var csNamespace))
            {
                isInline = true;
                return csNamespace;
            }

            isInline = false;
            return GetNamespaceForFile(uxmlOrGenCsFilePath);
        }

        public static void SetInlineNamespace(string filePath, string newNamespace)
        {
            if (filePath.EndsWith(".gen.cs"))
                filePath = filePath.Replace(".gen.cs", ".uxml");
            if (!filePath.EndsWith(".uxml"))
                return;
            var settings = CodeGenSettingsSerializer.FromUxml(File.ReadAllText(filePath));
            settings.csNamespace = newNamespace;
            CodeGenSettingsSerializer.SaveTo(settings, filePath);
        }
        
        private static bool TryGetInlineNamespace(string filePath, out string csNamespace)
        {
            csNamespace = GetInlineNamespace(filePath);
            return csNamespace != null;
        }

        private static string GetInlineNamespace(string filePath)
        {
            if (filePath.EndsWith(".gen.cs"))
                filePath = filePath.Replace(".gen.cs", ".uxml");
            if (!filePath.EndsWith(".uxml"))
                return null;

            return CodeGenSettingsSerializer.FromUxml(File.ReadAllText(filePath)).csNamespace;
        }

        /// <summary>
        /// Gets the namespace based on assembly definition files and project settings
        /// </summary>
        private static string GetNamespaceForFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return string.Empty;

            var assemblyDefinitionFilePath =
                CompilationPipeline.GetAssemblyDefinitionFilePathFromScriptPath(filePath);
            if (string.IsNullOrEmpty(assemblyDefinitionFilePath))
                return EditorSettings.projectGenerationRootNamespace;

            if (!TryLoadAssemblyDefinition(assemblyDefinitionFilePath, out var asmDef))
                throw new Exception($"unexpected: {assemblyDefinitionFilePath}");

            if (!string.IsNullOrEmpty(asmDef.rootNamespace))
                return asmDef.rootNamespace;

            return asmDef.name;
        }

        private static bool TryLoadAssemblyDefinition(string filePath, out AsmDefinition asmDef)
        {
            asmDef = JsonUtility.FromJson<AsmDefinition>(File.ReadAllText(filePath));
            return asmDef != null;
        }

        [Serializable]
        private class AsmDefinition
        {
            public string name;
            public string rootNamespace;
        }
    }
}