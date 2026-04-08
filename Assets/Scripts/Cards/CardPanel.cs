using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyGame.LootBox;

namespace MyGame.Cards
{
    public sealed class CardPanel : MonoBehaviour
    {
        [SerializeField] private CardOpeningPanel _cardOpeningPanel;
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private Button _closeButton;

        private Anim _anim;
        private Image _backgroundImage;
        private RectTransform _closeRect;

        public void Init(MiniCardsController miniCardsController)
        {
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.5f);
            _backgroundImage = GetComponent<Image>();
            _cardOpeningPanel.Init();
            _cardOpeningPanel.OnComplete += _bigCard.ShowNew;
            _bigCard.Init(this, miniCardsController);

            _closeButton.onClick.AddListener(_bigCard.Close);
            _closeRect = _closeButton.GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        public void ShowFromLootBox(Card card, LootBoxSettings lootBoxSettings)
        {
            MainPanel.Instance.Hide();
            _bigCard.SetCard(card);
            gameObject.SetActive(true);
            ResetPanel();
            _anim.SetNewSequence();
            _anim.Insert(0, _backgroundImage.DOFade(1, 1));
            _cardOpeningPanel.Show(lootBoxSettings.GetRandomKeys, lootBoxSettings.GetRandomLabs, lootBoxSettings.GetRandomGoldKey);
        }

        public void Show(Card card, Transform miniCardTransform)
        {
            MainPanel.Instance.Hide();
            gameObject.SetActive(true);
            ResetPanel();
            _bigCard.SetCard(card);
            _bigCard.Show(miniCardTransform);
            _anim.SetNewSequence();
            _anim.Insert(0, _backgroundImage.DOFade(1, 1));
            _anim.Insert(0, _closeRect.DOAnchorPosY(-65, 1));
        }

        public void Hide()
        {
            _anim.SetNewSequence();
            _anim.Insert(0.1f, _backgroundImage.DOFade(0, 1));
            _anim.InsertCallback(1, () => gameObject.SetActive(false));
            _anim.InsertCallback(0.7f, MainPanel.Instance.Show);
            _anim.Insert(0, _closeRect.DOAnchorPosY(65, 1));
        }

        private void ResetPanel()
        {
            Color backgroundColor = _backgroundImage.color;
            backgroundColor.a = 0;
            _backgroundImage.color = backgroundColor;

            Vector3 closeAnchoredPosition = _closeRect.anchoredPosition;
            closeAnchoredPosition.y = 65;
            _closeRect.anchoredPosition = closeAnchoredPosition;
        }

        private void OnDestroy()
        {
            _cardOpeningPanel.OnComplete -= _bigCard.ShowNew;
        }
    }
}