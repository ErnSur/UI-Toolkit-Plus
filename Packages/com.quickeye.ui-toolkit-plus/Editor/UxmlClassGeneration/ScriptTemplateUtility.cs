using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuickEye.UIToolkit.Editor.UxmlClassGeneration
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

        public static string ReplaceNamespaceTags(string templateContent, string @namespace)
        {
            const string namespaceStartTag = "#NAMESPACE_START#";
            const string namespaceEndTag = "#NAMESPACE_END#";

            if (!templateContent.Contains(namespaceStartTag) || !templateContent.Contains(namespaceEndTag))
                return templateContent;

            if (string.IsNullOrEmpty(@namespace))
            {
                templateContent = Regex.Replace(templateContent, $"((\\r\\n)|\\n)[ \\t]*{namespaceStartTag}[ \\t]*",
                    string.Empty);
                templateContent = Regex.Replace(templateContent, $"((\\r\\n)|\\n)[ \\t]*{namespaceEndTag}[ \\t]*",
                    string.Empty);

                return templateContent;
            }

            // Use first found newline character as newline for entire file after replace.
            var newline = templateContent.Contains("\r\n") ? "\r\n" : "\n";
            var contentLines =
                new List<string>(templateContent.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None));

            int i = 0;

            for (; i < contentLines.Count; ++i)
            {
                if (contentLines[i].Contains(namespaceStartTag))
                    break;
            }

            var startTagLine = contentLines[i];

            // Use the whitespace between beginning of line and #NAMESPACE_START# as identation.
            var indentationString = startTagLine.Substring(0, startTagLine.IndexOf("#", StringComparison.Ordinal));

            contentLines[i] = $"namespace {@namespace}";
            contentLines.Insert(i + 1, "{");

            i += 2;

            for (; i < contentLines.Count; ++i)
            {
                var line = contentLines[i];

                if (String.IsNullOrEmpty(line) || line.Trim().Length == 0)
                    continue;

                if (line.Contains(namespaceEndTag))
                {
                    contentLines[i] = "}";
                    break;
                }

                contentLines[i] = $"{indentationString}{line}";
            }

            return string.Join(newline, contentLines.ToArray());
        }
    }
}