namespace Phoenix.Server
{
	public class ConnectedAccount
	{

		/// <summary>
		/// Stores Connected Account Client.
		/// </summary>
		public ConnectedClient Client { get; set; }
		
		public Common.Data.Types.Account Account { get; set; }

    }
}
