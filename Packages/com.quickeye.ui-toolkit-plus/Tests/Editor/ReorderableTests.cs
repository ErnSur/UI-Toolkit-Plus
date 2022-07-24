using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

public class ReorderableTests
{
    private List<string> names;
    private VisualElement root;
    [SetUp]
    public void Setup()
    {
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

    private List<string> RootToListOfNames()
    {
        return root.Children().Select(e => e.name).ToList();
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

internal static class ListExtensions
{
    public static void Move(this IList list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }
}