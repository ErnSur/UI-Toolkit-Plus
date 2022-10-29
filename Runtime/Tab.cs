using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class Tab : BaseBindable<bool>
    {
        public const string ClassName = "tab";
        public const string TextClassName = "tab__text";

        protected Label Label;

        private VisualElement _tabContent;
        public readonly Reorderable Reorderable = new Reorderable(ClassName){LockDragToAxis = true};

        public VisualElement TabContent
        {
            get => _tabContent;
            set
            {
                _tabContent = value;
                _tabContent?.ToggleDisplayStyle(this.value);
            }
        }

        public bool IsReorderable
        {
            get => Reorderable.target == this;
            set => this.ToggleManipulator(Reorderable, value);
        }

        public string Text
        {
            get => Label.text;
            set => Label.text = value;
        }

        public bool IsDragged => Reorderable.IsDragged(this);

        public Tab() : this(null) { }

        public Tab(string text)
        {
            this.InitResources();
            EnableInClassList(ClassName, true);
            Label = new Label(text);
            Label.EnableInClassList(TextClassName, true);
            Add(Label);
            AddToClassList("tab");
            
            RegisterCallback<PointerDownEvent>(PointerDownHandler);
            IsReorderable = false;
            SetActive(value);
        }

        protected virtual void PointerDownHandler(PointerDownEvent evt)
        {
            value = true;
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
            if(parent == null)
                return;
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
                tab.IsReorderable = _reorderable.GetValueFromBag(bag, cc);
            }
        }
    }
}