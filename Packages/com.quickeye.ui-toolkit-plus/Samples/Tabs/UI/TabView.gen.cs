// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 1.9.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    partial class TabView
    {
        private QuickEye.UIToolkit.TabGroup tabGroupVertical;
        private QuickEye.UIToolkit.TabDropdown vertTabDropdown1;
        private QuickEye.UIToolkit.TabGroup tabGroupHorizontal;
        private QuickEye.UIToolkit.TabDropdown horTabDropdown1;
        private QuickEye.UIToolkit.TabDropdown horTabDropdown2;
    
        protected void AssignQueryResults(VisualElement root)
        {
            tabGroupVertical = root.Q<QuickEye.UIToolkit.TabGroup>("tab-group-vertical");
            vertTabDropdown1 = root.Q<QuickEye.UIToolkit.TabDropdown>("vert-tab-dropdown-1");
            tabGroupHorizontal = root.Q<QuickEye.UIToolkit.TabGroup>("tab-group-horizontal");
            horTabDropdown1 = root.Q<QuickEye.UIToolkit.TabDropdown>("hor-tab-dropdown-1");
            horTabDropdown2 = root.Q<QuickEye.UIToolkit.TabDropdown>("hor-tab-dropdown-2");
        }
    }
}
