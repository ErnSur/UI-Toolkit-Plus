namespace QuickEye.UxmlBridgeGen
{
    using System;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;

    internal static class UIBuilderUtils
    {
        private static readonly Type UIBuilderType =
            Type.GetType("Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule");

        public static bool TryGetUIBuilderWindow(out EditorWindow window)
        {
            if (UIBuilderType == null)
            {
                Debug.LogWarning("Could not find UI Builder type.");
                window = null;
                return false;
            }

            var objectsOfTypeAll = Resources.FindObjectsOfTypeAll(UIBuilderType);
            window = objectsOfTypeAll.Cast<EditorWindow>().FirstOrDefault();

            return window;
        }
    }
}