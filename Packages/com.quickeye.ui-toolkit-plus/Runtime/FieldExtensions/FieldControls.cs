using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.FieldExtensions
{
    // @formatter:off
    public class TogglePlus           : Toggle,           IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<TogglePlus,           UxmlTraits> { } }
    public class RadioButtonPlus      : RadioButton,      IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<RadioButtonPlus,      UxmlTraits> { } }
    public class TextFieldPlus        : TextField,        IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<TextFieldPlus,        UxmlTraits> { } }
    public class IntegerFieldPlus     : IntegerField,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<IntegerFieldPlus,     UxmlTraits> { } }
    public class FloatFieldPlus       : FloatField,       IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<FloatFieldPlus,       UxmlTraits> { } }
    public class LongFieldPlus        : LongField,        IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<LongFieldPlus,        UxmlTraits> { } }
    public class Vector2FieldPlus     : Vector2Field,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<Vector2FieldPlus,     UxmlTraits> { } }
    public class Vector3FieldPlus     : Vector3Field,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<Vector3FieldPlus,     UxmlTraits> { } }
    public class Vector4FieldPlus     : Vector4Field,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<Vector4FieldPlus,     UxmlTraits> { } }
    public class RectFieldPlus        : RectField,        IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<RectFieldPlus,        UxmlTraits> { } }
    public class BoundsFieldPlus      : BoundsField,      IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<BoundsFieldPlus,      UxmlTraits> { } }
    public class Vector2IntFieldPlus  : Vector2IntField,  IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<Vector2IntFieldPlus,  UxmlTraits> { } }
    public class Vector3IntFieldPlus  : Vector3IntField,  IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<Vector3IntFieldPlus,  UxmlTraits> { } }
    public class RectIntFieldPlus     : RectIntField,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<RectIntFieldPlus,     UxmlTraits> { } }
    public class BoundsIntFieldPlus   : BoundsIntField,   IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<BoundsIntFieldPlus,   UxmlTraits> { } }
    public class RadioButtonGroupPlus : RadioButtonGroup, IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<RadioButtonGroupPlus, UxmlTraits> { } }
    public class SliderPlus           : Slider,           IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<SliderPlus,           UxmlTraits> { } }
    public class SliderIntPlus        : SliderInt,        IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<SliderIntPlus,        UxmlTraits> { } }
    public class MinMaxSliderPlus     : MinMaxSlider,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<MinMaxSliderPlus,     UxmlTraits> { } }
    public class DropdownFieldPlus    : DropdownField,    IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<DropdownFieldPlus,    UxmlTraits> { } }
    public class EnumFieldPlus        : EnumField,        IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<EnumFieldPlus,        UxmlTraits> { } }
    public class PopupFieldPlus<T>    : PopupField<T>,    IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<PopupFieldPlus<T>,    UxmlTraits> { } }
    // @formatter:on
}