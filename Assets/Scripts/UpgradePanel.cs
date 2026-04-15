using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;
using DG.Tweening;

namespace MyGame
{
    public sealed class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _addMiniCards;
        [SerializeField] private RectTransform _downPanel;
        [SerializeField] private RectTransform _upPanel;
        [SerializeField] private RectTransform _downPanelContent;
        [SerializeField] private RectTransform _upPanelContent;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Text _upgradePriceText;

        private readonly MiniCard[] _slotMiniCards = new MiniCard[3];
        private MiniCard _upMinicard;
        private List<MiniCard> _downMiniCards = new();
        private BigCard _bigCard;
        private MiniCardsController _miniCardsController;
        private Anim _anim;
        private Card _card;
        private int _upgradePrice;

        public void Init(BigCard bigCard, MiniCardsController miniCardsController)
        {
            _bigCard = bigCard;
            _miniCardsController = miniCardsController;

            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(1);

            _backButton.onClick.AddListener(Back);
            _upgradeButton.onClick.AddListener(Upgrade);

            gameObject.SetActive(false);
        }

        public void SetCard(Card card)
        {
            _card = card;
            _upgradePrice = Settings.RankCard[_card.rank + 1].priceLab;
            _upgradePriceText.text = _upgradePrice.ToString();
        }

        public void Show()
        {
            ResetPanel();
            LoadUpMiniCard();
            LoadDownMiniCards();
            SetSlotMiniCard(_card, 1);

            gameObject.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0.5f, _addMiniCards[0].DOAnchorPosX(190, 0.5f));
            _anim.Insert(0.5f, _addMiniCards[2].DOAnchorPosX(-190, 0.5f));
            _anim.Insert(0.5f, _downPanel.DOAnchorPosY(240, 0.5f));
            _anim.Insert(0.5f, _upPanel.DOAnchorPosY(-265, 0.5f));
        }

        public void Hide()
        {
            _anim.SetNewSequence();
            _anim.Insert(0, _addMiniCards[0].DOAnchorPosX(-190, 0.5f));
            _anim.Insert(0, _addMiniCards[1].DOScale(Vector3.zero, 0.5f));
            _anim.Insert(0, _addMiniCards[2].DOAnchorPosX(190, 0.5f));
            _anim.Insert(0, _downPanel.DOAnchorPosY(-240, 0.5f));
            _anim.Insert(0, _upPanel.DOAnchorPosY(265, 0.5f));
            _anim.InsertCallback(0.5f, () => 
            {
                gameObject.SetActive(false);

                _miniCardsController.ReturnMiniCard(_upMinicard);
                _upMinicard = null;

                for (int i = 0; i < _slotMiniCards.Length; i++)
                    if (_slotMiniCards[i] != null)
                        ResetSlotMiniCard(i);

                for (int i = 0; i < _downMiniCards.Count; i++)
                    _miniCardsController.ReturnMiniCard(_downMiniCards[i]);
                _downMiniCards.Clear();

            });
        }

        private void Back()
        {
            _bigCard.SetCard(_card);
            Hide();
            _bigCard.Show(_addMiniCards[1]);
        }

        private void Upgrade()
        {
            if (_upgradePrice <= GameData.Labs.GetValue())
            {
                for (int i = 0; i < _slotMiniCards.Length; i++)
                    CardsData.Delete(_slotMiniCards[i].Card);
                CardsData.Add(_upMinicard.Card);
                GroupSelectionPanel.Instance.AddAndRemoveNewAction(_upMinicard.Card.groupId, true);
                GameData.Labs.Add(-_upgradePrice, true);
                _bigCard.SetCard(_upMinicard.Card);
                Hide();
                _bigCard.Show(_upPanelContent);
            }
            else Shop.Instance.Show();
        }

        private void LoadUpMiniCard()
        {
            Card card = Card.GetNew(_card.groupId, _card.id, _card.rank + 1);

            _upMinicard = _miniCardsController.GetMiniCard();
            _upMinicard.transform.SetParent(_upPanelContent);
            _upMinicard.transform.localPosition = Vector3.zero;
            _upMinicard.transform.localScale = Vector3.one;
            _upMinicard.SetCard(card);
        }

        private void LoadDownMiniCards()
        {
            List<Card> cards = CardsData.GetGroup(_card.groupId);
            for (int i = 0; i < cards.Count; i++)
            {
                if(cards[i].id == _card.id && cards[i].rank == _card.rank)
                {
                    Card card = cards[i];
                    MiniCard miniCard = _miniCardsController.GetMiniCard();
                    miniCard.transform.SetParent(_downPanelContent);
                    miniCard.transform.localPosition = Vector3.zero;
                    miniCard.transform.localScale = Vector3.one;
                    miniCard.SetCard(card);
                    miniCard.SetClickLogic(() => ClickToMiniCard(card));
                    _downMiniCards.Add(miniCard);
                }
            }
        }

        private void ClickToMiniCard(Card card)
        {
            for (int i = 0; i < _slotMiniCards.Length; i++)
            {
                if (_slotMiniCards[i] == null) continue;
                if(_slotMiniCards[i].Card == card)
                {
                    ResetSlotMiniCard(i);
                    return;
                }
            }
            for (int i = 0; i < _slotMiniCards.Length; i++)
            {
                if(_slotMiniCards[i] == null)
                {
                    SetSlotMiniCard(card, i);
                    break;
                }
            }
        }

        private void SetSlotMiniCard(Card card, int slotId)
        {
            _slotMiniCards[slotId] = _miniCardsController.GetMiniCard();
            _slotMiniCards[slotId].transform.SetParent(_addMiniCards[slotId]);
            _slotMiniCards[slotId].transform.localPosition = Vector3.zero;
            _slotMiniCards[slotId].transform.localScale = Vector3.one;
            _slotMiniCards[slotId].SetCard(card);

            for (int i = 0; i < _downMiniCards.Count; i++)
                if (_downMiniCards[i].Card == card)
                {
                    _downMiniCards[i].SetSelectebles(true);
                    break;
                }
            TrySetCanUpgrade();
        }

        private void TrySetCanUpgrade()
        {
            for (int i = 0; i < _slotMiniCards.Length; i++)
                if (_slotMiniCards[i] == null)
                    return;

            _upgradeButton.interactable = true;
        }

        private void ResetSlotMiniCard(int slotId)
        {
            for (int i = 0; i < _downMiniCards.Count; i++)
            {
                if(_downMiniCards[i].Card == _slotMiniCards[slotId].Card)
                {
                    _downMiniCards[i].SetSelectebles(false);
                    break;
                }
            }
            _miniCardsController.ReturnMiniCard(_slotMiniCards[slotId]);
            _slotMiniCards[slotId] = null;
            _upgradeButton.interactable = false;
        }

        private void ResetPanel()
        {
            _upgradeButton.interactable = false;
            _addMiniCards[0].anchoredPosition = new Vector2(-190, _addMiniCards[0].anchoredPosition.y);
            _addMiniCards[1].localScale = Vector3.one;
            _addMiniCards[2].anchoredPosition = new Vector2(190, _addMiniCards[2].anchoredPosition.y);
            _downPanel.anchoredPosition = new Vector2(_downPanel.anchoredPosition.x, -240);
            _upPanel.anchoredPosition = new Vector2(_upPanel.anchoredPosition.x, 265);
        }
    }
}