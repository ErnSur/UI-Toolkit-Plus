using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Editor
{
    public class ToolbarDropdownButton : ToolbarButton, IToolbarMenuElement
    {
        public new const string ussClassName = "qe-toolbar-dropdown-button";
        public const string iconUssClassName = ussClassName + "__icon";
        public const string labelUssClassName = ussClassName + "__label";
        public const string spacerUssClassName = ussClassName + "__spacer";
        public const string dropdownAreaUssClassName = ussClassName + "__dropdown-area";

        private readonly TextElement _label;
        private readonly VisualElement _dropdownArea;
        
        public DropdownMenu menu { get; }

        public new string text
        {
            get => _label.text;
            set => _label.text = value;
        }

        public ToolbarDropdownButton()
        {
            this.InitResources();
            AddToClassList(ussClassName);
            style.flexDirection = FlexDirection.Row;
            
            menu = new DropdownMenu();

            _label = new TextElement();
            _label.AddToClassList(labelUssClassName);
            Add(_label);

            var spacer = new VisualElement();
            spacer.AddToClassList(spacerUssClassName);
            Add(spacer);

            var dropdownIcon = new VisualElement();
            dropdownIcon.AddToClassList(iconUssClassName);

            _dropdownArea = new VisualElement();
            _dropdownArea.RegisterCallback<MouseDownEvent>(evt =>
            {
                evt.PreventDefault();
                evt.StopImmediatePropagation();

                this.ShowMenu();
            });
            _dropdownArea.AddToClassList(dropdownAreaUssClassName);
            _dropdownArea.Add(dropdownIcon);
            Add(_dropdownArea);
        }

        private new class UxmlFactory : UxmlFactory<ToolbarDropdownButton, UxmlTraits> { }

        private new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription _text = new UxmlStringAttributeDescription { name = "text" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ((ToolbarDropdownButton)ve).text = _text.GetValueFromBag(bag, cc);
            }
        }

    }
}