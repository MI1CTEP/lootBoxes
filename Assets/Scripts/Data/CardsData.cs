using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using MyGame.Cards;

namespace MyGame
{
    public static class CardsData
    {
        private static List<Card>[] _cards;
        private readonly static string _keyForSave = "all_cards_";

        public static List<Card> GetGroup(int groupId) => _cards[groupId];

        //подгружаю данные при старте игры
        public static void Load()
        {
            _cards = new List<Card>[Settings.CardGroups.Length];

            for (int i = 0; i < Settings.CardGroups.Length; i++)
            {
                if (PlayerPrefs.HasKey(_keyForSave + i))
                {
                    string text = PlayerPrefs.GetString(_keyForSave + i);
                    _cards[i] = JsonConvert.DeserializeObject<List<Card>>(text);
                }
                else _cards[i] = new();
            }
        }

        //»грок получил новую карту
        public static void Add(Card card)
        {
            _cards[card.groupId].Add(card);
            Save(card.groupId);
        }

        //»грок продал или удалил карту
        public static void Delete(Card card)
        {
            _cards[card.groupId].Remove(card);
            Save(card.groupId);
        }

        public static void DeleteRange(List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
                _cards[cards[i].groupId].Remove(cards[i]);
            Save(cards[0].groupId);
        }

        //—охран€ю json в строку после изменени€ списка карт
        private static void Save(int idGroup)
        {
            string text = JsonConvert.SerializeObject(_cards[idGroup]);
            PlayerPrefs.SetString(_keyForSave + idGroup, text);
        }
    }
}