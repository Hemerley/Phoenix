using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Request
{
    public class ClientRoomCommand : Command
    {

		#region -- Properties --
		#endregion

		public ClientRoomCommand()
		{
			this.CommandType = CommandType.ClientRoom;
		}
	}
}
