using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_2018
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#else
using UnityEngine.UIElements;
#endif

namespace QuickEye.UIToolkit
{
    public class EnumerableVisualElement : VisualElement, IEnumerable<VisualElement>, IEquatable<VisualElement>
    {
        public bool Equals(VisualElement other)
        {
            return base.Equals(other);  
        }

        public IEnumerator<VisualElement> GetEnumerator()
        {
            foreach (var child in contentContainer.Children())
                yield return child;
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public class Column : EnumerableVisualElement
    {
        public Column() : this(false) { }
        public Column(bool reverse)
        {
            style.flexDirection = reverse
                ? FlexDirection.ColumnReverse
                : FlexDirection.Column;
        }
    }

    public class Row : EnumerableVisualElement
    {
        public Row() : this(false) { }
        public Row(bool reverse)
        {
            style.flexDirection = reverse
                ? FlexDirection.RowReverse
                : FlexDirection.Row;
        }
    }
}