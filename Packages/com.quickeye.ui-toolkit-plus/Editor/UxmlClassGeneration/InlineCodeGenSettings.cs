using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;

namespace QuickEye.UIToolkit.Editor
{
    internal class InlineCodeGenSettings
    {
        public const string CsNamespaceAttributeName = "code-gen-namespace";
        public string FieldPrefix;
        public readonly string CsNamespace;

        public InlineCodeGenSettings(string uxml)
        {
            var root = XDocument.Parse(uxml).Root;
            FieldPrefix = root?.Attribute("code-gen-prefix")?.Value;
            CsNamespace = root?.Attribute(CsNamespaceAttributeName)?.Value;
        }

        public static InlineCodeGenSettings FromUxml(string uxml)
        {
            return new InlineCodeGenSettings(uxml);
        }

        public static void SetSetting(string filePath, string key, string value)
        {
            var root = XDocument.Parse(File.ReadAllText(filePath)).Root;
            root.SetAttributeValue(key,value);
            using (var writer = new XmlTextWriter(filePath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                root.WriteTo(writer);
            }
        }
    }
}