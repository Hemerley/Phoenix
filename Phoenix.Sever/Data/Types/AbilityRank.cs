namespace Phoenix.Server
{
    public class AbilityRank
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public AbilityRank()
        {

        }

        public AbilityRank(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
