using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace QuickEye.UxmlBridgeGen
{
    using UnityEngine;

    internal static class UxmlParser
    {
        /// <summary>
        /// Names with underscore prefix are ignored. This is so that we can name elements for the sake of hierarchy readability.
        /// </summary>
        public static bool TryGetElementsWithValidName(string uxml, out UxmlElement[] elements)
        {
            try
            {
                elements = (from ele in XDocument.Parse(uxml).Descendants()
                    let name = ele.Attribute("name")?.Value
                    where name != null && !name.StartsWith("_")
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