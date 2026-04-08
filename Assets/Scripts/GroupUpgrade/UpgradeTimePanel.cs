using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame.GroupUpgrade
{
    public sealed class UpgradeTimePanel : UpgradeCardGroupPanel
    {
        [SerializeField] private Text _upgradeText;

        public void Show()
        {
            if (GameData.CardGroupPassiveIncome.Time.LoadLevel(GameData.CurrentCardGroupId) < Settings.Upgrades.income.maxLevelTime)
            {
                gameObject.SetActive(true);
                int startTime = Settings.Upgrades.income.startTime;
                int stepTime = Settings.Upgrades.income.stepTime;
                int currentTime = startTime - stepTime * GameData.CardGroupPassiveIncome.Time.LoadLevel(GameData.CurrentCardGroupId);
                int upgradedTime = startTime - stepTime * (1 + GameData.CardGroupPassiveIncome.Time.LoadLevel(GameData.CurrentCardGroupId));
                _upgradeText.text = $"{currentTime}→<color=#85FE65>{upgradedTime}</color>";
                SetInteractableButton(true);
            }
            else gameObject.SetActive(false);
        }

        protected override void OnClick()
        {
            GameData.CardGroupPassiveIncome.Time.UpLevel(GameData.CurrentCardGroupId);
        }
    }
}
