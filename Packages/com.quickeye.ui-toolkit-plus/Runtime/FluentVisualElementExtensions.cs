using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class FluentVisualElementExtensions
    {
        public static T Class<T>(this T ve, params string[] classNames) where T : VisualElement
        {
            foreach (var name in classNames)
                ve.AddToClassList(name);
            return ve;
        }

        public static TElement Callback<TElement, TEventType>(this TElement ve, EventCallback<TEventType> callback,
            TrickleDown useTrickleDown = TrickleDown.NoTrickleDown)
            where TElement : VisualElement
            where TEventType : EventBase<TEventType>, new()
        {
            ve.RegisterCallback(callback, useTrickleDown);
            return ve;
        }

        public static T Is<T>(this T ve, out T reference) where T : VisualElement
        {
            reference = ve;
            return ve;
        }

        public static T Clicked<T>(this T ve, Action callback) where T : VisualElement
        {
            switch (ve)
            {
                case Button button:
#if UNITY_2019_3_OR_NEWER
                    button.clicked += callback;
#else
                    button.clickable.clicked += callback;
#endif
                    break;
                default:
                    ve.AddManipulator(new Clickable(callback));
                    break;
            }

            return ve;
        }

        // TODO: Try to make Clicked work on any VisualElement
        // TODO: Try to make Clicked with optional "delay,interval" parameters
        // problem here start when user invokes `Clicked` multiple times
        // right now only the first invocation will actually work
        // this is due to how manipulators work... probably.
        // public static T Clicked<T>(this T ve, Action callback, long delay = 0, long interval = 0) where T : VisualElement
        // {
        //     var clickable = new Clickable(callback, delay + 100, interval + 100);
        //     ve.AddManipulator(clickable);
        //     return ve;
        // }

        public static T Focusable<T>(this T ve, bool value) where T : VisualElement
        {
            ve.focusable = value;
            return ve;
        }

        public static T Text<T>(this T ve, string text) where T : TextElement
        {
            ve.text = text;
            return ve;
        }

        public static T BackgroundColor<T>(this T ve, Color color) where T : VisualElement
        {
            ve.style.backgroundColor = color;
            return ve;
        }

        public static T Size<T>(this T ve, float width, float height) where T : VisualElement
        {
            ve.style.width = width;
            ve.style.height = height;
            return ve;
        }

        public static T Width<T>(this T ve, float width) where T : VisualElement
        {
            ve.style.width = width;
            return ve;
        }

        public static T Height<T>(this T ve, float height) where T : VisualElement
        {
            ve.style.height = height;
            return ve;
        }

        public static T FlexDir<T>(this T ve, FlexDirection direction) where T : VisualElement
        {
            ve.style.flexDirection = direction;
            return ve;
        }

        public static T Column<T>(this T ve) where T : VisualElement
            => FlexDir(ve, FlexDirection.Column);

        public static T Row<T>(this T ve) where T : VisualElement
            => FlexDir(ve, FlexDirection.Row);

        public static T BindingPath<T>(this T ve, string bindingPath)
            where T : VisualElement, IBindable
        {
            ve.bindingPath = bindingPath;
            return ve;
        }
    }
}