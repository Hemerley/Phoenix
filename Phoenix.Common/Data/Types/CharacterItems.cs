namespace Phoenix.Common.Data.Types
{
    class CharacterItems
    {
        public int ID { get; set; }
        public int CharacterID { get; set; }
        public int ItemID { get; set; }
        public int ItemAmount { get; set; }
        public int SlotIndex { get; set; }
        public bool IsEquipped { get; set; }
    }
}
