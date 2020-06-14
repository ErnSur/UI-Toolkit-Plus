using System;
using System.Collections.Generic;
#if UNITY_2018
using UnityEngine.Experimental.UIElements;
#else
using UnityEngine.UIElements;
#endif

namespace QuickEye.UIToolkit
{
    public static class INotifyValueChangedExtensions
    {
        private static readonly Dictionary<Delegate, Delegate> _wrapedCallbacks = new Dictionary<Delegate, Delegate>();

        public static bool RegisterThisValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
        {
            EventCallback<ChangeEvent<T>> wrappedCallback = evt =>
            {
                if (evt.target == control)
                    callback.Invoke(evt);
            };
            _wrapedCallbacks[callback] = wrappedCallback;

            return control.RegisterValueChangedCallback(wrappedCallback);
        }

        public static bool UnregisterThisValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
        {
            if (_wrapedCallbacks.TryGetValue(callback, out var wrappedCallback))
            {
                return control.UnregisterValueChangedCallback(wrappedCallback as EventCallback<ChangeEvent<T>>);
            }
            return false;
        }
        
        #if UNITY_2018
        public static bool RegisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
        {
            control.OnValueChanged(callback);
            return true;
        }
        
        public static bool UnregisterValueChangedCallback<T>(this INotifyValueChanged<T> control, EventCallback<ChangeEvent<T>> callback)
        {
            control.RemoveOnValueChanged(callback);
            return true;
        }
        #endif
    }
}