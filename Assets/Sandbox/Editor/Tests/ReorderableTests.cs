using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

// TODO:
// create methods that inserts element into list with respect of non reorderable elements
// and returns a list of all move operations
// then use this function to send events on all moved elements
public class ReorderableTests
{
    private List<string> list;

    [SetUp]
    public void Setup()
    {
        list = new List<string>
        {
            "Plane", //0
            "Car", //1
            "-human", //2
            
            "-Dog", //3
            "cat", //4
            "fly", //5
        };
    }

    List<(int oldIndex, int newIndex)> GetChangedIndexes(int oldIndex, int newIndex, List<string> list)
    {
        // Get range of elements that will be affected by the operation
        // elements are represented by their oldIndex and newIndex
        var affectedElements = new List<(int oldIndex, int newIndex)>();


        var offset = 1;
        affectedElements.Add((oldIndex, newIndex));
        if (oldIndex > newIndex)
        {
            for (int i = newIndex; i < oldIndex; i++)
            {
                if (IsReorderable(list[i]))
                {
                    offset++;
                    continue;
                }

                affectedElements.Add((i, GetNewIndex()));

                int GetNewIndex()
                {
                    if (i + offset >= list.Count)
                        return i;
                    if (IsReorderable(list[i + offset]))
                    {
                        offset++;
                        return GetNewIndex();
                    }

                    return i + offset;
                }
            }
        }
        else
        {
            for (int i = oldIndex + 1; i < newIndex + 1; i++)
            {
                if (IsReorderable(list[i]))
                {
                    offset++;
                    continue;
                }
                affectedElements.Add((i, GetNewIndex()));
                
                int GetNewIndex()
                {
                    if (i - offset < 0)
                        return i;
                    if (IsReorderable(list[i - offset]))
                    {
                        offset++;
                        return GetNewIndex();
                    }

                    return i - offset;
                }
            }
        }

        Debug.Log($"=== Affected elements :");
        foreach ((int oldIndex, int newIndex) tuple in affectedElements)
        {
            Debug.Log($"{tuple}");
        }

        Debug.Log($"============");

        Debug.Log($"=== Elements that will have to move:");
        // Now I have range of affected elements
        // If all of them would be reorderable then we would just need to sent event s to all of them
        var cache = new Dictionary<int, string>();
        foreach (var (oIndex, nIndex) in affectedElements)
        {
            var oldValue = list[nIndex];

            var valueToMove = cache.ContainsKey(oIndex) ? cache[oIndex] : list[oIndex];
            Debug.Log($"Move: {valueToMove} in place of {oldValue}");

            cache[nIndex] = list[nIndex];
            list[nIndex] = valueToMove;
        }

        Debug.Log($"============");
        Debug.Log($"=== Final result of moving the [{oldIndex}] to [{newIndex}]");

        foreach (var i in list)
        {
            Debug.Log($"{i}");
        }

        bool IsReorderable(string e) => e.StartsWith("-");
        return default;
    }

    // A Test behaves as an ordinary method
    [TestCase(4, 1)]
    [TestCase(1, 4)]
    public void ReorderableTestsSimplePasses(int oldIndex, int newIndex)
    {
        var correctResultList = list.ToList();
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


        GetChangedIndexes(oldIndex, newIndex, list);
        Debug.Log($"=== Should Be");
        foreach (var t in correctResultList)
        {
            Debug.Log(t);
        }

        CollectionAssert.AreEqual(correctResultList, list);
    }
}

public static class ListExtensions
{
    public static void Move(this IList list, int oldIndex, int newIndex)
    {
        var item = list[oldIndex];

        list.RemoveAt(oldIndex);
        list.Insert(newIndex, item);
    }
}