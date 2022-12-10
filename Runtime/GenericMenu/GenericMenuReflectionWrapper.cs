using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    internal class GenericMenuReflectionWrapper : IGenericMenu
    {
        private readonly object _obj;
        private readonly MethodBase _addItem;
        private readonly MethodBase _addItem2;
        private readonly MethodBase _addDisabledItem;
        private readonly MethodBase _addSeparator;
        private readonly MethodBase _dropDown;

        public GenericMenuReflectionWrapper(object obj)
        {
            _obj = obj;
            var type = obj.GetType();
            _addItem = type.GetMethod(nameof(AddItem), new Type[] { typeof(string), typeof(bool), typeof(Action) });
            _addItem2 = type.GetMethod(nameof(AddItem),
                new Type[] { typeof(string), typeof(bool), typeof(Action<object>), typeof(object) });
            _addDisabledItem = type.GetMethod(nameof(AddDisabledItem), new Type[] { typeof(string), typeof(bool) });
            _addSeparator = type.GetMethod(nameof(AddSeparator), new Type[] { typeof(string) });
            _dropDown = type.GetMethod(nameof(DropDown),
                new Type[] { typeof(Rect), typeof(VisualElement), typeof(bool) });
        }

        public void AddItem(string itemName, bool isChecked, Action action)
        {
            _addItem.Invoke(_obj, new object[] { itemName, isChecked, action });
        }

        public void AddItem(string itemName, bool isChecked, Action<object> action, object data)
        {
            _addItem2.Invoke(_obj, new object[] { itemName, isChecked, action, data });
        }

        public void AddDisabledItem(string itemName, bool isChecked)
        {
            _addDisabledItem.Invoke(_obj, new object[] { itemName, isChecked });
        }

        public void AddSeparator(string path)
        {
            _addSeparator.Invoke(_obj, new object[] { path });
        }

        public void DropDown(Rect position, VisualElement targetElement = null, bool anchored = false)
        {
            _dropDown.Invoke(_obj, new object[] { position, targetElement, anchored });
        }
    }
}