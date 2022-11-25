using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    [XmlRoot("UXML", Namespace = "UnityEngine.UIElements")]
    public class InlineSettings
    {
        [XmlAttribute("gen-cs-namespace")]
        public string CsNamespace;

        [XmlAttribute("gen-cs-file")]
        public string GenCsGuid;

        [XmlAttribute("gen-cs-private-field-prefix")]
        public string PrivateFieldPrefix;

        [XmlAttribute("gen-cs-private-field-suffix")]
        public string PrivateFieldSuffix;

        [XmlAttribute("gen-cs-private-field-style")]
        public string PrivateFieldStyle;


        [XmlAttribute("gen-cs-class-prefix")]
        public string ClassPrefix;

        [XmlAttribute("gen-cs-class-suffix")]
        public string ClassSuffix;

        [XmlAttribute("gen-cs-class-style")]
        public string ClassStyle;

        public static void test(string uxmlPath)
        {
            var root = FromXml(File.ReadAllText(uxmlPath));
            Debug.Log($"ns {root.CsNamespace}");
            root.GenCsGuid = null;
            root.WriteXmlAttributes(uxmlPath);
        }

        public static InlineSettings FromXml(string xml)
        {
            var stream = new StringReader(xml);

            var serializer = new XmlSerializer(typeof(InlineSettings));
            var root = (InlineSettings)serializer.Deserialize(stream);
            return root;
        }

        public void WriteXmlAttributes(string uxmlPath)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            if (root == null)
                return;
            foreach (var (attributeName, value) in GetSerializableAttributes())
            {
                if (value != null)
                    root.SetAttributeValue(attributeName, value);
                else
                    root.Attribute(attributeName)?.Remove();
            }

            Write(uxmlPath, root);
        }

        private (string attributeName, string value)[] GetSerializableAttributes()
        {
            var serializableAttributes = from field in GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
                let attr = field.GetCustomAttribute<XmlAttributeAttribute>()
                where attr != null
                select (attr.AttributeName, field.GetValue(this) as string);
            return serializableAttributes.ToArray();
        }


        private static void Write(string uxmlPath, XElement root)
        {
            using (var writer = new XmlTextWriter(uxmlPath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                root.WriteTo(writer);
            }
        }
    }
}