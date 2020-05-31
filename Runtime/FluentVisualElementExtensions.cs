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

        public static T AttachToPanel<T>(this T ve, EventCallback<AttachToPanelEvent> callback) where T : VisualElement
        {
            ve.RegisterCallback(callback);
            return ve;
        }

        public static T Is<T>(this T ve, out T reference) where T : VisualElement
        {
            reference = ve;
            return ve;
        }

        public static T Action<T>(this T ve, Action callback) where T : Button
        {
            ve.clicked += callback;
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

        public static T FlexDir<T>(this T ve, FlexDirection direction) where T : VisualElement
        {
            ve.style.flexDirection = direction;
            return ve;
        }

        public static T Column<T>(this T ve) where T : VisualElement
            => FlexDir(ve, FlexDirection.Column);

        public static T Row<T>(this T ve) where T : VisualElement
            => FlexDir(ve, FlexDirection.Row);
    }
}