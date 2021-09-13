namespace Phoenix.Common.Data.Types
{
    public class AbilityType
    {
        public int Id  { get; set; }
        public string Name { get; set; }

        public AbilityType()
        {

        }

        public AbilityType(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
