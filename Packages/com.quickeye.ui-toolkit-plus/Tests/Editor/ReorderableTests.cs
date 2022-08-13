using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuickEye.UIToolkit;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UIElements;

public class ReorderableTests
{
    private List<string> names;
    private VisualElement root;
    private TestWindow wnd;

    [SetUp]
    public void Setup()
    {
        wnd =EditorWindow.GetWindow<TestWindow>();

        names = new List<string>
        {
            "Plane", //0
            "Car", //1
            "-human", //2
            "-Dog", //3
            "cat", //4
            "fly", //5
        };
        root = new VisualElement();
        foreach (var name in names)
        {
            var e = new VisualElement() { name = name };
            if(!name.StartsWith("-"))
                e.AddManipulator(new Reorderable());
            root.Add(e);
        }
    }

    [TearDown]
    public void TearDown()
    {
        wnd.Close();
    }

    private List<string> RootToListOfNames()
    {
        return root.Children().Select(e => e.name).ToList();
    }


    [Test]
    public void Button_receives_callbacks()
    {
        var flag = false;
        //var button = new Button(()=> flag = true);
        var button = new VisualElement();
        button.RegisterCallback<ClickEvent>(evt=> flag = true);
        wnd.rootVisualElement.Add(button);

        SendEvent<ClickEvent>(button);
        Assert.IsTrue(flag);
    }
    
    [Test]
    public void Button_receives_callbacks2()
    {
        var flag = false;
        var button = new Button(()=> flag = true);
        wnd.rootVisualElement.Add(button);

        var buttonCentre = Vector2.one;
        var evt1 = new Event()
        {
            type = EventType.MouseDown,
            button = 0,
            mousePosition = buttonCentre,
            clickCount = 1
        };
     
        using (var mouseDownEvent = MouseDownEvent.GetPooled(evt1))
        {
            button.SendEvent(mouseDownEvent);
        }
         
        var evt2 = new Event()
        {
            type = EventType.MouseUp,
            button = 0,
            mousePosition = buttonCentre,
            clickCount = 1
        };
 
        using (var mouseUpEvent = MouseUpEvent.GetPooled(evt2))
        {
            button.SendEvent(mouseUpEvent);
        }

        Assert.IsTrue(flag);
    }
    
    public static void ForceMousePositionToCenterOfGameWindow()
    {
#if UNITY_EDITOR
        // Force the mouse to be in the middle of the game screen
        var game = UnityEditor.EditorWindow.GetWindow(typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView"));
        Vector2 warpPosition = game.rootVisualElement.contentRect.center;  // never let it move
        Mouse.current.WarpCursorPosition(warpPosition);
        InputState.Change(Mouse.current.position, warpPosition);
#endif
    }

    private void SendEvent<T>(IEventHandler target) where  T : EventBase<T>, new()
    {
        using (var evt = EventBase<T>.GetPooled())
        {
            evt.target = target;
            target.SendEvent(evt);
        }
    }

   
    [TestCase(4, 1)]
    [TestCase(1, 4)]
    [TestCase(0, 2)]
    [TestCase(5, 4)]
    [TestCase(5, 1)]
    public void InsertValue_Reorderable(int oldIndex, int newIndex)
    {
        var correctResultList = names.ToList();
        correctResultList.Move(oldIndex, newIndex);
        var iHuman = correctResultList.IndexOf("-human");
        var iDog = correctResultList.IndexOf("-Dog");
        if (oldIndex > newIndex)
        {
            correctResultList.Move(iHuman, 2);
            correctResultList.Move(iDog, 3);
        }
        else
        {
            correctResultList.Move(iDog, 3);
            correctResultList.Move(iHuman, 2);
        }


        ReorderableUtility.MoveReorderable(newIndex, root[oldIndex]);
        Debug.Log($"=== Result");
        foreach (var t in RootToListOfNames())
        {
            Debug.Log(t);
        }
        Debug.Log($"=== Should Be");
        foreach (var t in correctResultList)
        {
            Debug.Log(t);
        }
        
        CollectionAssert.AreEqual(correctResultList, RootToListOfNames());
    }
}