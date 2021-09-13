namespace Phoenix.Common.Data.Types
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Image { get; set; }
        public int Type { get; set; }
        public int Slot { get; set; }
        public int Value { get; set; }
        public int Rarity { get; set; }
        public int Weight { get; set; }
        public int Damage { get; set; }
        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Intellect { get; set; }
        public int Stamina { get; set; }
        public double Crit { get; set; }
        public double Haste { get; set; }
        public double Mastery { get; set; }
        public double Versatility { get; set; }
        public int PhilosophyReq { get; set; }
        public int StrengthReq { get; set; }
        public int AgilityReq { get; set; }
        public int IntellectReq { get; set; }
        public int StaminaReq { get; set; }
        public int AlignmentReq { get; set; }
        public string Script { get; set; }

        public Item()
        {

        }

        public Item(int id, string name, int image, int type, int slot, int value, int rarity, int weight, int damage, int strength, int agility, int intellect, int stamina, double crit, double haste, double mastery, double versatility, int philosopherReq, int strengthReq, int agilityReq, int intellectReq, int staminaReq, int alignmentReq, string script)
        {
            this.Id = id;
            this.Name = name;
            this.Image = image;
            this.Type = type;
            this.Slot = slot;
            this.Value = value;
            this.Rarity = rarity;
            this.Weight = weight;
            this.Damage = damage;
            this.Strength = strength;
            this.Agility = agility;
            this.Intellect = intellect;
            this.Stamina = stamina;
            this.Crit = crit;
            this.Haste = haste;
            this.Mastery = mastery;
            this.Versatility = versatility;
            this.PhilosophyReq = philosopherReq;
            this.StrengthReq = strengthReq;
            this.AgilityReq = agilityReq;
            this.IntellectReq = intellectReq;
            this.StaminaReq = staminaReq;
            this.AlignmentReq = alignmentReq;
            this.Script = script;
        }
    }
}
