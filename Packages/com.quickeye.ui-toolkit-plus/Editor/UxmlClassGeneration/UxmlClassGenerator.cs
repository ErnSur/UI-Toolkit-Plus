using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    internal static class UxmlClassGenerator
    {
        private const string GenerateCsMenuItemName = "CONTEXT/VisualTreeAsset/Generate C# Class";

        [MenuItem(GenerateCsMenuItemName)]
        private static void GenerateGenCs(MenuCommand command)
        {
            var asset = command.context as VisualTreeAsset;
            GenerateGenCs(asset);
        }

        [MenuItem(GenerateCsMenuItemName, true)]
        private static bool GenerateGenCsValidation(MenuCommand command)
        {
            return AssetDatabase.GetAssetPath(command.context).EndsWith(".uxml");
        }

        public static void GenerateGenCs(VisualTreeAsset asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var uxml = File.ReadAllText(path);
            var settings = CodeGenSettings.FromUxml(uxml);
            if (UxmlParser.TryGetElementsWithName(uxml, out var elements))
            {
                GenerateScript(asset, elements, GetGenCsFilePath(path), settings);
            }
        }

        private static string GetFieldDeclaration(UxmlElement element, CodeGenSettings settings)
        {
            var type = element.IsUnityEngineType ? element.TypeName : element.FullyQualifiedTypeName;

            return
                $"private {type} {settings.FieldPrefix}{CodeGeneration.UssNameToVariableName(element.NameAttribute)};";
        }

        private static string GetFieldAssigment(UxmlElement element, CodeGenSettings settings)
        {
            var type = element.IsUnityEngineType ? element.TypeName : element.FullyQualifiedTypeName;
            var name = element.NameAttribute;
            var varName = settings.FieldPrefix + CodeGeneration.UssNameToVariableName(name);

            return $"{varName} = root.Q<{type}>(\"{name}\");";
        }

        private static void GenerateScript(VisualTreeAsset asset, UxmlElement[] uxmlElements, string path,
            CodeGenSettings settings)
        {
            var fields = uxmlElements.Select(e => GetFieldDeclaration(e, settings));
            var assignments = uxmlElements.Select(e => GetFieldAssigment(e, settings));

            var template = Resources.Load<TextAsset>("QuickEye/UXMLGenScriptTemplate").text
                .Replace("#SCRIPT_NAME#", asset.name)
                .Replace("#PACKAGE_VERSION#", PackageInfo.Version);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#FIELDS#", fields);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#ASSIGNMENTS#", assignments);
            template = ScriptTemplateUtility.ReplaceNamespaceTags(template, CodeGeneration.GetNamespaceForFile(path));

            var assetGuid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(asset));

            path = GenerationTrackerUtility.GetFixedPath(assetGuid, path);

            File.WriteAllText(path, template);

            AssetDatabase.Refresh();

            GenerationTrackerUtility.Refresh();
        }

        public static string GetGenCsFilePath(string uxmlFilePath)
        {
            return uxmlFilePath.Replace(".uxml", ".gen.cs");
        }
    }
}