using System;

namespace Phoenix.Common.Data.Types
{
    public class NPC
    {

        #region -- Database Fields --
        public int ID { get; set; }
        public int TypeID { get; set; }
        public int RarityID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string HisHer { get; set; }
        public string HeShe { get; set; }
        public string BName { get; set; }
        public int Level { get; set; }
        public int Gold { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intellect { get; set; }
        public int Stamina { get; set; }
        public int Damage { get; set; }
        public double Crit { get; set; }
        public double Haste { get; set; }
        public double Mastery { get; set; }
        public double Versatility { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Taunt { get; set; }
        public int SpawnTime { get; set; }
        public int SpawnDelay { get; set; }
        public int VanishTime { get; set; }
        public string Script { get; set; }
        #endregion

        #region -- Server Fields --

        #region -- General Statistics --
        public string DisplayName { get; set; }
        public Guid InstanceID { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public int RoomID { get; set; }
        #endregion

        #region -- Combat Statistics --
        public double AttackSpeed { get; set; }
        public bool IsAttacking { get; set; }
        public int TargetID { get; set; }
        public int Threat { get; set; }
        #endregion

        #region -- Resource Statistics --
        public int CurrentHealth { get; set; }
        public int CurrentMana { get; set; }
        #endregion

        #endregion

    }
}
