using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame.Cards
{
    public sealed class MiniCard : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _cardImage;
        [SerializeField] private Text _idText;
        [SerializeField] private Image _outline;
        [SerializeField] private GameObject[] _selectebles;

        private Sprite _cardSprite;
        private Card _card;
        private UnityAction _onClick;

        public Card Card => _card;

        public void Init()
        {
            ResetCard();
        }

        public void SetCard(Card card)
        {
            _card = card;
            _idText.text = $"#{card.id + 1}";
            _idText.color = Settings.RankCard[card.rank].outlineColor;
            _outline.color = Settings.RankCard[card.rank].outlineColor;
            AsyncContent.LoadMiniCardSprite(card, (sprite) => 
            {
                if (_cardSprite != null)
                    AsyncContent.ReleaseSprite(_cardSprite);
                _cardSprite = sprite;
                _cardImage.sprite = _cardSprite;
            });
        }

        public void SetClickLogic(UnityAction onClick)
        {
            _onClick = onClick;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(_onClick != null)
            {
                _onClick.Invoke();
            }
        }

        public void SetSelectebles(bool value)
        {
            for (int i = 0; i < _selectebles.Length; i++)
                _selectebles[i].SetActive(value);
        }

        public void ResetCard()
        {
            SetSelectebles(false);
            if (_cardSprite != null)
            {
                AsyncContent.ReleaseSprite(_cardSprite);
                _cardSprite = null;
            }
            _card = null;
            _onClick = null;
        }
    }
}