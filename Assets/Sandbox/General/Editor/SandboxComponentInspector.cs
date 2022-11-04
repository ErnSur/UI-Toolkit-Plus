using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    [CustomEditor(typeof(SandboxComponent))]
    public class SandboxComponentInspector : UnityEditor.Editor
    {
        [SerializeField]
        private VisualTreeAsset inspectorTemplate;


        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            root.Add(inspectorTemplate.CloneTree());
            return root;
        }

        public override bool UseDefaultMargins() => false;
    }
}