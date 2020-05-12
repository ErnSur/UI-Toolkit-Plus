using System;

namespace QuickEye.UIToolkit
{
    public sealed class UQueryAttribute : Attribute
    {
        public UQueryAttribute() { }
        public UQueryAttribute(string name) => Name = name;

        public string Name { get; set; }
    }
}