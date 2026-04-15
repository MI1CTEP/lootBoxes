using System;
using UnityEngine;
using UnityEngine.Events;

namespace MyGame
{
    public static class GameData
    {
        public static int CurrentCardGroupId { get; set; } //Текущая группа карточек
        //public static bool IsFirstLaunch => PlayerPrefs.GetInt("first") == 0; //Проверка на первый запуск игры


        public static bool IsFirstLaunch()
        {
            string key = "first";

            if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
            {
                return int.Parse(value) == 0;
            }
            else
            {
                return false;
            }

        }



        // public static void SetNoFirstLaunch() => PlayerPrefs.SetInt("first", 1);
        public static void SetNoFirstLaunch()
        {
            string key = "first";
            string valueDic = 1.ToString();

            if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
            {

            }
            else //если ключ уже есть
            {
                PlayfabService.instance.dicCurrency[key] = valueDic;
            }
        }

        public static class Keys
        {
            private static readonly string _key = "keys";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            //Добавляем ключи и сохраняем. Также говорим нужно ли показывать партикл добавления ключей
            public static void Add(int value, bool showParticle)
            {
                //Позже состыковать с временем выхода и начислять (можно зависывать время в Playfab)

                PlayfabService.instance.dicCurrency[_key] = (value + GetValue()).ToString();
                //PlayerPrefs.SetInt(_key, value + GetValue());
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

            //public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicCurrency[_key]);
        }

        public static class GoldKeys
        {
            private static readonly string _key = "gold_keys";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            public static void Add(int value, bool showParticle)
            {
                //PlayerPrefs.SetInt(_key, value + GetValue());
                PlayfabService.instance.dicCurrency[_key] = (value + GetValue()).ToString();
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

           // public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicCurrency[_key]);
        }

        public static class Labs
        {
            private static readonly string _key = "labs";

            public static UnityAction<int> OnAdd { get; set; }
            public static UnityAction<int> OnParticle { get; set; }

            public static void Add(int value, bool showParticle)
            {
                PlayfabService.instance.dicCurrency[_key] = (value + GetValue()).ToString();
                //PlayerPrefs.SetInt(_key, value + GetValue());
                OnAdd?.Invoke(value);
                if (showParticle)
                    OnParticle?.Invoke(value);
            }

           // public static int GetValue() => PlayerPrefs.GetInt(_key);
            public static int GetValue() => int.Parse(PlayfabService.instance.dicCurrency[_key]);
        }

        //Опыт внутри группы карт (для прокачки уровня группы карт)
        public static class CardGroupExperience
        {
            private static readonly string _key = "card_group_experience_";

            public static void Add(int idGroup, int value)
            {
                string key = _key + idGroup;
                string valueDic = (Load(idGroup) + value).ToString();

                if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                {
                    
                }
                else //если ключ уже есть
                {
                    PlayfabService.instance.dicCurrency[key] = valueDic;
                }

               // PlayerPrefs.SetInt(_key + idGroup, Load(idGroup) + value);
            }

            //public static int Load(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

            public static int Load(int idGroup)
            {

                string key = _key + idGroup;

                if(PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                {
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
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

                if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                {

                }
                else //если ключ уже есть
                {
                    PlayfabService.instance.dicCurrency[key] = valueDic;
                }

               // PlayerPrefs.SetInt(_key + idGroup, Load(idGroup) + 1);
            }

           // public static int Load(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);


            public static int Load(int idGroup)
            {

                string key = _key + idGroup;

                if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                {
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
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

                if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                {

                }
                else //если ключ уже есть
                {
                    PlayfabService.instance.dicCurrency[key] = valueDic;
                }


                //int currentLevel = LoadLevel(idGroup);
                //PlayerPrefs.SetInt(_key + idGroup, currentLevel + 1);
            }

            //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

            public static int LoadLevel(int idGroup)
            {

                string key = _key + idGroup;

                if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                {
                    return int.Parse(value);
                }
                else
                {
                    return 0;
                }
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

                    if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                    {

                    }
                    else //если ключ уже есть
                    {
                        PlayfabService.instance.dicCurrency[key] = valueDic;
                    }

                    //int currentLevel = LoadLevel(idGroup);
                    //PlayerPrefs.SetInt(_key + idGroup, currentLevel + 1);
                    OnUpLevel?.Invoke(idGroup);
                }

                //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_key + idGroup);

                public static int LoadLevel(int idGroup)
                {

                    string key = _key + idGroup;

                    if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                    {
                        return int.Parse(value);
                    }
                    else
                    {
                        return 0;
                    }
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

                    if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                    {

                    }
                    else //если ключ уже есть
                    {
                        PlayfabService.instance.dicCurrency[key] = valueDic;
                    }


                    //int currentLevel = LoadLevel(idGroup);
                    //PlayerPrefs.SetInt(_keyLevel + idGroup, currentLevel + 1);
                }


                //public static int LoadLevel(int idGroup) => PlayerPrefs.GetInt(_keyLevel + idGroup);

                public static int LoadLevel(int idGroup)
                {

                    string key = _keyLevel + idGroup;

                    if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                    {
                        return int.Parse(value);
                    }
                    else
                    {
                        return 0;
                    }
                }





                //Говорим игре, что сегодня мы 1 раз заработали ключи в группе
                public static void AddEarnedToday(int idGroup)
                {
                    //PlayerPrefs.SetInt(_keyEarnedToday + idGroup, LoadEarnedToday(idGroup) + 1);


                    string key = _keyEarnedToday + idGroup;
                    string valueDic = (LoadEarnedToday(idGroup) + 1).ToString();

                    if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                    {

                    }
                    else //если ключ уже есть
                    {
                        PlayfabService.instance.dicCurrency[key] = valueDic;
                    }
                }


                //Спрашиваем у игры, сколько раз мы сегодня заработали ключи внутри группы
                public static int LoadEarnedToday(int idGroup)
                {
                    if (DateTime.Now.DayOfYear != GetLastDay(idGroup))
                    {
                        SaveLastDay(idGroup);

                        string keyDic = _keyEarnedToday + idGroup;
                        string valueDic = 0.ToString();

                        if (PlayfabService.instance.dicCurrency.TryAdd(keyDic, valueDic)) //если новый ключ
                        {

                        }
                        else //если ключ уже есть
                        {
                            PlayfabService.instance.dicCurrency[keyDic] = valueDic;
                        }


                        //PlayerPrefs.SetInt(_keyEarnedToday + idGroup, 0);
                        return 0;
                    }



                    string key = _keyEarnedToday + idGroup;

                    if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                    {
                        return int.Parse(value);
                    }
                    else
                    {
                        return 0;
                    }


                    //return PlayerPrefs.GetInt(_keyEarnedToday + idGroup);
                }




                //private static void SaveLastDay(int idGroup) => PlayerPrefs.SetInt(_keyLastDay + idGroup, DateTime.Now.DayOfYear);

                private static void SaveLastDay(int idGroup)
                {
                    string key = _keyLastDay + idGroup;
                    string valueDic = DateTime.Now.DayOfYear.ToString();

                    if (PlayfabService.instance.dicCurrency.TryAdd(key, valueDic)) //если новый ключ
                    {

                    }
                    else //если ключ уже есть
                    {
                        PlayfabService.instance.dicCurrency[key] = valueDic;
                    }
                }




                //private static int GetLastDay(int idGroup) => PlayerPrefs.GetInt(_keyLastDay + idGroup);

                public static int GetLastDay(int idGroup)
                {

                    string key = _keyLastDay + idGroup;

                    if (PlayfabService.instance.dicCurrency.TryGetValue(key, out var value))
                    {
                        return int.Parse(value);
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }
    }
}