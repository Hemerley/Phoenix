using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class Character
    {

        /// <summary>
        /// Stores Character ID.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Stores Character Name.
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Stores Character Gender.
        /// </summary>
        public string gender { get; set; }

        /// <summary>
        /// Stores Character Philosophy.
        /// </summary>
        public string philosophy { get; set; }

        /// <summary>
        /// Stores Character Level.
        /// </summary>
        public int level { get; set; }

        /// <summary>
        /// Stores Character Experience.
        /// </summary>
        public int experience { get; set; }

        /// <summary>
        /// Stores Character Strength.
        /// </summary>
        public int strength { get; set; }

        /// <summary>
        /// Stores Character Agility.
        /// </summary>
        public int agility { get; set; }

        /// <summary>
        /// Stores Character Intellect.
        /// </summary>
        public int intellect { get; set; }

        /// <summary>
        /// Stores Character Stamina.
        /// </summary>
        public int stamina { get; set; }

        /// <summary>
        /// Stores Character Armor.
        /// </summary>
        public int armor { get; set; }

        /// <summary>
        /// Stores Character Damage.
        /// </summary>
        public int damage { get; set; }

        /// <summary>
        /// Stores Character Crit.
        /// </summary>
        public int crit { get; set; }

        /// <summary>
        /// Stores Character Haste.
        /// </summary>
        public int haste { get; set; }

        /// <summary>
        /// Stores Character Mastery.
        /// </summary>
        public int mastery { get; set; }

        /// <summary>
        /// Stores Character Versatility.
        /// </summary>
        public int versatility { get; set; }

        /// <summary>
        /// Stores Character Weight.
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// Stores Character Health.
        /// </summary>
        public int health { get; set; }

        /// <summary>
        /// Stores Character Mana.
        /// </summary>
        public int mana { get; set; }

        /// <summary>
        /// Stores Character Inventory.
        /// </summary>
        public string[] inventory { get; set; }

        public Character(int id, string name, string gender, string philosophy, int level, int experience, int strength, int agility, int intellect, int stamina, int armor, int damage, int crit, int haste, int mastery, int versatility, int weight, int health, int mana, string inventory)
        {

            this.id = id;
            this.name = name;
            this.gender = gender;
            this.philosophy = philosophy;
            this.level = level;
            this.experience = experience;
            this.strength = strength;
            this.agility = agility;
            this.intellect = intellect;
            this.stamina = stamina;
            this.armor = armor;
            this.crit = crit;
            this.haste = haste;
            this.mastery = mastery;
            this.versatility = versatility;
            this.weight = weight;
            this.health = health;
            this.mana = mana;
            this.inventory = inventory.Split("|");
        }
    }
}
