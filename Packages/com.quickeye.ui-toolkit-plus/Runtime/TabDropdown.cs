#if UNITY_2021_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDropdown : Tab
    {
        public event Action<IGenericMenu> BeforeMenuShow;

        public const string ClassName = "tab-dropdown";
        public const string ArrowCLassName = ClassName + "--arrow";

        private IGenericMenu _menu;
        private Clickable _clickable;

        private VisualElement _dropdownButton;

        public TabDropdown() :this(null)
        {
        }

        public TabDropdown(string text) :base(text)
        {
            this.InitResources();
            AddToClassList(ClassName);
            _dropdownButton = new VisualElement();
            _dropdownButton.AddToClassList(ArrowCLassName);
            Add(_dropdownButton);
            _clickable = new Clickable(ShowMenu);
            _dropdownButton.AddManipulator(_clickable);
        }
        
        private void ShowMenu()
        {
            if ((_menu is GenericDropdownMenuWrapper gdm && gdm.Menu.contentContainer.panel != null) || BeforeMenuShow == null || IsDragged)
                return;
            _menu = GenericMenuUtility.CreateMenuForContext(panel.contextType);
            BeforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }

        private class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        private class UxmlTraits : Tab.UxmlTraits { }
    }
}
#endif