using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame.GroupUpgrade
{
    public sealed class UpgradeInDayPanel : UpgradeCardGroupPanel
    {
        [SerializeField] private Text _upgradeText;

        public void Show()
        {
            if (GameData.CardGroupPassiveIncome.InDay.LoadLevel(GameData.CurrentCardGroupId) < 1 &&
                GameData.CardGroupLevel.Load(GameData.CurrentCardGroupId) >= Settings.Upgrades.income.neededGroupLevelToUpgradeQuantity - 1)
            {
                gameObject.SetActive(true);
                int startInDay = Settings.Upgrades.income.startQuantity;
                _upgradeText.text = $"{startInDay}→<color=#85FE65>{startInDay + 1}</color>";
                SetInteractableButton(true);
            }
            else gameObject.SetActive(false);
        }

        protected override void OnClick()
        {
            GameData.CardGroupPassiveIncome.InDay.UpLevel(GameData.CurrentCardGroupId);
        }
    }
}