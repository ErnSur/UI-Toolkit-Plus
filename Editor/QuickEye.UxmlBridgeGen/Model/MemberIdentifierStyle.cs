using System;

namespace QuickEye.UxmlBridgeGen
{
    [Serializable]
    internal struct MemberIdentifierStyle
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