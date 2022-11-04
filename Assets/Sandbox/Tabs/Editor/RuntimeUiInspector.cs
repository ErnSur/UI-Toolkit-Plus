using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    //[CustomEditor(typeof(RuntimeUi))]
    public class RuntimeUiInspector : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset tabsTemplate;

        [SerializeField]
        private VisualTreeAsset inspectorTemplate;

        private RuntimeUiInspectorView _inspectorView;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.Add(tabsTemplate.CloneTree());
            root.Add(inspectorTemplate.CloneTree());
            _inspectorView = new RuntimeUiInspectorView(root);
            return root;
        }

        public override bool UseDefaultMargins() => false;
    }
}