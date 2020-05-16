using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static partial class VisualElementExtensions
    {
        public static void Q<T>(this VisualElement ve, out T element, string name, string className = null) where T : VisualElement
        {
            element = ve.Q<T>(name, className);
            if (element == null)
            {
                Debug.LogWarning($"Could not find: {name}, {className}");
            }
        }

        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element, EventCallback<ChangeEvent<T>> onValueChange, T initialValue)
        {
            InitField(ve, element, onValueChange, () => initialValue);
        }

        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element, EventCallback<ChangeEvent<T>> onValueChange, Func<T> getInitialValue)
        {
            element.RegisterValueChangedCallback(onValueChange);

            ve.RegisterCallback<AttachToPanelEvent>(InitValue);

            void InitValue(AttachToPanelEvent e)
            {
                element.value = getInitialValue();
                ve.UnregisterCallback<AttachToPanelEvent>(InitValue);
            }
        }

        public static void ToggleDisplayStyle(this VisualElement ve, bool value)
        {
            ve.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }

    public static partial class VisualElementExtensions
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
    }
}