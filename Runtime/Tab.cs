using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class Tab : BaseBindable<bool>
    {
        public const string ClassName = "qe-tab";
        public const string TextClassName = ClassName + "__text";
        public const string CheckedClassName = ClassName + "--checked";
        public const string UncheckedClassName = ClassName + "--unchecked";

        public readonly Reorderable Reorderable = new(ClassName) { LockDragToAxis = true };

        private readonly Label _textElement;

        private VisualElement _tabContent;

        public Tab() : this(null) { }

        public Tab(string text)
        {
            this.InitResources();
            AddToClassList(ClassName);

            _textElement = new Label(text);
            _textElement.AddToClassList(TextClassName);
            Add(_textElement);

            RegisterCallback<PointerDownEvent>(PointerDownHandler);
            IsReorderable = false;
            SetActive(value);
        }

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
            get => _textElement.text;
            set => _textElement.text = value;
        }

        public bool IsDragged => Reorderable.IsDragged(this);

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
            EnableInClassList(CheckedClassName, isActive);
            EnableInClassList(UncheckedClassName, !isActive);
            TabContent?.ToggleDisplayStyle(isActive);
            if (isActive)
                DeactivateSiblings();
        }

        private void DeactivateSiblings()
        {
            if (parent == null)
                return;
            foreach (var tab in parent.Children().OfType<Tab>())
                if (tab != this)
                    tab.SetValueWithoutNotify(false);
        }

        public new class UxmlFactory : UxmlFactory<Tab, UxmlTraits> { }

        public new class UxmlTraits : BaseBindableTraits<bool, UxmlBoolAttributeDescription>
        {
            private readonly UxmlBoolAttributeDescription _isReorderable = new()
            {
                name = "is-reorderable"
            };

            private readonly UxmlStringAttributeDescription _text = new() { name = "text" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var tab = (Tab)ve;
                tab.Text = _text.GetValueFromBag(bag, cc);
                tab.IsReorderable = _isReorderable.GetValueFromBag(bag, cc);
            }
        }
    }
}