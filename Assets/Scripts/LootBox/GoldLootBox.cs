using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame.LootBox
{
    public sealed class GoldLootBox : MonoBehaviour
    {
        [SerializeField] private LootBoxSettings _lootBoxSettings;
        [SerializeField] private Button _buyButton;
        [SerializeField] private Text _buyButtonText;

        private CardPanel _cardPanel;

        public void Init(CardPanel cardPanel)
        {
            _cardPanel = cardPanel;
            _buyButton.onClick.AddListener(Buy);
            _buyButtonText.text = $"йсохрэ <color=white>{_lootBoxSettings.Price}</color>";
        }

        private void Buy()
        {
            if (GameData.GoldKeys.GetValue() >= _lootBoxSettings.Price)
            {
                GameData.GoldKeys.Add(-_lootBoxSettings.Price, true);
                Card card = GetRandomCard();
                _cardPanel.ShowFromLootBox(card, _lootBoxSettings);
            }
            else Shop.Instance.Show();
        }

        private Card GetRandomCard()
        {
            int random = Random.Range(0, Settings.CardGroups[GameData.CurrentCardGroupId].CardsLength);
            int rank = _lootBoxSettings.GetRank();

            return Card.GetNew(GameData.CurrentCardGroupId, random, rank);
        }
    }
}