using System;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    [Serializable]
    internal class CodeGenSettings
    {
        [NonSerialized]
        public string csNamespace;

        [Header("Code Style")]
        public MemberIdentifierSettings className = new MemberIdentifierSettings()
        {
            style = CaseStyle.UpperCamelCase
        };

        public MemberIdentifierSettings privateField = new MemberIdentifierSettings()
        {
            style = CaseStyle.LowerCamelCase
        };
        
        public CodeGenSettings AddChangesTo(CodeGenSettings settings)
        {
            return new CodeGenSettings
            {
                className = AddChanges(settings.className, className),
                privateField = AddChanges(settings.privateField, privateField),
                csNamespace = csNamespace ?? settings.csNamespace
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