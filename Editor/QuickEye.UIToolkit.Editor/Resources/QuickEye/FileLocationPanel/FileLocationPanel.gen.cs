// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 3.0.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    partial class FileLocationPanel
    {
        private VisualElement fieldsContainer;
        private TextField pathField;
        private Button pathButton;
        private QuickEye.UIToolkit.FileNameField nameField;
        private VisualElement buttonContainer;
        private Button cancelButton;
        private Button addButton;
    
        protected void AssignQueryResults(VisualElement root)
        {
            fieldsContainer = root.Q<VisualElement>("fields-container");
            pathField = root.Q<TextField>("path-field");
            pathButton = root.Q<Button>("path-button");
            nameField = root.Q<QuickEye.UIToolkit.FileNameField>("name-field");
            buttonContainer = root.Q<VisualElement>("button-container");
            cancelButton = root.Q<Button>("cancel-button");
            addButton = root.Q<Button>("add-button");
        }
    }
}
