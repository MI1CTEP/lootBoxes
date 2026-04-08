using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.Cards
{
    public sealed class MiniCardsPanel : MonoBehaviour
    {
        private MiniCardsController _miniCardsController;
        private CardPanel _cardPanel;
        private readonly List<MiniCard> _miniCards = new();
        private GridLayoutGroup _gLG;
        private RectTransform _rectTransform;

        public void Init(MiniCardsController miniCardsController, CardPanel cardPanel)
        {
            _miniCardsController = miniCardsController;
            _cardPanel = cardPanel;
            _gLG = GetComponent<GridLayoutGroup>();
            _rectTransform = GetComponent<RectTransform>();
            UpdateContent();
        }

        public void UpdateContent()
        {
            ClearContent();
            SetMiniCards();
            UpdateSize();
        }

        public void ClearContent()
        {
            for (int i = 0; i < _miniCards.Count; i++)
                _miniCardsController.ReturnMiniCard(_miniCards[i]);
            _miniCards.Clear();
        }

        private void SetMiniCards()
        {
            List<Card> cards = CardsData.GetGroup(GameData.CurrentCardGroupId).OrderByDescending(x => x.rank)
                .ThenBy(x => x.id)
                .ToList();

            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                MiniCard miniCard = _miniCardsController.GetMiniCard();
                miniCard.SetClickLogic(()=> _cardPanel.Show(card, miniCard.transform));
                miniCard.transform.SetParent(transform);
                miniCard.transform.localScale = Vector3.one;
                miniCard.SetCard(card);
                _miniCards.Add(miniCard);
            }
            UpdateSize();
        }

        private void UpdateSize()
        {
            Vector2 sizeDelta = _rectTransform.sizeDelta;
            int rows = (_miniCards.Count + _gLG.constraintCount - 1) / _gLG.constraintCount;
            sizeDelta.y = (_gLG.cellSize.y + _gLG.spacing.y) * rows;
            _rectTransform.sizeDelta = sizeDelta;
        }
    }
}