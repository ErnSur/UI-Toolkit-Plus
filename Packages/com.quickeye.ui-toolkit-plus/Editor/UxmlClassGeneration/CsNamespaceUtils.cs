using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CsNamespaceUtils
    {
        // TODO: Move elswhere
        public static CodeStyleRules GetFinalCodeStyleRulesFor(string uxmlFilePath)
        {
            var uxml = File.ReadAllText(uxmlFilePath);
            var settings = InlineSettings.GetCodeStyleRules(uxml).Override(CodeGenProjectSettings.CodeStyleRules);
            return settings;
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

        private static bool TryGetInlineNamespace(string uxmlOrCsGenFilePath, out string csNamespace)
        {
            if (TryGetUxmlPath(uxmlOrCsGenFilePath, out var uxmlPath))
            {
                csNamespace = InlineSettings.GetCsNamespace(uxmlPath);
                return csNamespace != null;
            }

            csNamespace = null;
            return false;
        }

        public static void SetInlineNamespace(string uxmlOrCsGenFilePath, string newNamespace)
        {
            if (uxmlOrCsGenFilePath.EndsWith(".gen.cs"))
                uxmlOrCsGenFilePath = uxmlOrCsGenFilePath.Replace(".gen.cs", ".uxml");
            if (!uxmlOrCsGenFilePath.EndsWith(".uxml"))
                return;
            InlineSettings.WriteCsNamespace(uxmlOrCsGenFilePath, newNamespace);
        }

        private static bool TryGetUxmlPath(string uxmlOrCsGenFilePath, out string uxmlPath)
        {
            if (uxmlOrCsGenFilePath.EndsWith(".uxml"))
            {
                uxmlPath = uxmlOrCsGenFilePath;
                return true;
            }

            if (uxmlOrCsGenFilePath.EndsWith(".gen.cs"))
            {
                uxmlPath = uxmlOrCsGenFilePath.Replace(".gen.cs", ".uxml");
                return true;
            }

            uxmlPath = null;
            return false;
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