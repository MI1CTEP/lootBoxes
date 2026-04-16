using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace MyGame
{
    public static class GameData
    {
        public const string KEY_SAVE_GAME_DATA = "save_game_data";
        public static int CurrentCardGroupId { get; set; } //Текущая группа карточек
        //public static bool IsFirstLaunch => PlayerPrefs.GetInt("first") == 0; //Проверка на первый запуск игры

        private static void SaveNewData(string key, string valueData)
        {
            if (PlayfabService.instance.dicGameData.TryAdd(key, valueData)) //если новый ключ
            {

            }
            else //если ключ уже есть
            {
                PlayfabService.instance.dicGameData[key] = valueData;
            }
        }

        private static T LoadData<T>(string key, T valueDefolt)
        {
            if (PlayfabService.instance.dicGameData.TryGetValue(key, out var value))
            {
                if (typeof(T) == typeof(bool))
                {
                    Debug.Log(value);
                    var result = !(bool)(object)valueDefolt;
                    Debug.Log(result);
                    return (T)(object)result;
                }  
                else
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }

            }
            else
            {
                return valueDefolt;
            }
        }


        public static bool IsFirstLaunch()
        {
            string key = "first";



            return LoadData(key, true);

        }



        // public static void SetNoFirstLaunch() => PlayerPrefs.SetInt("first", 1);
        public static void SetNoFirstLaunch()
        {
            string key = "first";
            string valueDic = 1.ToString();


            SaveNewData(key, valueDic);

        }

        public static class Keys
        {
            private static readonly string _key = "keys";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            //Добавляем ключи и сохраняем. Также говорим нужно ли показывать партикл добавления ключей
            public static void Add(int value, bool showParticle)
            {

                PlayfabService.instance.dicGameData[_key] = (value + GetValue()).ToString();
                //PlayerPrefs.SetInt(_key, value + GetValue());
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

            //public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicGameData[_key]);
        }

        public static class GoldKeys
        {
            private static readonly string _key = "gold_keys";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            public static void Add(int value, bool showParticle)
            {
                //PlayerPrefs.SetInt(_key, value + GetValue());
                PlayfabService.instance.dicGameData[_key] = (value + GetValue()).ToString();
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

           // public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicGameData[_key]);
        }

        public static class Labs
        {
            private static readonly string _key = "labs";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            public static void Add(int value, bool showParticle)
            {
                PlayfabService.instance.dicGameData[_key] = (value + GetValue()).ToString();
                //PlayerPrefs.SetInt(_key, value + GetValue());
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

           // public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicGameData[_key]);
        }

        //Опыт внутри группы карт (для прокачки уровня группы карт)
        public static class CardGroupExperience
        {
            private static readonly string _key = "card_group_experience_";

            public static void Add(int idGroup, int value)
            {
                string key = _key + idGroup;
                string valueDic = (Load(idGroup) + value).ToString();

                SaveNewData(key, valueDic);


               // PlayerPrefs.SetInt(_key + idGroup, Load(idGroup) + value);
            }

            //public static int Load(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

            public static int Load(int idGroup)
            {

                string key = _key + idGroup;

                return LoadData(key, 0);


            }

        }

        //Уровень внутри группы карт
        public static class CardGroupLevel
        {
            private static readonly string _key = "card_group_level_";

            public static void Up(int idGroup)
            {
                string key = _key + idGroup;
                string valueDic = (Load(idGroup) + 1).ToString();

                SaveNewData(key, valueDic);


               // PlayerPrefs.SetInt(_key + idGroup, Load(idGroup) + 1);
            }

           // public static int Load(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);


            public static int Load(int idGroup)
            {

                string key = _key + idGroup;

                return LoadData(key, 0);


            }
        }

        //1 параметр) У каждой группы карт есть параметры, у этих параметров свой уровень. Данный параметр это вместимость карточек, а именно уровнь (на сколько прокачана вместимость)
        public static class CardGroupCapacity
        {
            private static readonly string _key = "card_group_capacity_";


            public static void UpLevel(int idGroup)
            {
                string key = _key + idGroup;
                string valueDic = (LoadLevel(idGroup) + 1).ToString();


                SaveNewData(key, valueDic);



                //int currentLevel = LoadLevel(idGroup);
                //PlayerPrefs.SetInt(_key + idGroup, currentLevel + 1);
            }

            //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

            public static int LoadLevel(int idGroup)
            {

                string key = _key + idGroup;

                return LoadData(key, 0);


            }
        }

        //2 параметр) Зароботок внутри группы
        public static class CardGroupPassiveIncome
        {
            //Подпараметр. Время зароботка ключей внутри группы
            public static class Time
            {
                private static readonly string _key = "time_passive_income_";

                public static UnityAction<int> OnUpLevel { get; set; }

                public static void UpLevel(int idGroup)
                {

                    string key = _key + idGroup;
                    string valueDic = (LoadLevel(idGroup) + 1).ToString();

                    SaveNewData(key, valueDic);


                    //int currentLevel = LoadLevel(idGroup);
                    //PlayerPrefs.SetInt(_key + idGroup, currentLevel + 1);
                    OnUpLevel?.Invoke(idGroup);
                }

                //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

                public static int LoadLevel(int idGroup)
                {

                    string key = _key + idGroup;

                    return LoadData(key, 0);


                }
            }

            //Подпараметр. Сколько раз можно заработать ключи внутри группы
            public static class InDay
            {
                private static readonly string _keyLevel = "in_day_passive_income_"; //Уровнеь, или сколько раз в день мы можем зарабатывать
                private static readonly string _keyEarnedToday = "earned_today_passive_income_"; //Сколько раз мы сегодня заработали
                private static readonly string _keyLastDay = "last_day_passive_income_"; //В какой день мы последний раз зарабатывали

                public static void UpLevel(int idGroup)
                {

                    string key = _keyLevel + idGroup;
                    string valueDic = (LoadLevel(idGroup) + 1).ToString();


                    SaveNewData(key, valueDic);


                    //int currentLevel = LoadLevel(idGroup);
                    //PlayerPrefs.SetInt(_keyLevel + idGroup, currentLevel + 1);
                }


                //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_keyLevel + idGroup);

                public static int LoadLevel(int idGroup)
                {

                    string key = _keyLevel + idGroup;

                    return LoadData(key, 0);


                }





                //Говорим игре, что сегодня мы 1 раз заработали ключи в группе
                public static void AddEarnedToday(int idGroup)
                {
                    //PlayerPrefs.SetInt(_keyEarnedToday + idGroup, LoadEarnedToday(idGroup) + 1);


                    string key = _keyEarnedToday + idGroup;
                    string valueDic = (LoadEarnedToday(idGroup) + 1).ToString();

                    SaveNewData(key, valueDic);


                }


                //Спрашиваем у игры, сколько раз мы сегодня заработали ключи внутри группы
                public static int LoadEarnedToday(int idGroup)
                {
                    if (DateTime.Now.DayOfYear != GetLastDay(idGroup))
                    {
                        SaveLastDay(idGroup);

                        string keyDic = _keyEarnedToday + idGroup;
                        string valueDic = 0.ToString();

                        SaveNewData(keyDic, valueDic);




                        //PlayerPrefs.SetInt(_keyEarnedToday + idGroup, 0);
                        return 0;
                    }



                    string key = _keyEarnedToday + idGroup;

                    return LoadData(key, 0);


                    //return PlayerPrefs.GetInt(_keyEarnedToday + idGroup);
                }




                //private static void SaveLastDay(int idGroup) => PlayerPrefs.SetInt(_keyLastDay + idGroup, DateTime.Now.DayOfYear);

                private static void SaveLastDay(int idGroup)
                {
                    string key = _keyLastDay + idGroup;
                    string valueDic = DateTime.Now.DayOfYear.ToString();


                    SaveNewData(key, valueDic);

                }




                //private static int GetLastDay(int idGroup) => PlayerPrefs.GetInt(_keyLastDay + idGroup);

                public static int GetLastDay(int idGroup)
                {

                    string key = _keyLastDay + idGroup;

                    return LoadData(key, 0);


                }
            }
        }

        public static class AutoAddKeys
        {
            private static readonly string _keyPremium = "auto_add_keys_premium";
            private static readonly string _keyLastTime = "auto_add_keys_last_time";

            public static UnityAction OnActivatePremium { get; set; }

            //Активируем ускоренный зароботок ключей
            public static void ActivatePremium()
            {
                //PlayerPrefs.SetInt(_keyPremium, 1);


                string key = _keyPremium;
                string valueDic = 1.ToString();

                SaveNewData(key, valueDic);



                OnActivatePremium?.Invoke();
            }

            //Проверяем активирована ли ускоренный зароботок ключей
            public static bool IsPremium()
            {
                string key = _keyPremium;

                return LoadData(key, false);



                //return PlayerPrefs.GetInt(_keyPremium) == 1;
            }

            //Сохраняем текущее время в секундах
            public static void SaveLastTime()
            {
                //int currentTime = GetCurrentTime();
                //PlayerPrefs.SetInt(_keyLastTime, currentTime);

                string key = _keyLastTime;
                string valueDic = GetCurrentTime().ToString();

                SaveNewData(key, valueDic);


            }

            //Cтолько прошло секунд с последнего сохранения времени
            public static int LoadElapsedTime()
            {
                int currentTime = GetCurrentTime();
                //int lastTime = PlayerPrefs.GetInt(_keyLastTime);
                //return currentTime - lastTime;


                string key = _keyLastTime;

                return LoadData(key, currentTime);

            }

            //Столько прошло секунд с 1 января 2026 года
            private static int GetCurrentTime()
            {
                DateTime epoch = new(2026, 1, 1);
                return (int)(DateTime.Now - epoch).TotalSeconds;
            }
        }
    }
}