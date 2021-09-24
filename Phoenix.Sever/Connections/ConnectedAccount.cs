using Phoenix.Common.Data.Types;
namespace Phoenix.Server.Connections
{
    public class ConnectedAccount
    {
        /// <summary>
        /// Stores Connected Account Client.
        /// </summary>
        public ConnectedClient Client { get; set; }
        /// <summary>
        /// Store Account.
        /// </summary>
        public Account Account { get; set; }
    }
}
