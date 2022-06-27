using System;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDropdown : Tab
    {
        public event Action<GenericDropdownMenu> BeforeMenuShow;

        [Q(null, "unity-enum-field__arrow")]
        private VisualElement _arrow;

        private readonly Clickable _clickable;
        private GenericDropdownMenu _menu;

        public TabDropdown()
        {
            hierarchy.Clear();
            this.InitResources();
            _clickable = new Clickable(ShowMenu);
            ToggleArrow(value);
        }

        private void ShowMenu()
        {
            if (_menu?.contentContainer.panel != null || IsDragged)
                return;
            _menu = new GenericDropdownMenu();
            BeforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }

        public override void SetValueWithoutNotify(bool newValue)
        {
            var clickedOnActiveTab = value && newValue;
            base.SetValueWithoutNotify(newValue);
            ToggleArrow(newValue);
            if (clickedOnActiveTab)
                this.AddManipulator(_clickable);
            else
            {
                this.RemoveManipulator(_clickable);
            }
        }

        private void ToggleArrow(bool value)
        {
            _arrow.ToggleDisplayStyle(value);
        }

        private class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        private class UxmlTraits : Tab.UxmlTraits { }
    }
}