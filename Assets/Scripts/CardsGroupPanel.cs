using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;
using MyGame.GroupUpgrade;

namespace MyGame
{
    public sealed class CardsGroupPanel : MonoBehaviour
    {
        [SerializeField] private Image _logo;
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _capacityValueText;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Text _upgradeButtonText;
        [SerializeField] private Image _progress;
        [SerializeField] private UpgradeLevelPanel _upgradeLevelPanel;

        private int _neededExperience;

        public void Init()
        {
            GameData.Keys.OnAdd += UpdateProgress;

            _upgradeLevelPanel.Init();
            _upgradeButton.onClick.AddListener(Upgrade);
        }

        public void UpdateGroup()
        {
            _neededExperience = Settings.Upgrades.experience.startValue + Settings.Upgrades.experience.stepValue * GameData.CardGroupLevel.Load(GameData.CurrentCardGroupId);

            CardsGroup cardsGroup = Settings.CardGroups[GameData.CurrentCardGroupId];
            _logo.sprite = cardsGroup.BackgroundSprite;
            _nameText.text = $"{cardsGroup.NameGroup} <color=#2E2E2E>спнбемэ {GameData.CardGroupLevel.Load(GameData.CurrentCardGroupId) + 1}</color>";
            int maxCapacity = GameData.CardGroupCapacity.LoadLevel(GameData.CurrentCardGroupId) * Settings.Upgrades.capacity.stepValue + Settings.Upgrades.capacity.startValue;
            _capacityValueText.text = $"{CardsData.GetGroup(GameData.CurrentCardGroupId).Count}/{maxCapacity}";

            UpdateProgress();
        }

        private void UpdateProgress(int value = 0)
        {
            int curentExperience = GameData.CardGroupExperience.Load(GameData.CurrentCardGroupId);
            if (curentExperience >= _neededExperience)
            {
                _upgradeButton.interactable = true;
                _progress.fillAmount = 1;
                _upgradeButtonText.text = "сксвьхрэ";
            }
            else
            {
                _upgradeButton.interactable = false;
                _progress.fillAmount = (float)curentExperience / _neededExperience;
                _upgradeButtonText.text = $"{curentExperience}/{_neededExperience}";
            }
        }

        private void Upgrade()
        {
            MainPanel.Instance.Hide();
            _upgradeLevelPanel.Show(_logo.GetComponent<RectTransform>());
        }

        private void OnDestroy()
        {
            GameData.Keys.OnAdd -= UpdateProgress;
        }
    }
}