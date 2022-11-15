using System;

namespace QuickEye.UIToolkit.Editor
{
    [Serializable]
    internal struct MemberIdentifierSettings
    {
        public string prefix;
        public string suffix;
        public CaseStyle style;
        public string Apply(string identifier)
        {
            return $"{prefix}{identifier.ApplyCaseStyle(style)}{suffix}";
        }
    }
}