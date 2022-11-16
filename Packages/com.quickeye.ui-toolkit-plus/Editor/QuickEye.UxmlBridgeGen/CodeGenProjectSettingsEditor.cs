using UnityEditor;

namespace QuickEye.UxmlBridgeGen
{
    [CustomEditor(typeof(CodeGenProjectSettings))]
    internal class CodeGenProjectSettingsEditor : UnityEditor.Editor
    {
        private const string SettingsPath = "Project/UXML-C# Code Generation";

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var instance = CodeGenProjectSettings.instance;
            var provider = AssetSettingsProvider.CreateProviderFromObject(
                SettingsPath, instance,
                SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(instance)));
            return provider;
        }

        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(SettingsPath);
        }

        public override void OnInspectorGUI()
        {
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var codeStyleProp = serializedObject.FindProperty("codeStyleRules");
                DrawPropertyChildren(codeStyleProp);
                if (changeScope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    CodeGenProjectSettings.instance.Save();
                }
            }
        }

        private static void DrawPropertyChildren(SerializedProperty prop)
        {
            var endProperty = prop.GetEndProperty();
            var childrenDepth = prop.depth + 1;
            while (prop.NextVisible(true) && !SerializedProperty.EqualContents(prop, endProperty))
            {
                if (prop.depth != childrenDepth)
                    continue;
                EditorGUILayout.PropertyField(prop, true);
            }
        }
    }
}