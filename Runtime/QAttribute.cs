using System;

namespace QuickEye.UIToolkit
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public sealed class QAttribute : Attribute
    {
        public string Name { get; set; }
        public string[] Classes { get; set; }

        public QAttribute() { }
        public QAttribute(string name, params string[] classes) =>
            (Name, Classes) = (name, classes);
    }
}