using System;
using UnityEngine;

namespace QuickEye.UxmlBridgeGen
{
    [Serializable]
    internal class CodeStyleRules
    {
        [Header("Code Style")]
        public MemberIdentifierStyle className = new MemberIdentifierStyle()
        {
            style = CaseStyle.UpperCamelCase
        };

        public MemberIdentifierStyle privateField = new MemberIdentifierStyle()
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

        private static MemberIdentifierStyle AddOverrides(MemberIdentifierStyle baseStyle,
            MemberIdentifierStyle overrides)
        {
            return new MemberIdentifierStyle
            {
                prefix = overrides.prefix ?? baseStyle.prefix,
                suffix = overrides.suffix ?? baseStyle.suffix,
                style = overrides.style != CaseStyle.NotSet ? overrides.style : baseStyle.style,
            };
        }
    }
}