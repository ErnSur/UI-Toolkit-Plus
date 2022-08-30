using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor.UxmlClassGeneration
{
    internal static class UxmlClassGenerator
    {
        [MenuItem("CONTEXT/VisualTreeAsset/Generate cs")]
        private static void GenerateGenCs(MenuCommand command)
        {
            var asset = command.context as VisualTreeAsset;
            GenerateGenCs(asset);
        }

        [MenuItem("Assets/Regenerate all UXML-C# files")]
        private static void RegenerateAll()
        {
            // TODO: Regenerate all gen scripts
        }

        public static void GenerateGenCs(VisualTreeAsset asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            var uxml = File.ReadAllText(path);
            var settings = CodeGenSettings.FromUxml(uxml);
            if (UxmlParser.TryGetElementsWithName(uxml, out var elements))
            {
                GenerateScript(asset.name, elements, GetGenCsFilePath(path), settings);
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

        private static void GenerateScript(string scriptName, UxmlElement[] uxmlElements, string path,
            CodeGenSettings settings)
        {
            var fields = uxmlElements.Select(e => GetFieldDeclaration(e, settings));
            var assignments = uxmlElements.Select(e => GetFieldAssigment(e, settings));

            var template = Resources.Load<TextAsset>("QuickEye/UXML Gen Script Template").text
                .Replace("#SCRIPT_NAME#", scriptName)
                .Replace("#PACKAGE_VERSION#", PackageInfo.Version);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#FIELDS#", fields);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#ASSIGNMENTS#", assignments);
            template = ScriptTemplateUtility.ReplaceNamespaceTags(template, CodeGeneration.GetNamespaceForFile(path));

            File.WriteAllText(path, template);

            AssetDatabase.Refresh();
        }

        public static string GetGenCsFilePath(string uxmlFilePath)
        {
            return uxmlFilePath.Replace(".uxml", ".gen.cs");
        }
    }
}