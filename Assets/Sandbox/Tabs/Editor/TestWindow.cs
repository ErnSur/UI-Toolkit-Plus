using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TestWindow : EditorWindow
{
    [MenuItem("Test/Sandbox Window")]
    private static void open() => GetWindow<TestWindow>();

    [SerializeField]
    private TestComponent testComponentPrefab;

    private void CreateGUI()
    {
        var root = new VisualElement();
        root.Add(new TestView().CreateUI());
        var inspectorElement = new InspectorElement(testComponentPrefab);
        root.Add(inspectorElement);
        
        rootVisualElement.Add(root);
    }
}