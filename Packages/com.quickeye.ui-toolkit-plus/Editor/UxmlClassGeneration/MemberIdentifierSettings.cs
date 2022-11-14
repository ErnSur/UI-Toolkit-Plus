using System;

namespace QuickEye.UIToolkit.Editor
{
    [Serializable]
    internal struct MemberIdentifierSettings
    {
        public string prefix;
        public string suffix;
        public CaseStyle style;
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