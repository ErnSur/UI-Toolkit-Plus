using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.UIToolkit.Samples.Cards
{
    public class UIController : MonoBehaviour
    {
        private VisualElement[] cards;
        private VisualElement root;

        void Start()
        {
            var uidoc = GetComponent<UIDocument>();
            root = uidoc.rootVisualElement;
            GetCards();
            AddReorderables();
        }

        private void AddReorderables()
        {
            foreach (var card in cards)
            {
                card.AddManipulator(new Reorderable());
            }
        }

        private void GetCards()
        {
            cards = root.Query(null, "card").Build().ToArray();
        }
    }
}