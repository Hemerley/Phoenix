using Phoenix.Common.Commands.Factory;
namespace Phoenix.Common.Commands.Request
{
	public class UnknownCommand : Command
	{
		public UnknownCommand()
		{
			this.CommandType = CommandType.Unknown;
		}
	}
}
