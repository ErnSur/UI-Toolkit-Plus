using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    partial class ReorderablesView
    {
        public ReorderablesView(VisualElement root)
        {
            el1.AddManipulator(new Reorderable());
            el2.AddManipulator(new Reorderable());
        }
    }
}