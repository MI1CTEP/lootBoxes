using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;
using DG.Tweening;

namespace MyGame.GroupUpgrade
{
    public sealed class UpgradeLevelPanel : MonoBehaviour
    {
        [SerializeField] private Text _groupNameText;
        [SerializeField] private Image _cardGroupLogo;
        [SerializeField] private Text _info1Text;
        [SerializeField] private Text _info2Text;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        [SerializeField] private UpgradeCapacityPanel _upgradeCapacityPanel;
        [SerializeField] private UpgradeTimePanel _upgradeTimePanel;
        [SerializeField] private UpgradeInDayPanel _upgradeInDayPanel;

        private Anim _anim;
        private Image _background;
        private RectTransform _logoRectTransform;
        private Vector3 _logoPosition;
        private Vector3 _logoSize;

        public void Init()
        {
            _anim = gameObject.AddComponent<Anim>();
            _anim.SetTimeAnim(1);

            _background = gameObject.GetComponent<Image>();

            _logoRectTransform = _cardGroupLogo.GetComponent<RectTransform>();
            _logoPosition = _logoRectTransform.localPosition;
            _logoSize = _logoRectTransform.sizeDelta;

            _upgradeCapacityPanel.Init(this);
            _upgradeTimePanel.Init(this);
            _upgradeInDayPanel.Init(this);

            gameObject.SetActive(false);
        }

        public void Show(RectTransform startRect)
        {
            Color backColor = _background.color;
            backColor.a = 0;
            _background.color = backColor;

            _groupNameText.text = Settings.CardGroups[GameData.CurrentCardGroupId].NameGroup;
            Vector3 namePosition = _groupNameText.transform.localPosition;
            namePosition.x = 1080;
            _groupNameText.transform.localPosition = namePosition;

            _cardGroupLogo.sprite = Settings.CardGroups[GameData.CurrentCardGroupId].BackgroundSprite;
            _logoRectTransform.position = startRect.position;
            _logoRectTransform.sizeDelta = startRect.sizeDelta;

            Vector3 info1Position = _info1Text.transform.localPosition;
            info1Position.x = -1080;
            _info1Text.transform.localPosition = info1Position;

            Color info2Color = _info2Text.color;
            info2Color.a = 0;
            _info2Text.color = info2Color;

            _verticalLayoutGroup.padding.top = 2000;
            _verticalLayoutGroup.spacing = 500;

            gameObject.SetActive(true);

            _upgradeCapacityPanel.Show();
            _upgradeTimePanel.Show();
            _upgradeInDayPanel.Show();

            _anim.SetNewSequence();
            _anim.Insert(0, _background.DOFade(1, 0.5f));
            _anim.Insert(0, _logoRectTransform.DOLocalMove(_logoPosition, 0.5f));
            _anim.Insert(0, _logoRectTransform.DOSizeDelta(_logoSize, 0.5f));
            _anim.Insert(0.3f, _groupNameText.transform.DOLocalMoveX(0, 0.5f));
            _anim.Insert(0.3f, _info1Text.transform.DOLocalMoveX(0, 0.5f));
            _anim.Insert(0.5f, _info2Text.DOFade(1, 0.5f));
            _anim.InsertToFloat(0.5f, 2000, 850, 0.5f, (value) => _verticalLayoutGroup.padding = new RectOffset(0, 0, (int)value, 0));
            _anim.InsertToFloat(0.5f, 500, 30, 0.5f, (value) => _verticalLayoutGroup.spacing = (int)value);
        }

        public void Hide()
        {
            MainPanel.Instance.UpdatePanel();
            _upgradeCapacityPanel.SetInteractableButton(false);
            _upgradeTimePanel.SetInteractableButton(false);
            _upgradeInDayPanel.SetInteractableButton(false);

            _anim.SetNewSequence();
            _anim.InsertToFloat(0, 850, 2000, 0.5f, (value) => _verticalLayoutGroup.padding = new RectOffset(0, 0, (int)value, 0));
            _anim.InsertToFloat(0, 30, 500, 0.5f, (value) => _verticalLayoutGroup.spacing = (int)value);
            _anim.Insert(0.3f, _groupNameText.transform.DOLocalMoveX(1080, 0.5f));
            _anim.Insert(0.3f, _info1Text.transform.DOLocalMoveX(-1080, 0.5f));
            _anim.Insert(0.5f, _logoRectTransform.DOSizeDelta(Vector2.zero, 0.5f));
            _anim.Insert(0.5f, _background.DOFade(0, 0.5f));
            _anim.Insert(0.5f, _info2Text.DOFade(0, 0.5f));
            _anim.InsertCallback(1, () => 
            {
                MainPanel.Instance.Show();
                gameObject.SetActive(false);
            });
        }
    }
}