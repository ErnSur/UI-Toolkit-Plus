// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 1.9.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    partial class FileLocationPanel
    {
        private VisualElement _fieldsContainer;
        private TextField _pathField;
        private Button _pathButton;
        private QuickEye.UIToolkit.FileNameField _nameField;
        private VisualElement _buttonContainer;
        private Button _cancelButton;
        private Button _addButton;
    
        protected void AssignQueryResults(VisualElement root)
        {
            _fieldsContainer = root.Q<VisualElement>("fields-container");
            _pathField = root.Q<TextField>("path-field");
            _pathButton = root.Q<Button>("path-button");
            _nameField = root.Q<QuickEye.UIToolkit.FileNameField>("name-field");
            _buttonContainer = root.Q<VisualElement>("button-container");
            _cancelButton = root.Q<Button>("cancel-button");
            _addButton = root.Q<Button>("add-button");
        }
    }
}
