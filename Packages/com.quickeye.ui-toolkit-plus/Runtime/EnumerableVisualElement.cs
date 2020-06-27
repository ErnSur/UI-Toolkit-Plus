using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class EnumerableVisualElement : VisualElement, IEnumerable<VisualElement>, IEquatable<VisualElement>
    {
        public bool Equals(VisualElement other) => base.Equals(other);

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