using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuickEye.UIToolkit.Editor
{
    internal static class TextUtils
    {
        public static string ToCamelCase(this string original)
        {
            var text = ToPascalCase(original);
            return $"{char.ToLowerInvariant(text[0])}{text.Substring(1)}";
        }

        public static string ToPascalCase(this string original)
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