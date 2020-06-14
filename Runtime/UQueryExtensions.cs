using System;
using System.Reflection;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit
{
    public static class UQueryExtensions
    {
        public static void AssignQueryResults(this VisualElement root, object target)
        {
            foreach (var (member, att) in target.GetType().GetQAttributeMembers())
            {
                Type returnType;
                Action<object, object> setMemberValue;
                
                if (member is FieldInfo field)
                    (returnType, setMemberValue) = (field.FieldType, field.SetValue);
                else if (member is PropertyInfo property)
                    (returnType, setMemberValue) = (property.PropertyType, property.SetValue);
                else continue;
                
                var queryResult = string.IsNullOrEmpty(att.Name)
                    ? root.Q(returnType)
                    : root.Q(att.Name, att.Classes);
                
                setMemberValue(target, queryResult);
            }
        }

        private static VisualElement Q(this VisualElement e, Type type)
        {
            var methodInfo = ReflectionExtensions.FindVisualElementOfType.MakeGenericMethod(type);

            return methodInfo.Invoke(null, new object[] {e, null, null}) as VisualElement;
        }
    }
}