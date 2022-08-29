using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class CompatibilityExtensions
    {
        public static void RegisterToOnItemChosen(this ListView lv, Action<object> callback)
        {
#if UNITY_2020_1_OR_NEWER
            lv.onItemsChosen += objs => callback?.Invoke(objs.First());
#else
            lv.onItemChosen += callback;
#endif
        }
        
        public static void RegisterToOnSelectionChange(this ListView lv, Action<IEnumerable<object>> callback)
        {
#if UNITY_2020_1_OR_NEWER
            lv.onSelectionChange += callback;
#else
            lv.onSelectionChanged += callback;
#endif
        }
        
        public static void UnregisterToOnSelectionChange(this ListView lv, Action<IEnumerable<object>> callback)
        {
#if UNITY_2020_1_OR_NEWER
            lv.onSelectionChange -= callback;
#else
            lv.onSelectionChanged -= callback;
#endif
        }
        
        public static void Rebuild(this ListView lv)
        {
#if UNITY_2020_1_OR_NEWER
            lv.Rebuild();
#else
            lv.Refresh();
#endif
        }
    }
}