namespace Phoenix.Common.Data.Types
{
    public class RoomKeyTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RoomKeyTypes()
        {

        }

        public RoomKeyTypes(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
     }
}
