namespace Phoenix.Common.Commands.Factory
{
	public class ClientCommand
	{
		/// <summary>
		/// The connected client's id that sent the command.
		/// </summary>
		public string Id { get; set; }

		public Command Command { get; set; }
	}
}
