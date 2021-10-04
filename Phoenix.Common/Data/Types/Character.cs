using System.Collections.Generic;

namespace Phoenix.Common.Data.Types
{
    public class Character
    {
        #region -- Database Fields --
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; } = "";
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
        public int CurrentHealth { get; set; }
        public int CurrentMana { get; set; }
        public bool AutoLoot { get; set; }
        public bool AutoAttack { get; set; }
        public int Recall { get; set; }
        public bool HealthRegen { get; set; }
        public bool IsDead { get; set; }
        public bool IsGhosted { get; set; }
        #endregion

        #region -- Server Fields --

        #region -- General Statistics --
        public List<Item> Items { get; set; }
        public string Type { get; set; }
        public string Caste { get; set; }
        public string Rank { get; set; }
        public double Crit { get; set; }
        public double Haste { get; set; }
        public double Mastery { get; set; }
        public double Versatility { get; set; }
        public int Gold { get; set; }
        public int CurrentStrength { get; set; }
        public int CurrentAgility { get; set; }
        public int CurrentIntellect { get; set; }
        public int CurrentStamina { get; set; }
        public int CurrentDamage { get; set; }
        public int CurrentArmor { get; set; }
        #endregion

        #region -- Combat Statistics --
        public double AttackSpeed { get; set; } = 1;
        public bool IsAttacking { get; set; } = false;
        public string TargetID { get; set; } = "";
        public bool TargetIsPlayer { get; set; } = false;
        #endregion

        #region -- Resource Statistics --
        public int MaxExperience { get; set; }
        public int InventoryIndex { get; set; }
        public int Weight { get; set; }
        #endregion

        #endregion
    }
}