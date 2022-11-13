using UnityEngine;

namespace QuickEye.UIToolkit.Editor
{
    internal enum CaseStyle
    {
        [InspectorName("")]
        NotSet = 0,
        [InspectorName("lowerCamelCase")]
        LowerCamelCase = 1,

        [InspectorName("UpperCamelCase")]
        UpperCamelCase = 2,
    }
}