using System.Xml.Linq;

namespace QuickEye.UIToolkit.Editor
{
    internal class CodeGenSettings
    {
        public string FieldPrefix;

        //public enum fieldNameCapitalization;
        private CodeGenSettings() { }

        public static CodeGenSettings FromUxml(string uxml)
        {
            var root = XDocument.Parse(uxml).Root;
            return new CodeGenSettings
            {
                FieldPrefix = root?.Attribute("code-gen-prefix")?.Value
            };
        }
    }
}