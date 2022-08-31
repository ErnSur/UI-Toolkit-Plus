using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace QuickEye.UIToolkit.Editor
{
    internal static class UxmlParser
    {
        public static bool TryGetElementsWithName(string uxml, out UxmlElement[] elements)
        {
            try
            {
                elements = (from ele in XDocument.Parse(uxml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null
                    select new UxmlElement(ele)).ToArray();

                return true;
            }
            catch (XmlException)
            {
                elements = null;
                return false;
            }
        }
    }
}