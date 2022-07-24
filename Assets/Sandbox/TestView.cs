using System;
using System.Collections.Generic;
using System.Linq;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

[Serializable]
public class TestView
{
    private List<string> model = new List<string>
    {
        "Plane", //0
        "Car", //1
        "-human", //2
        "-Dog", //3
        "cat", //4
        "fly", //5
    };

    public VisualElement CreateUI()
    {
        var root = new VisualElement();
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.Add(GetSpacer());
        var group = CreateTabBarAndBindModel();
        row.Add(group);
        row.Add(GetSpacer());

        var directionToggle = new Toggle();
        directionToggle.RegisterValueChangedCallback(evt =>
        {
            var group = row.Q<TabGroup>();
            group.Mode = evt.newValue ? TabGroupMode.Vertical : TabGroupMode.Horizontal;
            // group.style.alignItems = evt.newValue ? Align.FlexStart : Align.FlexEnd;
            // group.style.flexDirection = evt.newValue ? FlexDirection.Column : FlexDirection.Row;
        });
        root.Insert(0, directionToggle);
        root.Add(row);
 
        return root;
    }

    private void CreateScrollView(VisualElement root)
    {
        var sv = new ScrollView();
        sv.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        sv.mode = ScrollViewMode.Horizontal;
        var button = new RepeatButton(() => sv.horizontalScroller.value++, 0, 1);
        button.AddToClassList("unity-button");
        button.text = "++";
        for (int i = 0; i < 100; i++)
        {
            sv.Add(new Label($"Label {i}"));
        }

        root.Add(sv);
        root.Add(button);
    }

    private TabGroup CreateTabBarAndBindModel()
    {
        var group = new TabGroup { name = "Tab Group" };
        group.RegisterCallback<ChildOrderChangedEvent>(evt =>
        {
            model = group.Children().Select(e => (string)e.userData).ToList();
            //Debug.Log($"New Model:");
            foreach (var e in model)
            {
                //Debug.Log(e);
            }
        });

        foreach (var e in model)
        {
            var t = CreateTab(e, !e.StartsWith("-"));
            t.userData = e;

            group.Add(t);
        }

        return group;
    }

    private TabGroup CreateTabBar()
    {
        var group = new TabGroup { name = "Tab Group" };
        group.RegisterCallback<ChildOrderChangedEvent>(evt => { Debug.Log($"Order Changed: "); });
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