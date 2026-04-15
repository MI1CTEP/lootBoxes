using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame.Cards
{
    public sealed class BigCard : MonoBehaviour
    {
        [SerializeField] private BigCardButtons _bigCardButtons;
        [SerializeField] private BigCardSellPanel _bigCardSellPanel;
        [SerializeField] private UpgradePanel _upgradePanel;
        [SerializeField] private VideoPlayerPanel _videoPlayerPanel;
        [SerializeField] private Button _playVideoButton;
        [SerializeField] private Image _cardImage;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Text _idText;
        [SerializeField] private Text _nameGroupText;
        [SerializeField] private Text _nameText;
        [SerializeField] private Image _outline;

        private CardPanel _cardPanel;
        private Card _card;
        private Anim _anim;
        private RectTransform _nameRect;
        private RectTransform _nameGroupRect;
        private Sprite _cardSprite;
        private bool _isNewCard;
        private bool _isOpenedUpgrade;

        public void Init(CardPanel cardPanel, MiniCardsController miniCardsController)
        {
            _cardPanel = cardPanel;
            _bigCardButtons.Init(this);
            _bigCardSellPanel.Init(miniCardsController, this);
            _upgradePanel.Init(this, miniCardsController);
            _videoPlayerPanel.Init(this);
            _playVideoButton.onClick.AddListener(PlayVideo);
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(1);
            _nameRect = _nameText.GetComponent<RectTransform>();
            _nameGroupRect = _nameGroupText.GetComponent<RectTransform>();
            gameObject.SetActive(false);
        }

        public void SetCard(Card card)
        {
            _card = card;
            _bigCardButtons.SetCard(card);
            if (card.rank < 3)
            {
                _upgradePanel.SetCard(card);
                _playVideoButton.gameObject.SetActive(false);
            }
            else
            {
                _playVideoButton.gameObject.SetActive(true);
            }
            _idText.text = $"#{card.id + 1}";
            _idText.color = Settings.RankCard[card.rank].outlineColor;
            _nameGroupText.text = Settings.CardGroups[card.groupId].NameGroup;
            _nameText.text = Settings.CardGroups[card.groupId].GetNameCard(card.id);
            _outline.color = Settings.RankCard[card.rank].outlineColor;
            //Так как этот спрайт уже загружен до перехода. Так я избегаю "мигания"
            AsyncContent.LoadMiniCardSprite(card, (sprite) =>
            {
                _cardSprite = sprite;
                _cardImage.sprite = _cardSprite;
            });
            AsyncContent.LoadCardSprite(card, (sprite) => 
            {
                if (_cardSprite)
                    AsyncContent.ReleaseSprite(_cardSprite);
                _cardSprite = sprite;
                _cardImage.sprite = _cardSprite;
            });
        }

        public void ShowNew()
        {
            _isNewCard = true;
            gameObject.SetActive(true);
            ResetCard();
            transform.localScale = Vector3.one;
            _canvasGroup.alpha = 1;
            _nameRect.anchoredPosition = new Vector2(30, _nameRect.anchoredPosition.y);
            _nameGroupRect.anchoredPosition = new Vector2(30, _nameGroupRect.anchoredPosition.y);
            transform.eulerAngles = new Vector3(0, -90, 0);
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DORotate(Vector3.zero, 0.2f));
            _anim.InsertCallback(0.2f, () => _bigCardButtons.Show(true));
        }

        public void Show(Transform miniCardTransform)
        {
            _isNewCard = false;
            ResetCard();
            transform.position = miniCardTransform.position;
            transform.localScale = Vector3.one * 0.375f;
            gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DOLocalMove(new Vector3(0, 125, 0), 0.5f));
            _anim.Insert(0, transform.DOScale(Vector3.one, 0.5f));
            _anim.Insert(0.5f, _canvasGroup.DOFade(1, 0.3f));
            _anim.Insert(0.5f, _nameRect.DOAnchorPosX(30, 0.5f));
            _anim.Insert(0.5f, _nameGroupRect.DOAnchorPosX(30, 0.5f));
            _anim.InsertCallback(0.5f, () => _bigCardButtons.Show(false));
        }

        public void ShowUpgrade()
        {
            _isOpenedUpgrade = true;
            _upgradePanel.Show();
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DOLocalMove(Vector3.zero, 0.5f));
            _anim.Insert(0, transform.DOScale(Vector3.one * 0.375f, 0.5f));
            _anim.Insert(0, _canvasGroup.DOFade(0, 0.5f));
            _anim.InsertCallback(0.5f, ()=> 
            {
                gameObject.SetActive(false);
            });
        }

        public void SellForKeys()
        {
            int priceKey = Settings.RankCard[_card.rank].priceKey;
            GameData.CardGroupExperience.Add(_card.groupId, priceKey);
            GameData.Keys.Add(priceKey, true);
            if (!_isNewCard)
                CardsData.Delete(_card);
            MainPanel.Instance.UpdatePanel();
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DORotate(new Vector3(0, -90, 0), 0.5f));
            _anim.Insert(0, transform.DOLocalMoveX(-750, 0.5f));
            _anim.InsertCallback(0.5f, _cardPanel.Hide);
            _anim.InsertCallback(0.5f, Hide);
        }

        public void StopVideo()
        {
            _idText.gameObject.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0.5f, _canvasGroup.DOFade(1, 0.3f));
        }

        private void PlayVideo()
        {
            _idText.gameObject.SetActive(false);
            _canvasGroup.alpha = 0;
            _videoPlayerPanel.Show(_card);
        }

        public void SellForLabs()
        {
            int priceLab = Settings.RankCard[_card.rank].priceLab;
            GameData.Labs.Add(priceLab, true);
            if (!_isNewCard)
                CardsData.Delete(_card);
            MainPanel.Instance.UpdatePanel();
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DORotate(new Vector3(0, 90, 0), 0.5f));
            _anim.Insert(0, transform.DOLocalMoveX(750, 0.5f));
            _anim.InsertCallback(0.5f, _cardPanel.Hide);
            _anim.InsertCallback(0.5f, Hide);
        }

        public void GetCard()
        {
            CardsData.Add(_card);
            GroupSelectionPanel.Instance.AddAndRemoveNewAction(_card.groupId, true);
            GameData.CurrentCardGroupId = _card.groupId;
            Close();
        }

        public void Close()
        {
            MainPanel.Instance.UpdatePanel();
            _bigCardButtons.Hide();
            _anim.SetNewSequence();
            _anim.Insert(0, transform.DORotate(new Vector3(30, 0, 0), 0.3f));
            _anim.Insert(0.3f, transform.DOLocalMove(new Vector3(0, -1800, -1150), 0.7f));
            _anim.InsertCallback(0.6f, _cardPanel.Hide);
            _anim.InsertCallback(1, Hide);
            if (_isOpenedUpgrade)
                _upgradePanel.Hide();
        }

        public void OpenSellPanel()
        {
            _bigCardSellPanel.Show(_card.groupId);
        }

        public void SetCanGet() => _bigCardButtons.SetCanGet();

        private void Hide()
        {
            gameObject.SetActive(false);
            AsyncContent.ReleaseSprite(_cardSprite);
            _cardSprite = null;
            _videoPlayerPanel.TryHide();
        }

        private void ResetCard()
        {
            _idText.gameObject.SetActive(true);
            _isOpenedUpgrade = false;
            _canvasGroup.alpha = 0;
            _nameRect.anchoredPosition = new Vector2(800, _nameRect.anchoredPosition.y);
            _nameGroupRect.anchoredPosition = new Vector2(1200, _nameGroupRect.anchoredPosition.y);
            transform.localPosition = new Vector3(0, 125, 0);
            transform.eulerAngles = Vector3.zero;
        }
    }
}