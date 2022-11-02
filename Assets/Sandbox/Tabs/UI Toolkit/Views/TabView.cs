using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Sandbox
{
    public partial class TabView
    {
        public TabView(VisualElement root)
        {
            AssignQueryResults(root);
            horTabDropdown1.BeforeMenuShow += PopulateDropdown;
            vertTabDropdown1.BeforeMenuShow += PopulateDropdown;
        }

        private static void PopulateDropdown(IGenericMenu menu)
        {
            menu.AddItem("Hello, World!", false, () => { Debug.Log($"Hello, World!"); });
        }
    }
}