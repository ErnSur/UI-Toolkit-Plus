using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.FieldExtensions
{
    public interface IBaseField : ILabelable, IMixedValueSupport
    {
        object value        { get; }
        Label  labelElement { get; }
    }
}