using System;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class TestView
{
    public VisualElement CreateUI()
    {
        var tree = Resources.Load<VisualTreeAsset>("ReorderableSandbox");
        var root= tree.CloneTree();
        foreach (var rord in root.Query(null,"rord-element").Build() )  
        { 
            rord.AddManipulator(new Reorderable());
        }
        foreach (var button in root.Query<Button>(null,"rord-element").Build() )
        {
            //Debug.Log($"MES: {VAR}");
            button.clicked += () => { Debug.Log($"Hello from button"); };
        }
        foreach (var tabDropdown in root.Query<TabDropdown>().Build() )
        {
            //Debug.Log($"MES: {tabDropdown}");
            tabDropdown.IsReorderable = true;
            tabDropdown.BeforeMenuShow += (m) => { m.AddItem("sd",false,()=>Debug.Log($"Hello from button")); };
        }
        return root;
    }
}