using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    public partial class TabView
    {
        public TabView(VisualElement root)
        {
            AssignQueryResults(root);
            horTabDropdown1.BeforeMenuShow += PopulateSceneDropdown;
            vertTabDropdown1.BeforeMenuShow += PopulateSceneDropdown;
        }

        private static void PopulateSceneDropdown(IGenericMenu menu)
        {
            menu.AddItem("Hello, World!", false, () => { Debug.Log($"Hello, World!"); });
        }
    }
}