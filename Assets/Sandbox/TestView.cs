using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class TestView
{
    public VisualElement CreateUI()
    {
        var root = new VisualElement();
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.Add(GetSpacer());
        row.Add(CreateTabBar());
        row.Add(GetSpacer());

        root.Add(row);
        return root;
    }

    private TabGroup CreateTabBar()
    {
        var group = new TabGroup { name = "Tab Group" };
        //group.style.flexDirection = FlexDirection.Column;
        for (int i = 0; i < 3; i++)
        {
            var t = CreateTab($"Ele {i}", i != 1);

            group.Add(t);
        }

        for (int i = 0; i < 3; i++)
        {
            var t = CreateTabDropdown($"Ele {i}", i != 1);
            group.Add(t);
        }

        return group;
    }

    private VisualElement GetSpacer()
    {
        var x = new VisualElement()
        {
            style =
            {
                width = 10 + Random.Range(0, 3),
                height = 10 + Random.Range(0, 3),
                backgroundColor = Color.gray
            }
        };
        return x;
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
        wrapper.AddManipulator(new Reorderable());

        return wrapper;
    }

    private VisualElement CreateTab(string name, bool reorderable)
    {
        return new Tab(name) { name = name, IsReorderable = reorderable };
    }

    private VisualElement CreateTabDropdown(string name, bool reorderable)
    {
        return new TabDropdown(name) { name = name, IsReorderable = reorderable };
    }
}