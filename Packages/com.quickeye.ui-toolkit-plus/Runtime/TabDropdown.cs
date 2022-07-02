#if UNITY_2021_1_OR_NEWER
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDropdown : Tab
    {
        public event Action<GenericDropdownMenu> BeforeMenuShow;

        public const string ClassName = "tab-dropdown";
        public const string ArrowCLassName = ClassName + "--arrow";

        private readonly Clickable _clickable;
        private GenericDropdownMenu _menu;

        public TabDropdown() :this(null)
        {
        }

        public TabDropdown(string text) :base(text)
        {
            this.InitResources();
            AddToClassList(ClassName);
            var arrow = new VisualElement();
            arrow.AddToClassList(ArrowCLassName);
            Add(arrow);
            _clickable = new Clickable(ShowMenu);
        }
        
        private void ShowMenu()
        {
            if (_menu?.contentContainer.panel != null || BeforeMenuShow == null || IsDragged)
                return;
            _menu = new GenericDropdownMenu();
            BeforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }

        public override void SetValueWithoutNotify(bool newValue)
        {
            var clickedOnActiveTab = value && newValue;
            base.SetValueWithoutNotify(newValue);
            if (clickedOnActiveTab)
                this.AddManipulator(_clickable);
            else
                this.RemoveManipulator(_clickable);
        }

        private class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        private class UxmlTraits : Tab.UxmlTraits { }
    }
}
#endif