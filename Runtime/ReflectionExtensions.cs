using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    internal static class ReflectionExtensions
    {
        public static MethodInfo FindVisualElementOfType { get; } = GetFindVisualElementOfTypeMethod();

        private const BindingFlags QAttributeTargetFlags = BindingFlags.FlattenHierarchy
                                                           | BindingFlags.Instance
                                                           | BindingFlags.Static
                                                           | BindingFlags.Public
                                                           | BindingFlags.NonPublic;

        public static (MemberInfo, QAttribute)[] GetQAttributeMembers(this Type type)
        {
            return (from member in GetFieldsAndProperties()
                let att = member.GetCustomAttribute<QAttribute>()
                where att != null
                select (member, att)).ToArray();

            IEnumerable<MemberInfo> GetFieldsAndProperties()
            {
                foreach (var field in type.GetFields(QAttributeTargetFlags))
                    yield return field;
                foreach (var prop in type.GetProperties(QAttributeTargetFlags))
                    yield return prop;
            }
        }

        //Returns `UnityEngine.UIElements.UQueryExtensions.Q<T>(root)` method
        private static MethodInfo GetFindVisualElementOfTypeMethod()
        {
            var parameters = new[] {typeof(VisualElement), typeof(string), typeof(string[])};

            var uQueryExtensionMethods = typeof(UnityEngine.UIElements.UQueryExtensions)
                .GetMethods(BindingFlags.Static | BindingFlags.Public);

            var methods = from method in uQueryExtensionMethods
                where method.Name == "Q"
                where method.IsGenericMethod
                where method.HasParameters(parameters)
                select method;
            return methods.FirstOrDefault();
        }

        private static bool HasParameters(this MethodInfo methodInfo, IList<Type> paramTypes)
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