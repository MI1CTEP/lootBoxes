using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame.Cards
{
    public sealed class BigCardSellPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _downPanel;
        [SerializeField] private Transform _content;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _destroyButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Text _sellText;
        [SerializeField] private Text _destroyText;
        [SerializeField] private GameObject _blockClick;

        private MiniCardsController _miniCardsController;
        private Image _background;
        private Anim _anim;
        private RectTransform _sellRect;
        private RectTransform _destroyRect;
        private RectTransform _closeRect;
        private List<MiniCard> _currentMiniCards = new();
        private List<Card> _selectablesCards = new();
        private int _sellValue;
        private int _destroyValue;

        public void Init(MiniCardsController miniCardsController, BigCard bigCard)
        {
            _miniCardsController = miniCardsController;

            _background = GetComponent<Image>();

            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(1);

            _sellButton.onClick.AddListener(SellCards);
            _sellButton.onClick.AddListener(bigCard.SetCanGet);
            _destroyButton.onClick.AddListener(DestroyCards);
            _destroyButton.onClick.AddListener(bigCard.SetCanGet);
            _closeButton.onClick.AddListener(Hide);

            _sellRect = _sellButton.GetComponent<RectTransform>();
            _destroyRect = _destroyButton.GetComponent<RectTransform>();
            _closeRect = _closeButton.GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        public void Show(int cardGroupId)
        {
            _blockClick.SetActive(false);
            SetStartPositions();
            UpdateButtons();
            LoadDownMiniCards(cardGroupId);
            gameObject.SetActive(true);
            Color blockClickColor = _background.color;
            blockClickColor.a = 0;
            _background.color = blockClickColor;

            _anim.SetNewSequence();
            _anim.Insert(0, _downPanel.DOAnchorPosY(240, 0.5f));
            _anim.Insert(0, _background.DOFade(0.9f, 0.5f));
            _anim.Insert(0.5f, _sellRect.DOAnchorPosX(-216, 0.5f));
            _anim.Insert(0.5f, _destroyRect.DOAnchorPosX(216, 0.5f));
            _anim.Insert(0.5f, _closeRect.DOAnchorPosY(550, 0.5f));
        }

        private void LoadDownMiniCards(int cardGroupId)
        {
            List<Card> cards = CardsData.GetGroup(cardGroupId).OrderBy(x => x.rank)
                .ThenBy(x => x.id)
                .ToList();

            for (int i = 0; i < cards.Count; i++)
            {
                Card card = cards[i];
                MiniCard miniCard = _miniCardsController.GetMiniCard();
                miniCard.transform.SetParent(_content);
                miniCard.transform.localPosition = Vector3.zero;
                miniCard.transform.localScale = Vector3.one;
                miniCard.SetCard(card);
                miniCard.SetClickLogic(() => ClickToMiniCard(card));
                _currentMiniCards.Add(miniCard);
            }
        }

        private void ClickToMiniCard(Card card)
        {
            for (int i = 0; i < _currentMiniCards.Count; i++)
                if (_currentMiniCards[i].Card == card)
                {
                    if (_selectablesCards.Contains(card))
                    {
                        _currentMiniCards[i].SetSelectebles(false);
                        _selectablesCards.Remove(card);
                        _sellValue -= Settings.RankCard[card.rank].priceKey;
                        _destroyValue -= Settings.RankCard[card.rank].priceLab;
                    }
                    else
                    {
                        _currentMiniCards[i].SetSelectebles(true);
                        _selectablesCards.Add(card);
                        _sellValue += Settings.RankCard[card.rank].priceKey;
                        _destroyValue += Settings.RankCard[card.rank].priceLab;
                    }
                    break;
                }
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            _sellButton.interactable = _selectablesCards.Count > 0;
            _destroyButton.interactable = _selectablesCards.Count > 0;

            _sellText.text = $"œ–Œƒ¿“‹ <color=white>+{_sellValue}</color>";
            _destroyText.text = $"”Õ»◊“Œ∆»“‹ <color=white>+{_destroyValue}</color>";
        }

        private void SetStartPositions()
        {
            _currentMiniCards.Clear();
            _selectablesCards.Clear();
            _sellValue = 0;
            _destroyValue = 0;
            _downPanel.anchoredPosition = new Vector2(_downPanel.anchoredPosition.x, -240);
            _sellRect.anchoredPosition = new Vector2(-800, _sellRect.anchoredPosition.y);
            _destroyRect.anchoredPosition = new Vector2(800, _destroyRect.anchoredPosition.y);
            _closeRect.anchoredPosition = new Vector2(_closeRect.anchoredPosition.x, -60);
        }

        private void SellCards()
        {
            GameData.Keys.Add(_sellValue, true);
            RemoveCards();
            Hide();
        }

        private void DestroyCards()
        {
            GameData.Labs.Add(_destroyValue, true);
            RemoveCards();
            Hide();
        }

        private void RemoveCards() 
        {
            CardsData.DeleteRange(_selectablesCards);
            _selectablesCards.Clear();
        }

        private void Hide()
        {
            _blockClick.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0, _sellRect.DOAnchorPosX(-800, 0.5f));
            _anim.Insert(0, _destroyRect.DOAnchorPosX(800, 0.5f));
            _anim.Insert(0, _closeRect.DOAnchorPosY(-60, 0.5f));
            _anim.Insert(0.5f, _downPanel.DOAnchorPosY(-240, 0.5f));
            _anim.Insert(0.5f, _background.DOFade(0, 0.5f));
            _anim.InsertCallback(1, () => 
            {
                gameObject.SetActive(false);
                for (int i = 0; i < _currentMiniCards.Count; i++)
                    _miniCardsController.ReturnMiniCard(_currentMiniCards[i]);
            });
        }
    }
}