using System;
using UnityEngine;
using UnityEngine.UIElements;

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

        public static void SetSystemStyle(this VisualElement ve)
        {
            ve.transform.position = -Vector2.one;
            ve.transform.scale = Vector3.zero;
            ve.style.position = Position.Absolute;
        }

        public static void ToggleDisplayStyle(this VisualElement ve, bool value)
        {
            ve.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        //If Unity fixes the Default style of ScrollView this can be removed
        public static void ForceScrollViewMode(this ScrollView sv, ScrollViewMode mode)
        {
            sv.hierarchy[0].style.flexDirection =
            sv.hierarchy[0].hierarchy[0].style.flexDirection = mode == ScrollViewMode.Vertical ? FlexDirection.Column : FlexDirection.Row;
        }
    }
}