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
            var t = new Button();
           // var wrapper = new VisualElement();
            //wrapper.Add(t);
            //t.Reorderable = i != 2 && i != 4;
            t.text =t.name= $"Tab {i}";
            t.clickable = new Clickable(() => { Debug.Log($"MES: {t.name}"); });
            t.AddManipulator(new Reorderable());
            group.Add(t);
        }

        root.Add(group);
        return root;
    }
}