using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("QuickEye.UIToolkit.Editor.Tests")]
[assembly:InternalsVisibleTo("QuickEye.UIToolkit.Editor")]

#if UNITY_EDITOR
[assembly:UnityEditor.UIElements.UxmlNamespacePrefix("QuickEye.UIToolkit","quick-eye")]
#endif