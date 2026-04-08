using UnityEngine;

namespace MyGame.Cards
{
    [CreateAssetMenu(menuName = "SO/CardsSettings")]
    public sealed class CardsSettings : ScriptableObject
    {
        [SerializeField] private Color[] _outlineColors;
        [SerializeField] private int[] _keyPrices;
        [SerializeField] private int[] _labPrices;

        public Color GetOutlineColor(int rank) => _outlineColors[rank];
        public int GetKeyPrice(int rank) => _keyPrices[rank];
        public int GetLabPrice(int rank) => _labPrices[rank];
    }
}