namespace QuickEye.UIBuilderExtensions
{
    using UnityEngine.UIElements;

    internal static class VisualElementExtensions
    {
        public static void ToggleDisplayStyle(this VisualElement element, bool show)
        {
            element.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}