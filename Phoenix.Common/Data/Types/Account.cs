namespace Phoenix.Common.Data.Types
{
    public class Account
    {
		public int Id { get; set; }
		public int Gold { get; set; }
		public Character Character { get; set; }

		public Account()
        {

        }

		public Account(int id, int gold)
		{
			this.Id = id;
			this.Gold = gold;
		}
	}
}
