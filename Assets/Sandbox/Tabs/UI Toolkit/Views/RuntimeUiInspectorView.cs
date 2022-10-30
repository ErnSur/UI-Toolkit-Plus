using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    public partial class RuntimeUiInspectorView
    {
        public RuntimeUiInspectorView(VisualElement root)
        {
            root.AssignQueryResults(root);
            
        }
    }
}