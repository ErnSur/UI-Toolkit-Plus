// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 1.5.0
// -----------------------
using UnityEngine.UIElements;

namespace SampleAsmDefName
{
    partial class CodeGenExample
    {
        private Label _title;
        private VisualElement _menu;
        private Button _confirmButton;
        private QuickEye.UIToolkit.Tab _normalTab;
        private QuickEye.UIToolkit.TabGroup _dropTab;
        private UnityEditor.UIElements.Toolbar _toolbar;
    
        protected void AssignQueryResults(VisualElement root)
        {
            _title = root.Q<Label>("title");
            _menu = root.Q<VisualElement>("menu");
            _confirmButton = root.Q<Button>("confirm-button");
            _normalTab = root.Q<QuickEye.UIToolkit.Tab>("normal-tab");
            _dropTab = root.Q<QuickEye.UIToolkit.TabGroup>("drop--tab");
            _toolbar = root.Q<UnityEditor.UIElements.Toolbar>("toolbar");
        }
    }
}
