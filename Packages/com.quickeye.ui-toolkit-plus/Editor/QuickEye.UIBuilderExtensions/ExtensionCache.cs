namespace QuickEye.UIBuilderExtensions
{
    using UnityEditor;
    using UnityEngine;

    internal class ExtensionCache : ScriptableObject
    {
        [MenuItem("Test/Show Cache")]
        private static void SelectCache()
        {
            Selection.activeObject = Instance;
        }

        public EditorWindow UIBuilderWindow;

        public static ExtensionCache Instance
        {
            get
            {
                if (_instance == null)
                    _instance = GetOrCreate();

                return _instance;
            }
        }

        private static ExtensionCache _instance;

        private static ExtensionCache GetOrCreate()
        {
            var existing = Resources.FindObjectsOfTypeAll<ExtensionCache>();
            if (existing.Length > 0)
            {
                return existing[0];
            }

            return CreateInstance<ExtensionCache>();
        }
    }
}