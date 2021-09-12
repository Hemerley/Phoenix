namespace Phoenix.Server
{
    public class Philosophy
    {
        public int Id  { get; set; }
        public string Name { get; set; }

        public Philosophy()
        {

        }

        public Philosophy(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }
}
