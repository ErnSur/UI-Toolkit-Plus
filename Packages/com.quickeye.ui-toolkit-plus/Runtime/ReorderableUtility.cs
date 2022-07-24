using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using VE = UnityEngine.UIElements.VisualElement;

namespace QuickEye.UIToolkit
{
    internal static class ReorderableUtility
    {
        public static bool NeedsHorizontalScroller(this ScrollView sv)
        {
            var rect = sv.contentContainer.contentRect;
            var width1 = (double)rect.width;
            rect = sv.contentViewport.layout;
            var width2 = (double)rect.width;
            var scrollableWidth = (float)(width1 - width2);
            return scrollableWidth > 1.0 / 1000.0;
        }

        public static VE FindClosestElement(VisualElement target, VisualElement[] elements)
        {
            var bestDistanceSq = float.MaxValue;
            VisualElement closest = null;
            foreach (var element in elements)
            {
                var displacement =
                    RootSpaceOfElement(element) - target.transform.position;
                var distanceSq = displacement.sqrMagnitude;
                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closest = element;
                }
            }

            return closest;
        }

        private static Vector3 RootSpaceOfElement(VisualElement element)
        {
            var tabWorldSpace = element.parent.LocalToWorld(element.layout.position);
            return element.parent.WorldToLocal(tabWorldSpace);
        }

        public static void MoveReorderable(int newIndex, VE element)
        {
            var container = element.parent;
            var nonReorderables = new List<(VE value, int index)>();
            var oldIndex = -1;
            for (var i = 0; i <= newIndex + 1 || oldIndex == -1; i++)
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
                for (var i = 0; i < nonReorderables.Count; i++)
                {
                    var (v, index) = nonReorderables[i];
                    container.Insert(index, v);
                }
            }
            else
            {
                for (var i = nonReorderables.Count - 1; i >= 0; i--)
                {
                    var (v, index) = nonReorderables[i];
                    container.Insert(index, v);
                }
            }
        }
    }
}