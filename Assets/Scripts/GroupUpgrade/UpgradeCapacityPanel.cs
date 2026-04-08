using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame.GroupUpgrade
{
    public sealed class UpgradeCapacityPanel : UpgradeCardGroupPanel
    {
        [SerializeField] private Text _upgradeText;

        public void Show()
        {
            gameObject.SetActive(true);
            SetInteractableButton(true);
            int startCapacity = Settings.Upgrades.capacity.startValue;
            int stepCapacity = Settings.Upgrades.capacity.stepValue;
            int currentCapacity = startCapacity + stepCapacity * GameData.CardGroupCapacity.LoadLevel(GameData.CurrentCardGroupId);
            int upgradedCapacity = startCapacity + stepCapacity * (1 + GameData.CardGroupCapacity.LoadLevel(GameData.CurrentCardGroupId));
            _upgradeText.text = $"{currentCapacity}→<color=#85FE65>{upgradedCapacity}</color>";
        }

        protected override void OnClick()
        {
            GameData.CardGroupCapacity.UpLevel(GameData.CurrentCardGroupId);
        }
    }
}