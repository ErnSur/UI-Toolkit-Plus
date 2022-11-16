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

        private static readonly string[] IgnoredTagFullnames =
        {
            "UnityEngine.UIElements.Template",
            "Style"
        };

        private const string ColumnFullName = "UnityEngine.UIElements.Column";

        [MenuItem(GenerateCsMenuItemName)]
        private static void GenerateGenCs(MenuCommand command)
        {
            var asset = command.context as VisualTreeAsset;
            GenerateGenCs(asset, true);
        }

        [MenuItem(GenerateCsMenuItemName, true)]
        private static bool GenerateGenCsValidation(MenuCommand command)
        {
            return AssetDatabase.GetAssetPath(command.context).EndsWith(".uxml");
        }

        public static void GenerateGenCs(VisualTreeAsset uxmlAsset, bool pingAsset)
        {
            var uxmlFilePath = AssetDatabase.GetAssetPath(uxmlAsset);
            var uxml = File.ReadAllText(uxmlFilePath);

            var codeStyleRules = CsNamespaceUtils.GetFinalCodeStyleRulesFor(uxmlFilePath);

            if (!UxmlParser.TryGetElementsWithName(uxml, out var elements))
                return;

            var validElements = elements
                .Where(e => !IgnoredTagFullnames.Contains(e.FullyQualifiedTypeName))
                .ToArray();

            var genCsFilePath = GetGenCsFilePath(uxmlFilePath);

            var newScriptContent = CreateScriptContent(
                className: uxmlAsset.name,
                classNamespace: CsNamespaceUtils.GetCsNamespace(uxmlFilePath, out _),
                uxmlElements: validElements,
                codeStyle: codeStyleRules);

            if (File.Exists(genCsFilePath) &&
                !IsEqualWithoutComments(File.ReadAllText(genCsFilePath), newScriptContent))
                return;

            File.WriteAllText(genCsFilePath, newScriptContent);
            AssetDatabase.ImportAsset(genCsFilePath);
            SetIcon(genCsFilePath);
            if (pingAsset)
                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<MonoScript>(genCsFilePath));
        }

        private static void SetIcon(string path)
        {
            var monoImporter = AssetImporter.GetAtPath(path) as MonoImporter;
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>(
                "Packages/com.quickeye.ui-toolkit-plus/Editor/Icons/gen.cs Icon.png");

            monoImporter.SetIcon(icon);
            monoImporter.SaveAndReimport();
        }

        private static string GetFieldDeclaration(UxmlElement element, CodeStyleRules codeStyleRules)
        {
            var type = element.IsUnityEngineType ? element.TypeName : element.FullyQualifiedTypeName;
            var fieldIdentifier = codeStyleRules.privateField.Apply(element.NameAttribute);
            return $"private {type} {fieldIdentifier};";
        }

        private static string GetFieldAssigment(UxmlElement element, CodeStyleRules codeStyleRules)
        {
            var type = element.IsUnityEngineType ? element.TypeName : element.FullyQualifiedTypeName;
            var name = element.NameAttribute;
            var varName = codeStyleRules.privateField.Apply(name);

            if (element.FullyQualifiedTypeName == ColumnFullName)
            {
                return GetFieldAssigmentForColumn(element, varName, name, codeStyleRules);
            }

            return $"{varName} = root.Q<{type}>(\"{name}\");";
        }

        private static string GetFieldAssigmentForColumn(UxmlElement element, string varName, string name,
            CodeStyleRules codeStyleRules)
        {
            var multiColumnElement = element.XElement.Parent?.Parent?.ToUxmlElement();

            if (string.IsNullOrEmpty(multiColumnElement?.NameAttribute))
                return $"// Could not find \"{name}\" MultiColumn parent with a name.";

            var multiColumnEleVarName = codeStyleRules.privateField.Apply(multiColumnElement.NameAttribute);
            return $"{varName} = {multiColumnEleVarName}.columns[\"{name}\"];";
        }

        private static string CreateScriptContent(string className, string classNamespace, UxmlElement[] uxmlElements,
            CodeStyleRules codeStyle)
        {
            var fields = uxmlElements.Select(e => GetFieldDeclaration(e, codeStyle));
            var assignments = uxmlElements.Select(e => GetFieldAssigment(e, codeStyle));

            var template = Resources.Load<TextAsset>("QuickEye/UXMLGenScriptTemplate").text
                .Replace("#SCRIPT_NAME#", codeStyle.className.Apply(className))
                .Replace("#PACKAGE_VERSION#", PackageInfo.Version);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#FIELDS#", fields);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#ASSIGNMENTS#", assignments);
            template = ScriptTemplateUtility.ReplaceNamespaceTags(template, classNamespace);
            return template;
        }

        private static bool IsEqualWithoutComments(string oldContent, string newContent)
        {
            return TrimStartComments(oldContent) != TrimStartComments(newContent);
        }

        private static string TrimStartComments(string content)
        {
            using var reader = new StringReader(content);
            while ((char)reader.Peek() == '/')
                reader.ReadLine();
            return reader.ReadToEnd();
        }

        public static string GetGenCsFilePath(string uxmlFilePath)
        {
            return uxmlFilePath.Replace(".uxml", ".gen.cs");
        }
    }
}