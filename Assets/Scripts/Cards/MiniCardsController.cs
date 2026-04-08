using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame.Cards
{
    public sealed class MiniCardsController : MonoBehaviour
    {
        [SerializeField] private MiniCard _miniCardPrefab;

        private readonly Stack<MiniCard> _miniCards = new();

        public MiniCard GetMiniCard()
        {
            MiniCard miniCard;
            if (_miniCards.Count > 0)
            {
                miniCard = _miniCards.Pop();
            }
            else
            {
                miniCard = Instantiate(_miniCardPrefab, transform);
                miniCard.Init();
            }
            miniCard.gameObject.SetActive(true);
            return miniCard;
        }

        public void ReturnMiniCard(MiniCard miniCard)
        {
            miniCard.gameObject.SetActive(false);
            miniCard.transform.SetParent(transform);
            miniCard.ResetCard();
            _miniCards.Push(miniCard);
        }
    }
}