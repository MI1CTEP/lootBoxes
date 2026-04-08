using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace MyGame.Cards
{
    public sealed class BigCardButtons : MonoBehaviour
    {
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _destroyButton;
        [SerializeField] private Button _getButton;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Text _sellText;
        [SerializeField] private Text _destroyText;
        [SerializeField] private GameObject _capacityText;
        [SerializeField] private GameObject _blockCliks;

        private Card _card;
        private Anim _anim;
        private RectTransform _sellRect;
        private RectTransform _destroyRect;
        private RectTransform _getRect;
        private RectTransform _upgradeRect;

        public void Init(BigCard bigCard)
        {
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.5f);

            _sellButton.onClick.AddListener(Hide);
            _sellButton.onClick.AddListener(bigCard.SellForKeys);

            _destroyButton.onClick.AddListener(Hide);
            _destroyButton.onClick.AddListener(bigCard.SellForLabs);

            _getButton.onClick.AddListener(Hide);
            _getButton.onClick.AddListener(bigCard.GetCard);

            _upgradeButton.onClick.AddListener(bigCard.ShowUpgrade);
            _upgradeButton.onClick.AddListener(Hide);

            _sellRect = _sellButton.GetComponent<RectTransform>();
            _destroyRect = _destroyButton.GetComponent<RectTransform>();
            _getRect = _getButton.GetComponent<RectTransform>();
            _upgradeRect = _upgradeButton.GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        public void SetCard(Card card)
        {
            _card = card;
            _sellText.text = $"œ–Œƒ¿“‹ <color=white>+{Settings.RankCard[card.rank].priceKey}</color>";
            _destroyText.text = $"”Õ»◊“Œ∆»“‹ <color=white>+{Settings.RankCard[card.rank].priceLab}</color>";
            _upgradeButton.interactable = _card.rank < 3;
        }

        public void Show(bool isNewCard)
        {
            _blockCliks.SetActive(false);
            _capacityText.SetActive(false);
            SetStartPositions();
            gameObject.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0, _sellRect.DOAnchorPosX(-216, 1));
            _anim.Insert(0, _destroyRect.DOAnchorPosX(216, 1));
            if (isNewCard)
            {
                int currentCapacity = CardsData.GetGroup(_card.groupId).Count;
                int maxCapacity = GameData.CardGroupCapacity.LoadLevel(_card.groupId) * Settings.Upgrades.capacity.stepValue + Settings.Upgrades.capacity.startValue;
                _anim.Insert(0, _getRect.DOAnchorPosY(50, 1));
                if(currentCapacity >= maxCapacity)
                {
                    _getButton.interactable = false;
                    _anim.InsertCallback(1, () => _capacityText.SetActive(true));
                }
                else
                {
                    _getButton.interactable = true;
                }
            }
            else
            {
                _anim.Insert(0, _upgradeRect.DOAnchorPosY(50, 1));
            }
        }

        public void Hide()
        {
            _blockCliks.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0, _sellRect.DOAnchorPosX(-800, 1));
            _anim.Insert(0, _destroyRect.DOAnchorPosX(800, 1));
            _anim.Insert(0, _getRect.DOAnchorPosY(-600, 1));
            _anim.Insert(0, _upgradeRect.DOAnchorPosY(-600, 1));
            _anim.InsertCallback(1, () => gameObject.SetActive(false));
        }

        private void SetStartPositions()
        {
            _sellRect.anchoredPosition = new Vector2(-800, _sellRect.anchoredPosition.y);
            _destroyRect.anchoredPosition = new Vector2(800, _destroyRect.anchoredPosition.y);
            _getRect.anchoredPosition = new Vector2(_getRect.anchoredPosition.x, -600);
            _upgradeRect.anchoredPosition = new Vector2(_upgradeRect.anchoredPosition.x, -600);
        }
    }
}