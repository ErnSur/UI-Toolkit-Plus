using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabGroup : VisualElement
    {
        public TabGroup()
        {
            this.InitResources();
        }

        private class UxmlFactory : UxmlFactory<TabGroup>
        {
        }
    }
}