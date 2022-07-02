using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabGroup : VisualElement
    {
        public const string ClassName = "tab-group";
        public TabGroup()
        {
            this.InitResources();
            AddToClassList(ClassName);
        }

        private class UxmlFactory : UxmlFactory<TabGroup>
        {
        }
    }
}