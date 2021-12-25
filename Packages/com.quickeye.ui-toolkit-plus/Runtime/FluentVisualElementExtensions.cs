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

        public static T Clicked<T>(this T ve, Action callback) where T : VisualElement
        {
            switch (ve)
            {
                case Button button:
                    button.AddAction(callback);
                    break;
                default:
                    ve.AddManipulator(new Clickable(callback));
                    break;
            }

            return ve;
        }

        public static T Text<T>(this T ve, string text) where T : TextElement
        {
            ve.text = text;
            return ve;
        }

        public static T Column<T>(this T ve, bool reverse = false) where T : VisualElement
            => FlexDir(ve, reverse ? FlexDirection.ColumnReverse : FlexDirection.Column);

        public static T Row<T>(this T ve, bool reverse = false) where T : VisualElement
            => FlexDir(ve, reverse ? FlexDirection.RowReverse : FlexDirection.Row);

        private static T FlexDir<T>(this T ve, FlexDirection direction) where T : VisualElement
        {
            ve.style.flexDirection = direction;
            return ve;
        }

        public static T BindingPath<T>(this T ve, string bindingPath)
            where T : VisualElement, IBindable
        {
            ve.bindingPath = bindingPath;
            return ve;
        }
    }
}