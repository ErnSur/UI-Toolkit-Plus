using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    public partial class TabView
    {
        public TabView(VisualElement root)
        {
            AssignQueryResults(root);
            verticalTabsToggle.RegisterValueChangedCallback(e =>
            {
                tabGroup.Mode = e.newValue ? TabGroupMode.Vertical : TabGroupMode.Horizontal;
                viewRoot.EnableInClassList("vertical-tabs-layout", e.newValue);
                viewRoot.EnableInClassList("horizontal-tabs-layout", !e.newValue);
            });

            foreach (var tab in root.Query<Tab>().Build())
            {
                var content = new Label($"{tab.Text} Content");
                content.AddToClassList("tab-content");
                tab.TabContent = content;
                tabContentContainer.Add(content);
            }

            root.Q<Tab>().value = true;
            tab1.BeforeMenuShow += PopulateSceneDropdown;
        }

        private void PopulateSceneDropdown(IGenericMenu menu)
        {
            menu.AddItem("Reorderable", tab1.IsReorderable, () => { tab1.IsReorderable = !tab1.IsReorderable;  });
        }
    }
}