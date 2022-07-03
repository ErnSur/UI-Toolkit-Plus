using System.Collections.Generic;
using VE = UnityEngine.UIElements.VisualElement;

namespace QuickEye.UIToolkit
{
    public static class ReorderableUtility
    {
        public static void MoveReorderable(int newIndex, VE element)
        {
            var container = element.parent;
            var nonReorderables = new List<(VE value, int index)>();
            var oldIndex = -1;
            for (int i = 0; i <= newIndex + 1 || oldIndex == -1; i++)
            {
                var v = container[i];
                if (v == element)
                {
                    oldIndex = i;
                    continue;
                }

                if (!Reorderable.IsReorderable(container[i]))
                    nonReorderables.Add((container[i], i));
            }

            container.Insert(newIndex, element);
            if (oldIndex > newIndex)
            {
                for (int i = 0; i < nonReorderables.Count; i++)
                {
                    var (v, index) = nonReorderables[i];
                    container.Insert(index, v);
                }
            }
            else
            {
                for (int i = nonReorderables.Count - 1; i >= 0; i--)
                {
                    var (v, index) = nonReorderables[i];
                    container.Insert(index, v);
                }
            }
        }
    }
}