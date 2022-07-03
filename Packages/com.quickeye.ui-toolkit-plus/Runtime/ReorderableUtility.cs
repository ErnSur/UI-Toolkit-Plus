using System.Collections.Generic;
using UnityEngine;
using VE = UnityEngine.UIElements.VisualElement;

namespace QuickEye.UIToolkit
{
    public static class ReorderableUtility
    {
        public static void MoveReorderable(int oldIndex, int newIndex, List<VE> list,
            List<(int oldIndex, int newIndex)> changes)
        {
            // Get range of elements that will be affected by the operation
            // elements are represented by their oldIndex and newIndex
        
            var offset = 1;
            changes.Add((oldIndex, newIndex));
            if (oldIndex > newIndex)
            {
                for (int i = newIndex; i < oldIndex; i++)
                {
                    if (IsReorderable(list[i]))
                    {
                        offset++;
                        continue;
                    }

                    changes.Add((i, GetNewIndex()));

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

                    changes.Add((i, GetNewIndex()));

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
            foreach ((int oldIndex, int newIndex) tuple in changes)
            {
                Debug.Log($"{tuple}");
            }

            Debug.Log($"============");

            Debug.Log($"=== Elements that will have to move:");
            // Now I have range of affected elements
            // If all of them would be reorderable then we would just need to sent event s to all of them
            var cache = new Dictionary<int, VE>();
            foreach (var (oIndex, nIndex) in changes)
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

            bool IsReorderable(VE e) => Reorderable.IsReorderable(e);
        }
    }
}