using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class INotifyValueChangedExtensions
    {
        private static readonly Dictionary<Delegate, Delegate> WrappedCallbacks = new Dictionary<Delegate, Delegate>();

        public static bool RegisterThisValueChangedCallback<T>(this INotifyValueChanged<T> control,
            EventCallback<ChangeEvent<T>> callback)
        {
            WrappedCallbacks[callback] = (EventCallback<ChangeEvent<T>>) WrappedCallback;
            return control.RegisterValueChangedCallback(WrappedCallback);

            void WrappedCallback(ChangeEvent<T> evt)
            {
                if (evt.target == control) callback.Invoke(evt);
            }
        }

        public static bool UnregisterThisValueChangedCallback<T>(this INotifyValueChanged<T> control,
            EventCallback<ChangeEvent<T>> callback)
        {
            return WrappedCallbacks.TryGetValue(callback, out var wrappedCallback) &&
                   control.UnregisterValueChangedCallback(wrappedCallback as EventCallback<ChangeEvent<T>>);
        }
    }
}