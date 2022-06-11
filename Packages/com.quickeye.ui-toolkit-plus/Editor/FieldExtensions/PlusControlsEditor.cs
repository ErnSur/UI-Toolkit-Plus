using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.FieldExtensions.Editor
{
    // @formatter:off
    public class ObjectFieldPlus    : ObjectField,    IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<ObjectFieldPlus,    UxmlTraits> { } }
    public class ColorFieldPlus     : ColorField,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<ColorFieldPlus,     UxmlTraits> { } }
    public class GradientFieldPlus  : GradientField,  IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<GradientFieldPlus,  UxmlTraits> { } }
    public class EnumFlagsFieldPlus : EnumFlagsField, IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<EnumFlagsFieldPlus, UxmlTraits> { } }
    public class TagFieldPlus       : TagField,       IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<TagFieldPlus,       UxmlTraits> { } }
    public class MaskFieldPlus      : MaskField,      IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<MaskFieldPlus,      UxmlTraits> { } }
    public class LayerFieldPlus     : LayerField,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<LayerFieldPlus,     UxmlTraits> { } }
    public class LayerMaskFieldPlus : LayerMaskField, IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<LayerMaskFieldPlus, UxmlTraits> { } }
    public class CurveFieldPlus     : CurveField,     IBaseField { object IBaseField.value => value; public new class UxmlFactory : UxmlFactory<CurveFieldPlus,     UxmlTraits> { } }
    public class PropertyFieldPlus  : PropertyField,  ILabelable { public new class UxmlFactory : UxmlFactory<PropertyFieldPlus, UxmlTraits> { } }                                                 
    // @formatter:on
}