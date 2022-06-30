using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class TestView
{
    public VisualElement CreateUI()
    {
        var root = new VisualElement();
        var group = new TabGroup();

        for (int i = 0; i < 5; i++)
        {
            var t =  CreateButtonWithHandle($"Ele {i}");
            t.AddManipulator(new Reorderable());
            group.Add(t);
        }

        root.Add(group);
        return root;
    }

    private VisualElement CreateButtonWithHandle(string name)
    {
        var wrapper = new VisualElement();
        wrapper.style.flexDirection = FlexDirection.Row;
        
        var handle = new VisualElement();
        handle.style.width = 10;
        handle.style.backgroundColor = Color.cyan;

        var button = new Button(() => { Debug.Log($"Button: {name}"); });
        button.text = name;
        
        wrapper.Add(handle);
        wrapper.Add(button);
        return wrapper;
    }
}