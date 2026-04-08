using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "SO/Settings")]
    public sealed class Settings : ScriptableObject
    {
        [Tooltip("Настройки каждого ранга")]
        [SerializeField] private RankCardSetting[] _rankCardSetting;
        public static RankCardSetting[] RankCard { get; private set; }

        [Tooltip("Содержит все группы-сеты карточек")]
        [SerializeField] private CardsGroup[] _cardGroups;
        public static CardsGroup[] CardGroups { get; private set; }

        [Tooltip("Настройки улучшения групп-сеттов карточек")]
        [SerializeField] private UpgradesSetting _upgradesSetting;
        public static UpgradesSetting Upgrades { get; private set; }

        [Tooltip("Настройки магазина")]
        [SerializeField] private ShopSetting _shopSetting;
        public static ShopSetting Shop { get; private set; }

        public void Init()
        {
            RankCard = _rankCardSetting;
            CardGroups = _cardGroups;
            Upgrades = _upgradesSetting;
            Shop = _shopSetting;

            for (int i = 0; i < _cardGroups.Length; i++)
                _cardGroups[i].Id = i;
        }
    }

    [System.Serializable]
    public sealed class RankCardSetting
    {
        [Tooltip("Цвет обводки и цвет номера карточки")] 
        public Color outlineColor;

        [Tooltip("Цена карточки в ключах при продаже")]
        public int priceKey;

        [Tooltip("Цена карточки в реагенте. Такой будет цена при продаже, а также при объединении карточек")]
        public int priceLab;
    }

    [System.Serializable]
    public sealed class UpgradesSetting
    {
        [Tooltip("Необходимое количество опыта для открытия улучшения у группы карточек")]
        public ExperienceSetting experience;

        [Tooltip("Настройки максимального количества карточек внутри группы")]
        public CapacitySetting capacity;

        [Tooltip("Настройки пассивного зароботка внутри группы")]
        public IncomeSettings income;
    }

    [System.Serializable]
    public sealed class ExperienceSetting
    {
        [Tooltip("Стартовое значение, то есть значение на 1 уровне группы")]
        public int startValue;

        [Tooltip("С каждым новым уровнем необходимое количество опыта будет увеличиваться на это значение")]
        public int stepValue;
    }

    [System.Serializable]
    public sealed class CapacitySetting
    {
        [Tooltip("Максимальное количество карточек в группе. Значения для первого уровня")]
        public int startValue;

        [Tooltip("Максимальное количество карточек в группе будет увеличиваться на это значение")]
        public int stepValue;
    }

    [System.Serializable]
    public sealed class IncomeSettings
    {
        [Tooltip("Время, необходимое для зароботка ключей внутри группы в секундах на первом уровне")]
        public int startTime;

        [Tooltip("С каждым новым уровнем время зароботка будет уменьшаться на это значение в секундах")]
        public int stepTime;

        [Tooltip("Cтолько раз можно прокачать этот параметр")]
        public int maxLevelTime;

        [Tooltip("Столько раз в день можно заработать ключи внутри группы")]
        public int startQuantity;

        [Tooltip("Необходимый уровень для увеличения количества итераций зароботка в день")]
        public int neededGroupLevelToUpgradeQuantity;
    }

    [System.Serializable]
    public sealed class ShopSetting
    {
        [Tooltip("Столько золотых ключей добавится при покупке")]
        public int addGoldKey;

        [Tooltip("Столько ключей добавится при покупке")]
        public int addKey;

        [Tooltip("Столько реагента добавится при покупке")]
        public int addLab;
    }
}