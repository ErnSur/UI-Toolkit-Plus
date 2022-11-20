using System;
using System.IO;
using UnityEditor;

namespace QuickEye.UxmlBridgeGen
{
    internal static class InlineSettingsUtils
    {
        public static bool TryGetGenCsFilePath(string uxmlPath, out string genCsPath, out bool isMissing)
        {
            var root = InlineSettings.FromXml(File.ReadAllText(uxmlPath));
            return root.TryGetGenCsFilePath(out genCsPath, out isMissing);
        }
        
        public static bool TryGetGenCsFilePath(this InlineSettings settings, out string genCsPath, out bool isMissing)
        {
            if (settings.GenCsGuid == null)
            {
                isMissing = false;
                genCsPath = null;
                return false;
            }

            genCsPath = AssetDatabase.GUIDToAssetPath(settings.GenCsGuid);
            if (string.IsNullOrEmpty(genCsPath))
            {
                isMissing = true;
                return false;
            }

            isMissing = false;
            return true;
        }
        
        public static string GetCsNamespace(string uxmlPath)
        {
            var root = InlineSettings.FromXml(File.ReadAllText(uxmlPath));
            return root.CsNamespace;
        }

        public static void WriteCsNamespace(string uxmlPath, string csNamespace)
        {
            var root = InlineSettings.FromXml(File.ReadAllText(uxmlPath));
            root.CsNamespace = csNamespace;
            root.WriteXmlAttributes(uxmlPath);
        }

        public static CodeStyleRules GetCodeStyleRules(string uxml)
        {
            var root = InlineSettings.FromXml(uxml);
            return GetCodeStyleRules(root);
        }
        
        public static CodeStyleRules GetCodeStyleRules(this InlineSettings settings)
        {
            return new CodeStyleRules
            {
                privateField = NewMemberStyleRules(settings.PrivateFieldPrefix, settings.PrivateFieldSuffix,
                    settings.PrivateFieldStyle),
                className = NewMemberStyleRules(settings.ClassPrefix, settings.ClassSuffix, settings.ClassStyle),
            };
        }

        private static MemberIdentifierStyle NewMemberStyleRules(string prefix, string suffix, string style)
        {
            var res = new MemberIdentifierStyle
            {
                prefix = prefix,
                suffix = suffix,
            };

            Enum.TryParse(style, true, out res.style);
            return res;
        }
    }
}