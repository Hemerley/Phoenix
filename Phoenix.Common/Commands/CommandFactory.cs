using System;

namespace Phoenix.Common
{
	public class CommandFactory
	{
		public static Command ParseCommand(string rawCommand)
		{
			if (string.IsNullOrWhiteSpace(rawCommand))
				return new UnknownCommand();

			var commandParts = rawCommand.Split("|", StringSplitOptions.RemoveEmptyEntries);

			//TODO: Un-escape marked up pipes (&pipe;)

			if (!Enum.TryParse<CommandType>(commandParts[0], out CommandType commandType))
				return new UnknownCommand();

			switch (commandType)
			{
				case CommandType.Authenticate:
					
					/// <summary>
					/// Validate Incoming Command is Proper Format.
					/// </summary>
					if (commandParts.Length < 3)
						return new UnknownCommand();

					/// <summary>
					/// Return Authenticate Command.
					/// </summary>
					return new AuthenticateCommand
					{
						Username = commandParts[1],
						Password = commandParts[2]
					};
				case CommandType.AuthenticateResponse:
					//get the message
					if (commandParts.Length < 2)
						return new UnknownCommand();

					if (!int.TryParse(commandParts[1], out int success))
						return new UnknownCommand();

					return new AuthenticateResponseCommand
					{
						Success = success
					};
				case CommandType.MessageRoom:
					//get the message
					if (commandParts.Length < 2)
						return new UnknownCommand();

					return new MessageRoomCommand
					{
						Message = commandParts[1]
					};

				default:
					return new UnknownCommand();
			}
		}

		public static string FormatCommand(Command command)
		{
			return $"{(int)command.CommandType}|{string.Join("|", command.GetCommandParts())}";
		}
	}
}
