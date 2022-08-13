using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class GenericMenuUtility
    {
        private static readonly Type _GenericOsMenuType
#if UNITY_EDITOR
            = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.UIElements.GenericOSMenu")
#endif
;
        public static IGenericMenu CreateMenuForContext(ContextType context)
        {
            return context == ContextType.Player || _GenericOsMenuType == null
                ? new GenericDropdownMenuWrapper()
                : CreateEditorMenu();
        }

        private static GenericMenuReflectionWrapper CreateEditorMenu()
        {
            return new GenericMenuReflectionWrapper(Activator.CreateInstance(_GenericOsMenuType));
        }
    }
}