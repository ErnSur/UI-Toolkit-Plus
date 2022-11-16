using UnityEngine;

namespace QuickEye.UxmlBridgeGen
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