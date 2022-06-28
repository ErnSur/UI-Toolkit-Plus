#if UNITY_2021_1_OR_NEWER
using System.Linq;
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
#endif