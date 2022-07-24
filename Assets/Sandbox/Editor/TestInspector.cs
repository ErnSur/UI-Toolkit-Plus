using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof(TestComponent))]
public class TestInspector : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        root.Add(new TestView().CreateUI());
        //InspectorElement.FillDefaultInspector(root, serializedObject, this);
        return root;
    }
}