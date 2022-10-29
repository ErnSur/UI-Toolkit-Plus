using System;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class VisualElementExtensions
    {
        public static T ToggleManipulator<T>(this T ve, IManipulator manipulator, bool enable)
            where T : VisualElement
        {
            if (enable)
                ve.AddManipulator(manipulator);
            else
                ve.RemoveManipulator(manipulator);
            return ve;
        }

        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element,
            EventCallback<ChangeEvent<T>> onValueChange, T initialValue)
        {
            InitField(ve, element, onValueChange, () => initialValue);
        }

        // TODO: See if we can use `this INotifyValueChanged<T> ve` without `element` argument
        public static void InitField<T>(this VisualElement ve, INotifyValueChanged<T> element,
            EventCallback<ChangeEvent<T>> onValueChange, Func<T> getInitialValue)
        {
            element.RegisterValueChangedCallback(onValueChange);

            ve.RegisterCallback<AttachToPanelEvent>(InitValue);

            void InitValue(AttachToPanelEvent e)
            {
                element.value = getInitialValue();
                ve.UnregisterCallback<AttachToPanelEvent>(InitValue);
            }
        }

        public static VisualElement ToggleDisplayStyle(this VisualElement ve, bool value)
        {
            ve.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            return ve;
        }

        public static void AddAction(this Button button, Action clickEvent)
        {
#if UNITY_2019_3_OR_NEWER
            button.clicked += clickEvent;
#else
            button.clickable.clicked += clickEvent;
#endif
        }
    }
}