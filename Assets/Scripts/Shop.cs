using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MyGame
{
    public sealed class Shop : MonoBehaviour
    {
        [SerializeField] private RectTransform _downRect;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _goldKeyButton;
        [SerializeField] private Text _goldKeyButtonText;
        [SerializeField] private Button _keyButton;
        [SerializeField] private Text _keyButtonText;
        [SerializeField] private Button _labButton;
        [SerializeField] private Text _labButtonText;

        private Anim _anim;
        private Image _background;
        private Color _backgroundColor;

        public static Shop Instance { get; set; }

        public void Init()
        {
            Instance = this;

            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(0.5f);

            _background = GetComponent<Image>();
            _backgroundColor = _background.color;
            _background.color = Color.clear;

            _downRect.anchoredPosition = new Vector2(0, -360);

            _closeButton.onClick.AddListener(Hide);
            _goldKeyButton.onClick.AddListener(AddGoldKey);
            _keyButton.onClick.AddListener(AddKey);
            _labButton.onClick.AddListener(AddLab);

            _goldKeyButtonText.text = $"视先臆 <color=#FFFFFF>+{Settings.Shop.addGoldKey}</color>";
            _keyButtonText.text = $"视先臆 <color=#FFFFFF>+{Settings.Shop.addKey}</color>";
            _labButtonText.text = $"视先臆 <color=#FFFFFF>+{Settings.Shop.addLab}</color>";

            gameObject.SetActive(false);
        }

        public void Show()
        {
            SetInteractableButtons(true);
            gameObject.SetActive(true);
            _anim.SetNewSequence();
            _anim.Insert(0, _background.DOColor(_backgroundColor, 1));
            _anim.Insert(0, _downRect.DOAnchorPosY(360, 1));
        }

        private void Hide()
        {
            SetInteractableButtons(false);
            _anim.SetNewSequence();
            _anim.Insert(0, _background.DOColor(Color.clear, 1));
            _anim.Insert(0, _downRect.DOAnchorPosY(-360, 1));
            _anim.InsertCallback(1, () =>
            {
                gameObject.SetActive(false);
            });
        }

        private void SetInteractableButtons(bool value)
        {
            _closeButton.interactable = value;
            _goldKeyButton.interactable = value;
            _keyButton.interactable = value;
            _labButton.interactable = value;
        }

        private void AddGoldKey()
        {
            GameData.GoldKeys.Add(Settings.Shop.addGoldKey, true);
        }

        private void AddKey()
        {
            GameData.Keys.Add(Settings.Shop.addKey, true);
        }

        private void AddLab()
        {
            GameData.Labs.Add(Settings.Shop.addLab, true);
        }
    }
}