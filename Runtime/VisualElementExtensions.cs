using System;
using UnityEngine;
#if UNITY_2018
using UnityEngine.Experimental.UIElements;
#else
using UnityEngine.UIElements;
#endif

namespace QuickEye.UIToolkit
{
    public static class VisualElementExtensions
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
#if !UNITY_2018
        public static void ToggleDisplayStyle(this VisualElement ve, bool value)
        {
            ve.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }
#endif
    }
}