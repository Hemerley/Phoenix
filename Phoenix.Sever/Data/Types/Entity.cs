namespace Phoenix.Server
{
    public class Entity
    {
        public int ID { get; set; }
        public int Type { get; set; }
        public int Rarity { get; set; }
        public string Name { get; set; }
        public int Image { get; set; }
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
        public int InstanceID { get; set; }

        public Entity()
        {

        }

        public Entity(int id, int type, string name, int image, string hisHer, string heShe, string bName, int level, int gold, int strength, int agility, int intellect, int stamina, int damage, double crit, double haste, double mastery, double versatility, int health, int mana, int taunt, int spawnTime, int spawnDelay, int vanishTime, string script, int instanceid)
        {
            this.ID = id;
            this.Type = type;
            this.Name = name;
            this.Image = image;
            this.HisHer = hisHer;
            this.HeShe = heShe;
            this.BName = bName;
            this.Level = level;
            this.Gold = gold;
            this.Strength = strength;
            this.Agility = agility;
            this.Intellect = intellect;
            this.Stamina = stamina;
            this.Damage = damage;
            this.Crit = crit;
            this.Haste = haste;
            this.Mastery = mastery;
            this.Versatility = versatility;
            this.Health = health;
            this.Mana = mana;
            this.Taunt = taunt;
            this.SpawnTime = spawnTime;
            this.SpawnDelay = spawnDelay;
            this.VanishTime = vanishTime;
            this.Script = script;
            this.InstanceID = instanceid;
        }
    }
}
