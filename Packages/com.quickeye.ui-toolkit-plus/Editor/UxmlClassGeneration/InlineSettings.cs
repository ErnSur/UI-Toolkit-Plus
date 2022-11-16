using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace QuickEye.UIToolkit.Editor
{
    internal static class InlineSettings
    {
        private const string PrefixAttrNameSuffix = "prefix";
        private const string SuffixAttrNameSuffix = "suffix";
        private const string StyleAttrNameSuffix = "style";

        private const string AttributePrefixBase = "gen-cs";
        private const string FieldAttributeName = "private-field";
        private const string ClassAttributeName = "class";
        public const string CsNamespaceAttributeName = AttributePrefixBase + "-namespace";

        public static string GetCsNamespace(string uxmlPath)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            return GetCsNamespace(root);
        }

        private static string GetCsNamespace(XElement root)
        {
            return root?.Attribute(CsNamespaceAttributeName)?.Value;
        }

        public static void WriteCsNamespace(string uxmlPath, string csNamespace)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            if (root == null)
                return;
            if (csNamespace != null)
                root.SetAttributeValue(CsNamespaceAttributeName, csNamespace);
            else
                root.Attribute(CsNamespaceAttributeName)?.Remove();
            Write(uxmlPath, root);
        }

        public static CodeStyleRules GetCodeStyleRules(string uxml)
        {
            var root = XDocument.Parse(uxml).Root;

            return new CodeStyleRules
            {
                privateField = GetMemberIdentifierSettingsFromXml(root, FieldAttributeName),
                className = GetMemberIdentifierSettingsFromXml(root, ClassAttributeName)
            };
        }

        public static void WriteCodeStyleRules(string uxmlPath, CodeStyleRules settings)
        {
            var root = XDocument.Parse(File.ReadAllText(uxmlPath)).Root;
            if (root == null)
                return;
            SetMemberIdentifierSettingsToXml(root, settings.className, ClassAttributeName);
            SetMemberIdentifierSettingsToXml(root, settings.privateField, FieldAttributeName);

            Write(uxmlPath, root);
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
            string.Join('-', AttributePrefixBase, memberName, PrefixAttrNameSuffix);

        private static string GetSuffixAttrName(string memberName) =>
            string.Join('-', AttributePrefixBase, memberName, SuffixAttrNameSuffix);

        private static string GetStyleAttrName(string memberName) =>
            string.Join('-', AttributePrefixBase, memberName, StyleAttrNameSuffix);
    }
}