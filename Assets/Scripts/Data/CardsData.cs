using MyGame.Cards;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace MyGame
{
    public static class CardsData
    {
        private static List<Card>[] _cards;
        private readonly static string _keyForSave = "all_cards_";
        private const string FULL_SAVE_KEY = "cards_full_save"; // Ключ для полного сохранения



        public static List<Card> GetGroup(int groupId) => _cards[groupId];

        //подгружаю данные при старте игры
        public static void Load()
        {
            //string json = PlayfabService.instance.JsonCardsData;

            string json = PlayfabService.instance.dicCurrency[FULL_SAVE_KEY];

            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Нет сохранения, создаем пустые группы");
                _cards = new List<Card>[Settings.CardGroups.Length];
                for (int i = 0; i < _cards.Length; i++)
                    _cards[i] = new List<Card>();
                return;
            }


            //string json = PlayerPrefs.GetString(FULL_SAVE_KEY);
            AllCardsData allData = JsonConvert.DeserializeObject<AllCardsData>(json);


            _cards = new List<Card>[allData.groups.Count];
            for (int i = 0; i < allData.groups.Count; i++)
            {
                _cards[i] = allData.groups[i] ?? new List<Card>();
            }

        }

        //Игрок получил новую карту
        public static void Add(Card card)
        {
            _cards[card.groupId].Add(card);
            Save(card.groupId);
        }

        //Игрок продал или удалил карту
        public static void Delete(Card card)
        {
            _cards[card.groupId].Remove(card);
            Save(card.groupId);
        }

        //Сохраняю json в строку после изменения списка карт. ДУМАЮ ЭТОТ МЕТОД БОЛЬШЕ НЕ НУЖЕН
        private static void Save(int idGroup)
        {

            // Создаем контейнер для всех данных
            //AllCardsData allData = new AllCardsData();
            //allData.groups = new List<List<Card>>();

            //for (int i = 0; i < _cards.Length; i++)
            //{
            //    allData.groups.Add(_cards[i]);
            //}

            //string json = JsonConvert.SerializeObject(allData, Formatting.None);


            string json = GetDataCardsToString();

            PlayerPrefs.SetString(FULL_SAVE_KEY, json);
            PlayerPrefs.Save();


            //В Playfab
            //PlayfabService.instance.SetUserData(FULL_SAVE_KEY, json);

        }

        public static string GetDataCardsToString()
        {
            AllCardsData allData = new AllCardsData();
            allData.groups = new List<List<Card>>();

            for (int i = 0; i < _cards.Length; i++)
            {
                allData.groups.Add(_cards[i]);
            }

            string json = JsonConvert.SerializeObject(allData, Formatting.None);

            return json;
        }



    }


    [System.Serializable]
    public class AllCardsData
    {
        public List<List<Card>> groups;
    }
}