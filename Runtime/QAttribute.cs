using System;

namespace QuickEye.UIToolkit
{
    public sealed class QAttribute : Attribute
    {
        public string Name { get; set; }
        public string[] Classes { get; set; }

        public QAttribute() { }
        public QAttribute(string name, params string[] classes)=>
            (Name, Classes) = (name, classes);
    }
}