using System;
using System.Collections.Generic;
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
    // TODO: Test on 2021.3
    [Serializable]
    public class InlineSettings
    {
        static readonly Dictionary<string, FieldInfo> SerializableFields;

        static InlineSettings()
        {
            SerializableFields =
                (from field in typeof(InlineSettings).GetFields(BindingFlags.Instance | BindingFlags.Public)
                    let attr = field.GetCustomAttribute<XmlAttributeAttribute>()
                    where attr != null
                    select (attr.AttributeName, field)).ToDictionary(t => t.AttributeName, t => t.field);
        }

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

        // Change it to TryGet, uxml file content can be invalid
        // In UI add HelBox messages that UXML could not be parsed, (possibly invalid UXML format)
        public static InlineSettings FromXmlFile(string xmlFilePath)
        {
            try
            {
                return FromXml(File.ReadAllText(xmlFilePath));
            }
            catch (Exception)
            {
                Debug.LogError($"Failed deserialize inline settings of {xmlFilePath}");
                throw;
            }
        }

        public static InlineSettings FromXml(string xml)
        {
            var inlineSettings = new InlineSettings();
            // Create a StringReader to read the XML string
            using (var sr = new StringReader(xml))
            {
                // Create an XmlReader to parse the XML string
                using (var xr = XmlReader.Create(sr))
                {
                    // Move to the root element
                    xr.MoveToContent();
                    foreach (var kvp in SerializableFields)
                    {
                        kvp.Value.SetValue(inlineSettings,xr.GetAttribute(kvp.Key));
                    }
                }
            }
            return inlineSettings;
        }

        public void WriteTo(string uxmlPath)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            if (root == null)
                return;
            AddTo(root);

            Write(uxmlPath, root);
        }

        public void AddTo(XElement uxmlRootElement)
        {
            foreach (var (attributeName, fieldInfo) in SerializableFields)
            {
                var value = fieldInfo.GetValue(this);
                if (value != null)
                    uxmlRootElement.SetAttributeValue(attributeName, value);
                else
                    uxmlRootElement.Attribute(attributeName)?.Remove();
            }
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