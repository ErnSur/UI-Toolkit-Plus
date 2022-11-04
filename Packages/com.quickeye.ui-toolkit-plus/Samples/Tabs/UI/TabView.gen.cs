// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 2.0.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    partial class TabView
    {
        private VisualElement viewRoot;
        private VisualElement tabBar;
        private Toggle verticalTabsToggle;
        private QuickEye.UIToolkit.TabGroup tabGroup;
        private QuickEye.UIToolkit.TabDropdown tab1;
        private VisualElement tabContentContainer;
    
        protected void AssignQueryResults(VisualElement root)
        {
            viewRoot = root.Q<VisualElement>("view-root");
            tabBar = root.Q<VisualElement>("tab-bar");
            verticalTabsToggle = root.Q<Toggle>("vertical-tabs-toggle");
            tabGroup = root.Q<QuickEye.UIToolkit.TabGroup>("tab-group");
            tab1 = root.Q<QuickEye.UIToolkit.TabDropdown>("tab1");
            tabContentContainer = root.Q<VisualElement>("tab-content-container");
        }
    }
}
