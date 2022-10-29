using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public class GenericDropdownMenuWrapper : IGenericMenu
    {
        public readonly GenericDropdownMenu Menu = new GenericDropdownMenu();

        public void AddItem(string itemName, bool isChecked, Action action)
        {
            Menu.AddItem(itemName, isChecked, action);
        }

        public void AddItem(string itemName, bool isChecked, Action<object> action, object data)
        {
            Menu.AddItem(itemName, isChecked, action, data);
        }

        public void AddDisabledItem(string itemName, bool isChecked)
        {
            Menu.AddDisabledItem(itemName, isChecked);
        }

        public void AddSeparator(string path)
        {
            Menu.AddSeparator(path);
        }

        public void DropDown(Rect position, VisualElement targetElement = null, bool anchored = false)
        {
            Menu.DropDown(position, targetElement, anchored);
        }
    }
}