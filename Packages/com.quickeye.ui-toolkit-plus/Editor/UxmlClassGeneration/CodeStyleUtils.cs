using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CodeStyleUtils
    {
        public static string ApplyCaseStyle(this string text, CaseStyle style)
        {
            switch (style)
            {
                default:
                case CaseStyle.NotSet:
                    return text;
                case CaseStyle.LowerCamelCase:
                    return ToLowerCamelCase(text); 
                case CaseStyle.UpperCamelCase:
                    return ToUpperCamelCase(text);
            }
        }

        private static string ToLowerCamelCase(string original)
        {
            var text = ToUpperCamelCase(original);
            return $"{char.ToLowerInvariant(text[0])}{text.Substring(1)}";
        }

        private static string ToUpperCamelCase(string original)
        {
            var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            var startsWithLowerCaseChar = new Regex("^[a-z]");
            var startsWithNotLetter = new Regex("^[^a-zA-Z]+");
            var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");

            var pascalCase =
                original.Split(new char[] { '_', '-', '.', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    // replace all invalid chars with empty string
                    .Select(w => invalidCharsRgx.Replace(w, string.Empty))
                    // set first letter to uppercase
                    .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpperInvariant()))
                    // set upper case the first lower case following a number (Ab9cd -> Ab9Cd)
                    .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpperInvariant()));

            return startsWithNotLetter.Replace(string.Concat(pascalCase), string.Empty);
        }
    }
}