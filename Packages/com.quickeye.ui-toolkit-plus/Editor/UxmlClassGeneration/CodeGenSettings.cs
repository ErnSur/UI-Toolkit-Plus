using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    internal enum CaseStyle
    {
        [InspectorName("lowerCamelCase")]
        LowerCamelCase,

        [InspectorName("UpperCamelCase")]
        UpperCamelCase
    }

    [FilePath("ProjectSettings/UxmlCodeGenSettings.asset", FilePathAttribute.Location.ProjectFolder)]
    internal class CodeGenSettings : ScriptableSingleton<CodeGenSettings>
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

        public void Save() => Save(true);

        private void OnEnable()
        {
            if (hideFlags.HasFlag(HideFlags.NotEditable))
            {
                hideFlags ^= HideFlags.NotEditable;
            }
        }

        [Serializable]
        public class MemberIdentifierSettings
        {
            public string prefix;
            public string suffix;
            public CaseStyle style;

            public string ApplyStyle(string identifier)
            {
                return $"{prefix}{identifier}{suffix}";
            }
        }
    }


    [CustomEditor(typeof(CodeGenSettings))]
    internal class CodeGenSettingsEditor : UnityEditor.Editor
    {
        private const string ProjectUxmlCodeGeneration = "Project/UXML Code Generation";

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var instance = CodeGenSettings.instance;
            var provider = AssetSettingsProvider.CreateProviderFromObject(
                ProjectUxmlCodeGeneration, instance,
                SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(instance)));
            return provider;
        }

        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(ProjectUxmlCodeGeneration);
        }

        public override void OnInspectorGUI()
        {
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                DrawPropertiesExcluding(serializedObject, "m_Script");
                if (changeScope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    CodeGenSettings.instance.Save();
                }
            }
        }
    }
}