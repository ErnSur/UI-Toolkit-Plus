using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class UQueryExtensions
    {
        private static BindingFlags _uQueryAttributeFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy;

        private static MethodInfo _findElementOfType;

        static UQueryExtensions()
        {
            var parameters = new Type[] { typeof(VisualElement), typeof(string), typeof(string) };
            var methods = from method in typeof(UnityEngine.UIElements.UQueryExtensions).GetMethods(BindingFlags.Static | BindingFlags.Public)
                          where method.Name == "Q"
                          where method.IsGenericMethod
                          where method.HasParamaters(parameters)
                          select method;

            _findElementOfType = methods.FirstOrDefault();
        }

        public static void AssignQueryableMembers(this VisualElement root, object obj)
        {
            var members =
                from member in obj.GetType().GetMembers(_uQueryAttributeFlags)
                let att = member.GetCustomAttribute<UQueryAttribute>()
                where att != null
                select (member, att);

            foreach (var (member, att) in members)
            {
                Type returnType;
                Action<object, object> setMemberValue;

                if (member is FieldInfo field)
                    (returnType, setMemberValue) = (field.FieldType, field.SetValue);
                else if (member is PropertyInfo property)
                    (returnType, setMemberValue) = (property.PropertyType, property.SetValue);
                else continue;

                var queryResult = string.IsNullOrEmpty(att.Name)
                    ? Q(root, returnType)
                    : root.Q(att.Name);

                setMemberValue(obj, queryResult);
            }
        }

        private static object Q(VisualElement e, Type type)
        {
            var methodInfo = _findElementOfType.MakeGenericMethod(type);

            return methodInfo.Invoke(null, new object[] { e, null, null });
        }

        public static bool HasParamaters(this MethodInfo methodInfo,IList<Type> paramTypes)
        {
            var parameters = methodInfo.GetParameters();
            if (parameters.Length != paramTypes.Count)
                return false;
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != paramTypes[i])
                    return false;
            }
            return true;
        }
    }
}