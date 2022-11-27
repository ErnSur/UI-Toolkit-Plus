using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Cards
{
    public class UIController : MonoBehaviour
    {
        private VisualElement[] _cards;
        private VisualElement _root;

        void Start()
        {
            var uiDocument = GetComponent<UIDocument>();
            _root = uiDocument.rootVisualElement;
            GetCards();
            AddReorderables();
        }

        private void AddReorderables()
        {
            foreach (var card in _cards)
            {
                card.AddManipulator(new Reorderable());
            }
        }

        private void GetCards()
        {
            _cards = _root.Query(null, "card").Build().ToArray();
        }
    }
}