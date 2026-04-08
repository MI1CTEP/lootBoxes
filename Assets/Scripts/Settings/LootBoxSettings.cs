using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "SO/LootBoxSettings")]
    public sealed class LootBoxSettings : ScriptableObject
    {
        [Tooltip("Цена контейнера")]
        [SerializeField] private int _price;

        [Tooltip("Шанс выпадения ранга (серый/синий/фиолетовый/золотой). Это не %. Если значения будут 1/1/1/1, то реальные шансы будут 25%/25%/25%/25%")]
        [SerializeField] private int[] _rankChances;

        [Tooltip("При открытии контейнера игроку выпадет минимум(x) и максимум(y) ключей")]
        [SerializeField] private Vector2Int _keys;

        [Tooltip("При открытии контейнера игроку выпадет минимум(x) и максимум(y) реагента")]
        [SerializeField] private Vector2Int _labs;

        [Tooltip("При открытии контейнера игроку выпадет минимум(x) и максимум(y) золотых ключей")]
        [SerializeField] private Vector2Int _goldKeys;

        public int Price => _price;
        public int GetRandomKeys => Random.Range(_keys.x, _keys.y + 1);
        public int GetRandomLabs => Random.Range(_labs.x, _labs.y + 1);
        public int GetRandomGoldKey => Random.Range(_goldKeys.x, _goldKeys.y + 1);

        public int GetRank()
        {
            int fullChance = 0;
            for (int i = 0; i < _rankChances.Length; i++)
                fullChance += _rankChances[i];

            int random = Random.Range(0, fullChance);

            for (int i = _rankChances.Length - 1; i >= 0; i--)
            {
                fullChance -= _rankChances[i];
                if (fullChance <= random)
                    return i;
            }
            return 0;
        }
    }
}