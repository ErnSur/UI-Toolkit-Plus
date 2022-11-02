using System.Collections.Generic;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabGroup : VisualElement
    {
        public const string ClassName = "qe-tab-group";
        public const string VerticalModeClassName = ClassName + "--vertical";
        public const string HorizontalModeClassName = ClassName + "--horizontal";
        public const string HorizontalScrollerClassName = ClassName + "__horizontal-scroller";
        private Scroller _horizontalScroller;
        private TabGroupMode _mode;

        public TabGroup()
        {
            CreateScrollView();
            CreateHorizontalScroller();

            Mode = TabGroupMode.Horizontal;
            this.InitResources();
            AddToClassList(ClassName);
        }

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

        private float ScrollableWidth =>
            ScrollView.contentContainer.worldBound.width - ScrollView.contentViewport.layout.width;

        private void CreateScrollView()
        {
            ScrollView = new ScrollView();
            ScrollView.RegisterCallback<GeometryChangedEvent>(OnScrollViewGeometryChange);
            ScrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            ScrollView.horizontalScroller.valueChanged += OnHorizontalScrollerValueChanged;
            ScrollView.horizontalPageSize = 2;
            ScrollView.style.flexGrow = 1;

            hierarchy.Add(ScrollView);
        }

        private void CreateHorizontalScroller()
        {
            _horizontalScroller = new Scroller();
            _horizontalScroller.pickingMode = PickingMode.Ignore;
            _horizontalScroller.direction = SliderDirection.Horizontal;
            _horizontalScroller.lowButton.SetAction(ScrollView.horizontalScroller.ScrollPageUp, 0, 1);
            _horizontalScroller.highButton.SetAction(ScrollView.horizontalScroller.ScrollPageDown, 0, 1);
            _horizontalScroller.AddToClassList(HorizontalScrollerClassName);

            UpdateHorizontalScrollerVisibility();
        }

        private void OnScrollViewGeometryChange(GeometryChangedEvent evt)
        {
            UpdateHorizontalScrollerVisibility();
        }

        private void OnHorizontalScrollerValueChanged(float value)
        {
            UpdateHorizontalScrollerVisibility();
        }

        private void UpdateHorizontalScrollerVisibility()
        {
            var horizontalValue = ScrollView.horizontalScroller.value;
            var leftEnabled = horizontalValue != 0;
            var rightEnabled = horizontalValue < ScrollableWidth;
            _horizontalScroller.lowButton.ToggleDisplayStyle(leftEnabled);
            _horizontalScroller.highButton.ToggleDisplayStyle(rightEnabled);
        }

        private void SetLayoutMode(TabGroupMode mode)
        {
            EnableInClassList(VerticalModeClassName, mode == TabGroupMode.Vertical);
            EnableInClassList(HorizontalModeClassName, mode == TabGroupMode.Horizontal);
            ToggleHorizontalScroller(mode == TabGroupMode.Horizontal);
            
            _horizontalScroller.SetEnabled(mode == TabGroupMode.Horizontal);
            ScrollView.mode = mode == TabGroupMode.Horizontal ? ScrollViewMode.Horizontal : ScrollViewMode.Vertical;
            ScrollView.verticalScrollerVisibility =
                mode == TabGroupMode.Horizontal ? ScrollerVisibility.Hidden : ScrollerVisibility.Auto;
        }

        private void ToggleHorizontalScroller(bool enabled)
        {
            if (enabled)
                hierarchy.Add(_horizontalScroller);
            else
                _horizontalScroller.RemoveFromHierarchy();
        }

        private new class UxmlFactory : UxmlFactory<TabGroup, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlEnumAttributeDescription<TabGroupMode> _mode = new() { name = "mode" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield return new UxmlChildElementDescription(typeof(VisualElement)); }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var group = (TabGroup)ve;
                group.Mode = _mode.GetValueFromBag(bag, cc);
            }
        }
    }

    public enum TabGroupMode
    {
        Horizontal,
        Vertical
    }
}