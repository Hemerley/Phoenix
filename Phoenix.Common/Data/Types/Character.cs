namespace Phoenix.Common.Data.Types
{
    public class Character
    {
        #region -- Database Fields --
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string Image { get; set; }
        public string Gender { get; set; }
        public string HisHer { get; set; }
        public string HeShe { get; set; }
        public int Experience { get; set; }
        public string Title { get; set; }
        public int CasteID { get; set; }
        public int RankID { get; set; }
        public string Philosophy { get; set; }
        public int PhilosophyID { get; set; }
        public int Alignment { get; set; }
        public int Creation { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intellect { get; set; }
        public int Stamina { get; set; }
        public int Damage { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int RoomID { get; set; }

        #endregion

        #region -- Server Fields --

        #region -- General Statistics --
        public string Type { get; set; }
        public string Caste { get; set; }
        public string Rank { get; set; }
        public double Crit { get; set; }
        public double Haste { get; set; }
        public double Mastery { get; set; }
        public double Versatility { get; set; }
        #endregion

        #region -- Combat Statistics --
        public double AttackSpeed { get; set; }
        public bool IsAttacking { get; set; }
        public bool IsDead { get; set; }
        public int TargetID { get; set; }
        #endregion

        #region -- Resource Statistics --
        public int CurrentHealth { get; set; }
        public int CurrentMana { get; set; }
        #endregion

        #endregion
    }
}