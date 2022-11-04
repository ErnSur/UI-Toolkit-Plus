// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 2.0.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UssExporter
{
    partial class StyleSheetExporter
    {
        private UnityEditor.UIElements.ToolbarSearchField searchField;
        private TreeView treeView;
    
        protected void AssignQueryResults(VisualElement root)
        {
            searchField = root.Q<UnityEditor.UIElements.ToolbarSearchField>("search-field");
            treeView = root.Q<TreeView>("tree-view");
        }
    }
}
