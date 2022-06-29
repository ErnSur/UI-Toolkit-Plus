using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class Tab : BaseBindable<bool>
    {
        [Q]
        protected Label Label;

        private VisualElement _tabContent;
        private readonly ReorderableManipulator _reorderableManipulator = new ReorderableManipulator();

        public VisualElement TabContent
        {
            get => _tabContent;
            set
            {
                _tabContent = value;
                _tabContent?.ToggleDisplayStyle(this.value);
            }
        }

        public bool Reorderable
        {
            get => _reorderableManipulator.target == this;
            set => this.ToggleManipulator(_reorderableManipulator, value);
        }

        public string Text
        {
            get => Label.text;
            set => Label.text = value;
        }

        public bool IsDragged => ClassListContains(ReorderableManipulator.DraggedClassName);

        public Tab() : this(null) { }

        public Tab(string text)
        {
            this.InitResources();
            AddToClassList("tab");
            RegisterCallback<PointerDownEvent>(PointerDownHandler);
            this.AddManipulator(new ActiveClassManipulator("tab"));
            Reorderable = false;
            Text = text;
        }

        private void PointerDownHandler(PointerDownEvent evt)
        {
            SetValueWithoutNotify(true);
        }

        public override void SetValueWithoutNotify(bool newValue)
        {
            base.SetValueWithoutNotify(newValue);
            SetActive(newValue);
        }

        private void SetActive(bool isActive)
        {
            EnableInClassList("tab--checked", isActive);
            EnableInClassList("tab--unchecked", !isActive);
            TabContent?.ToggleDisplayStyle(isActive);
            if (isActive)
                DeactivateSiblings();
        }

        private void DeactivateSiblings()
        {
            foreach (var tab in parent.Children().OfType<Tab>())
                if (tab != this)
                    tab.SetValueWithoutNotify(false);
        }

        public class UxmlFactory : UxmlFactory<Tab, UxmlTraits> { }

        public class UxmlTraits : BaseBindableTraits<bool, UxmlBoolAttributeDescription>
        {
            private readonly UxmlStringAttributeDescription _text = new UxmlStringAttributeDescription()
                { name = "text" };

            private readonly UxmlBoolAttributeDescription _reorderable = new UxmlBoolAttributeDescription()
            {
                name = "Reorderable",
                defaultValue = false
            };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var tab = (Tab)ve;
                tab.Text = _text.GetValueFromBag(bag, cc);
                tab.Reorderable = _reorderable.GetValueFromBag(bag, cc);
            }
        }
    }
}