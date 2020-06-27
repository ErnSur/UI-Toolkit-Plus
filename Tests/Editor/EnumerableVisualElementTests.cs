using NUnit.Framework;

namespace QuickEye.UIToolkit.Tests
{
    public class EnumerableVisualElementTests
    {
        [Test]
        public void EnumerableIsNotNull()
        {
            var ve = new EnumerableVisualElement();
            var ve2 = new EnumerableVisualElement();
            
            Assert.AreNotEqual(ve,ve2);
        }
    }
}