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
        public static string UssNameToVariableName(string input)
        {
            return Regex.Replace(input, "-+.", m => char.ToUpper(m.Value[m.Length - 1]).ToString());
        }

        public static string GetInlineNamespace(string filePath)
        {
            if (filePath.EndsWith(".gen.cs"))
                filePath = filePath.Replace(".gen.cs", ".uxml");
            if (!filePath.EndsWith(".uxml"))
                return null;

            return new InlineCodeGenSettings(File.ReadAllText(filePath)).CsNamespace;
        }

        public static void RemoveInlineNamespace(string filePath)
        {
            if (filePath.EndsWith(".gen.cs"))
                filePath = filePath.Replace(".gen.cs", ".uxml");
            if (!filePath.EndsWith(".uxml"))
                return;

            InlineCodeGenSettings.RemoveSetting(filePath, InlineCodeGenSettings.CsNamespaceAttributeName);
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

        private static bool TryGetInlineNamespace(string filePath, out string csNamespace)
        {
            csNamespace = GetInlineNamespace(filePath);
            return csNamespace != null;
        }

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