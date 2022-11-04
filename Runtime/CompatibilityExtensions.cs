using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class CompatibilityExtensions
    {
        public static void RegisterToItemsChosen(this BaseVerticalCollectionView view, Action<IEnumerable<object>> callback)
        {
#if UNITY_2022_2_OR_NEWER
            view.itemsChosen += callback;
#else
            view.onItemsChosen += callback;
#endif
        }
        
        public static void UnregisterFromItemsChosen(this BaseVerticalCollectionView view, Action<IEnumerable<object>> callback)
        {
#if UNITY_2022_2_OR_NEWER
            view.itemsChosen -= callback;
#else
            view.onItemsChosen -= callback;
#endif
        }
        
        public static void RegisterToSelectionChanged(this BaseVerticalCollectionView view, Action<IEnumerable<object>> callback)
        {
#if UNITY_2022_2_OR_NEWER
            view.selectionChanged += callback;
#else
            view.onSelectionChange += callback;
#endif
        }
        
        public static void UnregisterFromSelectionChanged(this BaseVerticalCollectionView view, Action<IEnumerable<object>> callback)
        {
#if UNITY_2022_2_OR_NEWER
            view.selectionChanged -= callback;
#else
            view.onSelectionChange -= callback;
#endif
        }
    }
}