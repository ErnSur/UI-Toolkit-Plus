using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace QuickEye.UIToolkit.Editor
{
    internal static class CodeGenSettingsSerializer
    {
        private const string PrefixAttrNameSuffix = "-prefix";
        private const string SuffixAttrNameSuffix = "-suffix";
        private const string StyleAttrNameSuffix = "-style";
        
        private const string AttributePrefixBase = "code-gen-";
        private const string FieldAttributeName = "field";
        private const string ClassAttributeName = "class";
        public const string CsNamespaceAttributeName = AttributePrefixBase + "-namespace";

        public static CodeGenSettings FromUxml(string uxml)
        {
            var root = XDocument.Parse(uxml).Root;

            return new CodeGenSettings
            {
                csNamespace = root?.Attribute(CsNamespaceAttributeName)?.Value,
                privateField = GetMemberIdentifierSettingsFromXml(root, FieldAttributeName),
                className = GetMemberIdentifierSettingsFromXml(root, ClassAttributeName)
            };
        }

        public static void SaveTo(CodeGenSettings settings, string uxmlPath)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            if (root == null)
                return;
            SetMemberIdentifierSettingsToXml(root, settings.className, ClassAttributeName);
            SetMemberIdentifierSettingsToXml(root, settings.privateField, FieldAttributeName);
            if (settings.csNamespace != null)
                root.SetAttributeValue(CsNamespaceAttributeName, settings.csNamespace);
            else
                root.Attribute(CsNamespaceAttributeName)?.Remove();
            
            using (var writer = new XmlTextWriter(uxmlPath, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                root.WriteTo(writer);
            }
        }

        private static MemberIdentifierSettings GetMemberIdentifierSettingsFromXml(XElement root, string memberName)
        {
            var res = new MemberIdentifierSettings
            {
                prefix = root.Attribute(GetPrefixAttrName(memberName))?.Value,
                suffix = root.Attribute(GetSuffixAttrName(memberName))?.Value,
            };

            Enum.TryParse(root.Attribute(GetStyleAttrName(memberName))?.Value, true, out res.style);
            return res;
        }

        private static void SetMemberIdentifierSettingsToXml(XElement root, MemberIdentifierSettings settings,
            string memberName)
        {
            if (settings.prefix != null)
                root.SetAttributeValue(GetPrefixAttrName(memberName), settings.prefix);
            else
                root.Attribute(GetPrefixAttrName(memberName))?.Remove();

            if (settings.suffix != null)
                root.SetAttributeValue(GetSuffixAttrName(memberName), settings.suffix);
            else
                root.Attribute(GetSuffixAttrName(memberName))?.Remove();

            if (settings.style != CaseStyle.NotSet)
                root.SetAttributeValue(GetStyleAttrName(memberName), settings.style.ToString());
            else
                root.Attribute(GetStyleAttrName(memberName))?.Remove();
        }

        private static string GetPrefixAttrName(string memberName) =>
            AttributePrefixBase + memberName + PrefixAttrNameSuffix;

        private static string GetSuffixAttrName(string memberName) =>
            AttributePrefixBase + memberName + SuffixAttrNameSuffix;

        private static string GetStyleAttrName(string memberName) =>
            AttributePrefixBase + memberName + StyleAttrNameSuffix;
    }
}