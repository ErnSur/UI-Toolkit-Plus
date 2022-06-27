using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class TestComponent : MonoBehaviour
{
    private void Start()
    {
        var uiDoc = GetComponent<UIDocument>();
        uiDoc.rootVisualElement.Add(new TestView().CreateUI());
    }
}

public class TestView
{
    public VisualElement CreateUI()
    {
        var group = new TabGroup();
        var tab = new TabDropdown();
        tab.text = "One";
        tab.BeforeMenuShow += menu =>
        {
            menu.AddItem("sdsssssśsssssssssssd", false,() => Debug.Log("sd"));
        };
        group.Add(tab);
        group.Add(new Tab());
        return group;
    }
}