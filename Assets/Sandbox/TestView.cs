using QuickEye.UIToolkit;
using UnityEngine.UIElements;

public class TestView
{
    public VisualElement CreateUI()
    {
        var root = new VisualElement();
        var group = new TabGroup();

        for (int i = 0; i < 5; i++)
        {
            var t = new Tab();
            t.Reorderable = i != 2 && i != 4;
            t.Text = $"Tab {i}";
            group.Add(t);
        }

        root.Add(group);
        return root;
    }
}