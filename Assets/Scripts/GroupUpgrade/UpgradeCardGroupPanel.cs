using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MyGame.Cards;

namespace MyGame.GroupUpgrade
{
    public abstract class UpgradeCardGroupPanel : MonoBehaviour
    {
        [SerializeField] private Button _upgradeButton;

        private UpgradeLevelPanel _upgradeLevelPanel;

        public static UnityAction<int> OnUpgrade { get; set; }

        public void Init(UpgradeLevelPanel upgradeLevelPanel)
        {
            _upgradeLevelPanel = upgradeLevelPanel;
            _upgradeButton.onClick.AddListener(Click);
        }

        private void Click()
        {
            int experience = Settings.Upgrades.experience.startValue + Settings.Upgrades.experience.stepValue * GameData.CardGroupLevel.Load(GameData.CurrentCardGroupId);
            GameData.CardGroupExperience.Add(GameData.CurrentCardGroupId, -experience);
            GameData.CardGroupLevel.Up(GameData.CurrentCardGroupId);
            OnUpgrade?.Invoke(GameData.CurrentCardGroupId);
            OnClick();
            _upgradeLevelPanel.Hide();
        }

        protected abstract void OnClick();

        public void SetInteractableButton(bool value) => _upgradeButton.interactable = value;
    }
}