using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    // TODO: Add: Selected Tab Property
    public class TabGroup : VisualElement
    {
        public const string ClassName = "tab-group";

        public ScrollView ScrollView { get; private set; }
        public override VisualElement contentContainer => ScrollView.contentContainer;

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
            CreateScrollView();

            ScrollView.style.flexGrow = 1;
            Mode = TabGroupMode.Horizontal;
            this.InitResources();
            AddToClassList(ClassName);
        }

        private void CreateScrollView()
        {
            ScrollView = new ScrollView();
            ScrollView.RegisterCallback<GeometryChangedEvent>(OnScrollViewGeometryChange);
            ScrollView.horizontalScroller.valueChanged += OnHorizontalScrollerValueChanged;
            ScrollView.horizontalPageSize = 2;
            ScrollView.horizontalScroller.lowButton.SetAction(ScrollView.horizontalScroller.ScrollPageUp, 0, 1);
            ScrollView.horizontalScroller.highButton.SetAction(ScrollView.horizontalScroller.ScrollPageDown, 0, 1);
            ScrollView.horizontalScroller.pickingMode = PickingMode.Ignore;

            hierarchy.Add(ScrollView);
        }

        private void OnScrollViewGeometryChange(GeometryChangedEvent evt)
        {
            UpdateScrollButtonVisibility();
        }

        private void OnHorizontalScrollerValueChanged(float value)
        {
            UpdateScrollButtonVisibility();
        }

        private void UpdateScrollButtonVisibility()
        {
            var horizontalValue = ScrollView.horizontalScroller.value;
            var leftEnabled = horizontalValue != 0;
            var rightEnabled = horizontalValue < ScrollView.horizontalScroller.highValue;
            ScrollView.horizontalScroller.lowButton.ToggleDisplayStyle(leftEnabled);
            ScrollView.horizontalScroller.highButton.ToggleDisplayStyle(rightEnabled);
        }

        private void SetLayoutMode(TabGroupMode mode)
        {
#if UNITY_2021_2_OR_NEWER
            ScrollView.mode = mode == TabGroupMode.Horizontal ? ScrollViewMode.Horizontal : ScrollViewMode.Vertical;
            ScrollView.verticalScrollerVisibility =
                mode == TabGroupMode.Horizontal ? ScrollerVisibility.Hidden : ScrollerVisibility.Auto; 
#endif
            ScrollView.style.alignItems = mode == TabGroupMode.Vertical ? Align.FlexStart : Align.FlexEnd;
        }

        private class UxmlFactory : UxmlFactory<TabGroup> { }
    }

    public enum TabGroupMode
    {
        Horizontal,
        Vertical
    }
}