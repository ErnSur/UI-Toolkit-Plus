using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace QuickEye.UxmlBridgeGen
{
    internal static class ScriptTemplateUtility
    {
        public static string ReplaceTagWithIndentedMultiline(string templateContent, string tag,
            IEnumerable<string> newLines)
        {
            var tagWithIndentRegex = $"\\ *{tag}";
            var match = Regex.Match(templateContent, tagWithIndentRegex);
            var indent = match.Value[..^tag.Length];
            var replacement = string.Join(Environment.NewLine + indent, newLines);
            return Regex.Replace(templateContent, tag, replacement);
        }

        public static string ReplaceClassNameTag(string templateContent, string className)
        {
            return templateContent.Replace("#SCRIPT_NAME#", className);
        }

        public static string ReplacePackageVersionNameTag(string templateContent, string packageVersion)
        {
            return templateContent.Replace("#PACKAGE_VERSION#", packageVersion);
        }

        public static string ReplaceNamespaceTags(string templateContent, string namespaceName)
        {
            const string namespaceStartTag = "#NAMESPACE_START#";
            const string namespaceEndTag = "#NAMESPACE_END#";
            const string endOfLineAndSpaces = @"((\r\n)|\n)\ *";

            if (!templateContent.Contains(namespaceStartTag) || !templateContent.Contains(namespaceEndTag))
                return templateContent;

            if (string.IsNullOrEmpty(namespaceName))
            {
                templateContent = Regex.Replace(templateContent, $"{endOfLineAndSpaces}{namespaceStartTag}",
                    string.Empty);
                templateContent = Regex.Replace(templateContent, $"{endOfLineAndSpaces}{namespaceEndTag}",
                    string.Empty);

                return templateContent;
            }

            var sb = new StringBuilder();
            var namespaceIndent = string.Empty;
            using (var sr = new StringReader(templateContent))
            {
                while (sr.ReadLine() is { } line)
                {
                    if (line.Contains(namespaceStartTag))
                    {
                        namespaceIndent = line[..^namespaceStartTag.Length];
                        sb.AppendLine($"namespace {namespaceName}");
                        sb.AppendLine("{");
                        continue;
                    }

                    if (line.Contains(namespaceEndTag))
                    {
                        sb.AppendLine(line.Replace(namespaceEndTag, "}"));
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(line))
                        sb.Append(namespaceIndent);
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }
    }
}