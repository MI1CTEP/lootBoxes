using UnityEngine;

namespace MyGame
{
    [CreateAssetMenu(menuName = "SO/CardsGroup")]
    public sealed class CardsGroup : ScriptableObject
    {
        [SerializeField] private Sprite _backgroundSprite;
        [SerializeField] private string _nameGroup;
        [SerializeField] private string[] _namesCard;

        public Sprite BackgroundSprite => _backgroundSprite;
        public string NameGroup => _nameGroup;
        public string GetNameCard(int id) => _namesCard[id];
        public int CardsLength => _namesCard.Length;
        public int Id { get; set; }
    }
}