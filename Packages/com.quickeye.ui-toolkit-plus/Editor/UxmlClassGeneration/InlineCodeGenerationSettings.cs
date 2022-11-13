using System;

namespace QuickEye.UIToolkit.Editor
{
    [Serializable]
    internal class InlineCodeGenerationSettings : CodeGenerationSettings
    {
        public string csNamespace;

        public CodeGenerationSettings AddChangesTo(CodeGenerationSettings settings)
        {
            return new CodeGenerationSettings
            {
                className = AddChanges(settings.className, className),
                privateField = AddChanges(settings.privateField, privateField),
            };
        }

        private static MemberIdentifierSettings AddChanges(MemberIdentifierSettings baseSettings,
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