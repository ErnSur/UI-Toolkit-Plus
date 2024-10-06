namespace QuickEye.UIBuilderExtensions
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.UIElements;

    internal static class OpenUIBuilder
    {
        private static readonly Type UIBuilderWindowType =
            Type.GetType("Unity.UI.Builder.Builder, UnityEditor.UIBuilderModule");

        [MenuItem("Test/Open UI Builder Plus")]
        public static void Open()
        {
            var window = FindOrCreateUIBuilder();
            ExtensionCache.Instance.UIBuilderWindow = window;
            InitializeUI(window);
        }

        /// <remarks>
        /// Cannot Get UI Builder from here though <see cref="FindOrCreateUIBuilder"/>
        /// </remarks>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            if (ExtensionCache.Instance.UIBuilderWindow != null)
                InitializeUI(ExtensionCache.Instance.UIBuilderWindow);
        }

        private static EditorWindow FindOrCreateUIBuilder()
        {
            var window = EditorWindow.GetWindow(UIBuilderWindowType);
            return window;
        }

        private static void InitializeUI(EditorWindow window)
        {
            if (window.rootVisualElement.panel != null)
                SafeCreateUI(window.rootVisualElement);
            else
            {
                // Debug.Log("Waiting for panel to attach");
                window.rootVisualElement.RegisterCallback<GeometryChangedEvent>(TryInitUIOnGeometryChange);
            }

            return;

            void TryInitUIOnGeometryChange(GeometryChangedEvent evt)
            {
                var root = window.rootVisualElement;
                if (root.Q("inspector") == null)
                    return;

                // Debug.Log("Found inspector");
                SafeCreateUI(window.rootVisualElement);
                root.UnregisterCallback<GeometryChangedEvent>(TryInitUIOnGeometryChange);
            }
        }

        private static void SafeCreateUI(VisualElement root)
        {
            try
            {
                CreateUI(root);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create UI");
                Debug.LogException(e);
            }
        }

        private static void CreateUI(VisualElement root)
        {
            Assert.IsNotNull(root, "Root is null");
            var inspector = root.Q("inspector");
            Assert.IsNotNull(inspector, "Inspector is null");

            InitializeInspector(inspector);
        }

        private static void InitializeInspector(VisualElement inspector)
        {
            var headerContainer = inspector.Q("header-container");
            var tabView = new BuilderInspectorTabView(inspector);
            headerContainer.parent.Add(tabView);
            tabView.PlaceInFront(headerContainer);
        }
    }
}