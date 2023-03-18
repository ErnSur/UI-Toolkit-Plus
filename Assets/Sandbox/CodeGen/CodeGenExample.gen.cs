// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 3.0.3
// -----------------------
using UnityEngine.UIElements;

namespace SampleAsmDefName
{
    partial class CodeGenExample
    {
        private Label title;
        private VisualElement menu;
        private Button confirmButton;
        private QuickEye.UIToolkit.Tab normalTab;
        private QuickEye.UIToolkit.TabGroup dropTab;
    
        protected void AssignQueryResults(VisualElement root)
        {
            title = root.Q<Label>("title");
            menu = root.Q<VisualElement>("menu");
            confirmButton = root.Q<Button>("confirm-button");
            normalTab = root.Q<QuickEye.UIToolkit.Tab>("normal-tab");
            dropTab = root.Q<QuickEye.UIToolkit.TabGroup>("drop--tab");
        }
    }
}
