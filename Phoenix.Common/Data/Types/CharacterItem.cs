namespace Phoenix.Common.Data.Types
{
    public class CharacterItem
    {
        public int Id { get; set; }
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public int SlotIndex { get; set; }
        public int IsEquipped { get; set; }
    }
}
