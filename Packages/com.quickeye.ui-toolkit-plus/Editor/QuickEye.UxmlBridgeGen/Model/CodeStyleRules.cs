using System;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    [Serializable]
    internal class CodeStyleRules
    {
        [Header("Code Style")]
        public MemberIdentifierSettings className = new MemberIdentifierSettings()
        {
            style = CaseStyle.UpperCamelCase
        };

        public MemberIdentifierSettings privateField = new MemberIdentifierSettings()
        {
            style = CaseStyle.LowerCamelCase
        };
        
        public CodeStyleRules Override(CodeStyleRules settings)
        {
            return new CodeStyleRules
            {
                className = AddOverrides(settings.className, className),
                privateField = AddOverrides(settings.privateField, privateField),
            };
        }

        private static MemberIdentifierSettings AddOverrides(MemberIdentifierSettings baseSettings,
            MemberIdentifierSettings overrides)
        {
            return new MemberIdentifierSettings
            {
                prefix = overrides.prefix ?? baseSettings.prefix,
                suffix = overrides.suffix ?? baseSettings.suffix,
                style = overrides.style != CaseStyle.NotSet ? overrides.style : baseSettings.style,
            };
        }
    }
}