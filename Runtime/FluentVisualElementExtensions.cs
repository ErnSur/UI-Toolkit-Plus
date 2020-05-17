using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class FluentVisualElementExtensions
    {
        public static T WithClass<T>(this T ve, params string[] classNames) where T : VisualElement
        {
            foreach (var name in classNames)
                ve.AddToClassList(name);
            return ve;
        }
    }
}