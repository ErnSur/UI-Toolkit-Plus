#if UNITY_2021_1_OR_NEWER
using System.Linq;
using QuickEye.UIToolkit.Sandbox;
using UnityEngine;
using UnityEngine.UIElements;

public class RuntimeUi : MonoBehaviour
{
    private TabView _tabView;
    private void Start()
    {
        var uiDoc = GetComponent<UIDocument>();
        _tabView = new TabView(uiDoc.rootVisualElement);
    }
}
#endif