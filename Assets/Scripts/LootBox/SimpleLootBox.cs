using UnityEngine;
using UnityEngine.UI;
using MyGame.Cards;

namespace MyGame.LootBox
{
    public sealed class SimpleLootBox : MonoBehaviour
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
            if (GameData.Keys.GetValue() >= _lootBoxSettings.Price)
            {
                GameData.Keys.Add(-_lootBoxSettings.Price, true);
                Card card = GetRandomCard();
                _cardPanel.ShowFromLootBox(card, _lootBoxSettings);
            }
            else Shop.Instance.Show();
        }

        private Card GetRandomCard()
        {
            int value = 0;
            for (int i = 0; i < Settings.CardGroups.Length; i++)
                value += Settings.CardGroups[i].CardsLength;

            int random = Random.Range(0, value);
            int idGroup = 0;
            int id = 0;

            for (int i = 0; i < Settings.CardGroups.Length; i++)
            {
                if (random - Settings.CardGroups[i].CardsLength >= 0)
                {
                    random -= Settings.CardGroups[i].CardsLength;
                    idGroup++;
                }
                else
                {
                    id = random;
                    break;
                }
            }
            int rank = _lootBoxSettings.GetRank();

            return Card.GetNew(idGroup, id, rank);
        }
    }
}