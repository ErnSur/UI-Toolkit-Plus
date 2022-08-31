using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.CodeGen
{
    public partial class MainMenuView : MonoBehaviour
    {
        [SerializeField]
        private UIDocument uiDoc;

        private void OnEnable()
        {
            AssignQueryResults(uiDoc.rootVisualElement);
            titleLabel.text = "Super Game";
            quitButton.clicked += () => Debug.Log("Quit Game");
        }
    }
}