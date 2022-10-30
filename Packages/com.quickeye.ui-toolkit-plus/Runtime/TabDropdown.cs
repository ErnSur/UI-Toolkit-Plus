#if UNITY_2021_1_OR_NEWER
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

        public const string ClassName = "tab-dropdown";
        public const string ArrowCLassName = ClassName + "--arrow";

        private IGenericMenu _menu;
        private Clickable _clickable;

        private VisualElement _dropdownButton;

        public TabDropdown() : this(null) { }

        public TabDropdown(string text) : base(text)
        {
            this.InitResources();
            AddToClassList(ClassName);
            _dropdownButton = new VisualElement();
            _dropdownButton.AddToClassList(ArrowCLassName);
            Add(_dropdownButton);
            _clickable = new Clickable(ShowMenu);
            _dropdownButton.AddManipulator(_clickable);
            UpdateDropdownButtonVisibility();
        }

        private void ShowMenu()
        {
            if ((_menu is GenericDropdownMenuWrapper gdm && gdm.Menu.contentContainer.panel != null) ||
                _beforeMenuShow == null || IsDragged)
                return;
            _menu = GenericMenuUtility.CreateMenuForContext(panel.contextType);
            _beforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }

        private void UpdateDropdownButtonVisibility()
        {
            _dropdownButton.ToggleDisplayStyle(_beforeMenuShow != null);
        }
        public new class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        public new class UxmlTraits : Tab.UxmlTraits { }
    }
}
#endif