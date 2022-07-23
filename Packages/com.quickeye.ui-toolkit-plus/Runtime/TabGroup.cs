using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabGroup : VisualElement
    {
        public const string ClassName = "tab-group";

        private ScrollView scrollView;
        public override VisualElement contentContainer => scrollView.contentContainer;

        public TabGroupMode Mode
        {
            get => _mode;
            set
            {
                _mode = value;
                SetLayoutMode(_mode);
            }
        }

        private TabGroupMode _mode;
        public TabGroup()
        {
            scrollView = new ScrollView();
            scrollView.horizontalScrollerVisibility = scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;
            hierarchy.Add(scrollView);
            scrollView.style.flexGrow = 1;
            Mode = TabGroupMode.Horizontal;
            this.InitResources();
            AddToClassList(ClassName);
        }

        private void SetLayoutMode(TabGroupMode mode)
        {
            scrollView.mode= mode == TabGroupMode.Horizontal ? ScrollViewMode.Horizontal : ScrollViewMode.Vertical;
            scrollView.style.alignItems = mode == TabGroupMode.Vertical ? Align.FlexStart : Align.FlexEnd;

        }

        private class UxmlFactory : UxmlFactory<TabGroup>
        {
        }
    }

    public enum TabGroupMode
    {
        Horizontal,
        Vertical
    }
}