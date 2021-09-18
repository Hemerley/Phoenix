using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Factory
{
	public class UnknownCommand : Command
	{
		public UnknownCommand()
		{
			this.CommandType = CommandType.Unknown;
		}
	}
}
