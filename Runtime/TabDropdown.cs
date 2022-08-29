#if UNITY_2021_1_OR_NEWER
using System;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class TabDropdown : Tab
    {
        public event Action<GenericDropdownMenu> BeforeMenuShow;

        public const string ClassName = "tab-dropdown";
        public const string ArrowCLassName = ClassName + "--arrow";

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
        }
        
        private void ShowMenu()
        {
            if (_menu?.contentContainer.panel != null || BeforeMenuShow == null || IsDragged)
                return;
            _menu = new GenericDropdownMenu();
            BeforeMenuShow?.Invoke(_menu);
            _menu.DropDown(worldBound, this);
        }

        protected override void PointerDownHandler(PointerDownEvent evt)
        {
            var clickedOnActiveTab = value;

            base.PointerDownHandler(evt);
            
            if (clickedOnActiveTab)
                ShowMenu();
        }

        private class UxmlFactory : UxmlFactory<TabDropdown, UxmlTraits> { }

        private class UxmlTraits : Tab.UxmlTraits { }
    }
}
#endif