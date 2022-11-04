using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Tabs
{
    public class RuntimeUi : MonoBehaviour
    {
        private TabView _tabView;

        private void Start()
        {
            var uiDoc = GetComponent<UIDocument>();
            _tabView = new TabView(uiDoc.rootVisualElement);
        }
    }
}
