using System.Linq;
using System.Xml.Linq;

namespace QuickEye.UIToolkit.Editor
{
    internal class UxmlElement
    {
        public readonly XElement XElement;
        public readonly string Namespace;
        public readonly string TypeName;
        public readonly string NameAttribute;
        public string FullyQualifiedTypeName => $"{Namespace}.{TypeName}";
        public bool IsUnityEngineType => Namespace == "UnityEngine.UIElements";
        
        public UxmlElement(XElement xElement)
        {
            XElement = xElement;
            var localName = xElement.Name.LocalName;

            Namespace = xElement.Name.NamespaceName != XNamespace.None
                ? xElement.Name.NamespaceName
                : localName[..localName.LastIndexOf('.')];
            TypeName = localName.Contains('.')
                ? localName.Split('.').Last()
                : localName;
            NameAttribute = xElement.Attribute("name")?.Value;
            if (FullyQualifiedTypeName == "UnityEngine.UIElements.Instance")
                TypeName = "TemplateContainer";
        }
    }

    internal static class XElementExtensions
    {
        public static UxmlElement ToUxmlElement(this XElement xElement)
        {
            return new UxmlElement(xElement);
        }
    }
}