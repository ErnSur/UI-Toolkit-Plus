using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    public partial class TabView
    {
        public TabView(VisualElement root)
        {
            AssignQueryResults(root);
            el1.AddManipulator(new Reorderable());
            el2.AddManipulator(new Reorderable());
            tabDropdown1.BeforeMenuShow += menu =>
            {
                menu.AddItem("Hello, World!",false,()=>{ Debug.Log($"Hello, World!");});
            };
        }
    }
}