// -----------------------
// script auto-generated
// any changes to this file will be lost on next code generation
// com.quickeye.ui-toolkit-plus ver: 3.0.0
// -----------------------
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.CodeGen
{
    partial class MainMenuViewView
    {
        private Label titleLabel;
        private Button playButton;
        private Button settingsButton;
        private Button quitButton;
    
        protected void AssignQueryResults(VisualElement root)
        {
            titleLabel = root.Q<Label>("title-label");
            playButton = root.Q<Button>("play-button");
            settingsButton = root.Q<Button>("settings-button");
            quitButton = root.Q<Button>("quit-button");
        }
    }
}
