using Newtonsoft.Json;

namespace MyGame.Cards
{
    public sealed class Card
    {
        public int groupId;
        public int id;
        public int rank;
        [JsonIgnore] public bool isNew;

        public static Card GetNew(int groupId, int id, int rank)
        {
            Card card = new();
            card.groupId = groupId;
            card.id = id;
            card.rank = rank;
            card.isNew = true;
            return card;
        }
    }
}