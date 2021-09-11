using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server
{
    public class Character
    {

        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Image { get; set; }
        public string HisHer { get; set; }
        public string HeShe { get; set; }
        public int Experience { get; set; }
        public string Title { get; set; }
        public int Caste { get; set; }
        public int  Rank { get; set; }
        public int Philosophy { get; set; }
        public int Alignment { get; set; }
        public int Creation { get; set; }
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
        public int RoomID { get; set; }

        public Character(int id, int accountId, string name, int image, string hisHer, string heshe, int experience, string title, int caste, int rank, int philosophy, int alignment, int creation, int strength, int agility, int intellect, int stamina, int damage, int health, int mana, int roomID)
        {

            this.Id = id;
            this.AccountId = accountId;
            this.Name = name;
            this.Image = image;
            this.HisHer = hisHer;
            this.HeShe = heshe;
            this.Experience = experience;
            this.Title = title;
            this.Caste = caste;
            this.Rank = rank;
            this.Philosophy = philosophy;
            this.Alignment = alignment;
            this.Creation = creation;
            this.Strength = strength;
            this.Agility = agility;
            this.Intellect = intellect;
            this.Stamina = stamina;
            this.Damage = damage;
            this.Health = health;
            this.Mana = mana;
            this.RoomID = roomID;

        }
    }
}
