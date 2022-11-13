using UnityEditor;

namespace QuickEye.UIToolkit.Editor
{
    [CustomEditor(typeof(CodeGenProjectSettings))]
    internal class CodeGenProjectSettingsEditor : UnityEditor.Editor
    {
        private const string ProjectUxmlCodeGeneration = "Project/UXML Code Generation";

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            var instance = CodeGenProjectSettings.instance;
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
                DrawPropertyChildren(serializedObject.FindProperty("settings"));
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