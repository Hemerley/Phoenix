using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Request
{
    class ClientConnectCommand : Command
    {
		#region -- Properties --

		#endregion

		public ClientConnectCommand()
		{
			this.CommandType = CommandType.ClientConnect;
		}
	}
}
