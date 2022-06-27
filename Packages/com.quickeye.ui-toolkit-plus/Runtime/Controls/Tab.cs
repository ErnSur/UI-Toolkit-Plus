using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    //Add class on down
    //On move: remove and add if is outside
    // on up: remove class
    public class Tab : BaseBindable<bool>
    {
        [Q]
        protected Label Label;

        private VisualElement _tabContent;
        public bool enabled = true;

        public VisualElement TabContent
        {
            get => _tabContent;
            set
            {
                _tabContent = value;
                _tabContent?.ToggleDisplayStyle(this.value);
            }
        }

        public string text
        {
            get => Label.text;
            set => Label.text = value;
        }

        public bool IsDragged => ClassListContains(TabDragAndDropManipulator.TabDraggedClassName);

        public Tab() : this(null) { }

        public Tab(string text)
        {
            this.InitResources();
            AddToClassList("tab");
            RegisterCallback<PointerDownEvent>(PointerDownHandler);
            this.AddManipulator(new ActiveClassManipulator("tab"));
           // this.AddManipulator(new Clickable((Action)null));
            this.AddManipulator(new TabDragAndDropManipulator());
            this.text = text;
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
            enabled = isActive;
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
            private readonly UxmlStringAttributeDescription text = new UxmlStringAttributeDescription()
                { name = "text", defaultValue = "Tab" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((Tab)ve).Label.text = text.GetValueFromBag(bag, cc);
            }
        }
    }
}