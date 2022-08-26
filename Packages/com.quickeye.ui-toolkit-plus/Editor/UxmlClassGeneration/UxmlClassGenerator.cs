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
        private static void GenerateBinding(MenuCommand command)
        {
            var asset = command.context as VisualTreeAsset;
            var path = AssetDatabase.GetAssetPath(asset);
            var uxml = File.ReadAllText(path);
            
            if (CodeGenUtility.TryGetNamedElementsFromUxml(uxml, out var elements))
            {
                var scriptPath = path.Replace(".uxml", ".gen.cs");

                GenerateScript(command.context.name, elements, scriptPath);
            }
        }

        private static void GenerateScript(string scriptName, (string type, string name)[] uxmlElements, string path)
        {
            var fields = uxmlElements
                .Select(t => $"private {t.type} {CodeGenUtility.UssNameToVariableName(t.name)};");
            var assignments = uxmlElements
                .Select(t =>
                {
                    var (type, name) = t;
                    var varName = CodeGenUtility.UssNameToVariableName(name);

                    return $"{varName} = root.Q<{type}>(\"{name}\");";
                });

            var template = Resources.Load<TextAsset>("QuickEye/UXML Gen Script Template").text
                .Replace("#SCRIPT_NAME#", scriptName);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#FIELDS#", fields);
            template = ScriptTemplateUtility.ReplaceTagWithIndentedMultiline(template, "#ASSIGNMENTS#", assignments);
            template = ScriptTemplateUtility.ReplaceNamespaceTags(template, CodeGenUtility.GetNamespaceForFile(path));

            File.WriteAllText(path, template);

            AssetDatabase.Refresh();
        }
    }
}