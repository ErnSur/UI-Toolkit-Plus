using System;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    [Serializable]
    internal class CodeGenerationSettings
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
    }

    [Serializable]
    internal struct MemberIdentifierSettings
    {
        public string prefix;
        public string suffix;
        public CaseStyle style;

        public void SaveToXml(string xmlPath) { }

        public string ApplyStyle(string identifier)
        {
            switch (style)
            {
                case CaseStyle.LowerCamelCase:
                    identifier = identifier.ToCamelCase();
                    break;
                case CaseStyle.UpperCamelCase:
                    identifier = identifier.ToPascalCase();
                    break;
            }

            return $"{prefix}{identifier}{suffix}";
        }
    }
}