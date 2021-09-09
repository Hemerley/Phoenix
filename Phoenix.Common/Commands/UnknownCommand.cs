namespace Phoenix.Common
{
	public class UnknownCommand : Command
	{
		public UnknownCommand()
		{
			this.CommandType = CommandType.Unknown;
		}
	}
}
