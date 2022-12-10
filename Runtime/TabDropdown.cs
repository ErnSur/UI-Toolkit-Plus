using System;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDropdown : Tab
    {
        private event Action<IGenericMenu> _beforeMenuShow;
        public event Action<IGenericMenu> BeforeMenuShow
        {
            add
            {
                _beforeMenuShow += value;
                UpdateDropdownButtonVisibility();
            }
            remove
            {
                _beforeMenuShow -= value;
                UpdateDropdownButtonVisibility();
            }
        }

        public new const string ClassName = Tab.ClassName + "-dropdown";
        public const string ArrowClassName = ClassName + "__arrow";
        public const string DropdownAreaClassName = ClassName + "__dropdown-area";

        private IGenericMenu _menu;

        private readonly VisualElement _dropdownArea;
        private readonly VisualElement _dropdownIcon;

        public TabDropdown() : this(null) { }

        public TabDropdown(string text) : base(text)
        {
            this.InitResources();
            AddToClassList(ClassName);
            
            _dropdownArea = new VisualElement();
            _dropdownIcon = new VisualElement();
            _dropdownIcon.AddToClassList(ArrowClassName);
            _dropdownIcon.AddToClassList("unity-base-popup-field__arrow");
            Add(_dropdownArea);
            _dropdownArea.Add(_dropdownIcon);
            _dropdownArea.AddToClassList(DropdownAreaClassName);
            var clickable = new Clickable(ShowMenu);
            _dropdownArea.AddManipulator(clickable);
            UpdateDropdownButtonVisibility();
        }

        private void ShowMenu()
        {
            if (_beforeMenuShow == null || IsDragged)
                return;
            _menu = GenericMenuUtility.CreateMenuForContext(panel.contextType);
            _beforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }
        
        private void UpdateDropdownButtonVisibility()
        {
            _dropdownArea.ToggleDisplayStyle(_beforeMenuShow != null);
        }
        
        public new class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        public new class UxmlTraits : Tab.UxmlTraits { }
    }
}