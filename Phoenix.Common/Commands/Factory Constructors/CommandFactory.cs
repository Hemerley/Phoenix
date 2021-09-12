using System;
using System.Collections.Generic;

namespace Phoenix.Common
{
	public class CommandFactory
	{
		public static Command ParseCommand(string rawCommand)
		{
			if (string.IsNullOrWhiteSpace(rawCommand))
				return new UnknownCommand();

			var commandParts = rawCommand.Split("|", StringSplitOptions.RemoveEmptyEntries);

			foreach (var part in commandParts)
            {
				Helper.ReturnBar(part);
            }

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

					if (!int.TryParse(commandParts[1], out int authSuccess))
						return new UnknownCommand();

					return new AuthenticateResponseCommand
					{
						Success = authSuccess
					};
				case CommandType.MessageRoom:
					//get the message
					if (commandParts.Length < 2)
						return new UnknownCommand();

					return new MessageRoomCommand
					{
						Message = commandParts[1]
					};
				case CommandType.NewAccount:
					if (commandParts.Length < 4)
						return new UnknownCommand();

					return new NewAccountCommand
					{
						Username = commandParts[1],
						Email = commandParts[3],
						Password = commandParts[2]
					};
				case CommandType.NewAccountResponse:
					if (commandParts.Length < 2)
						return new UnknownCommand();

					if (!int.TryParse(commandParts[1], out int accountSuccess))
						return new UnknownCommand();

					return new NewAccountResponseCommand
					{
						Success = accountSuccess
					};
				case CommandType.NewCharacter:
					if (commandParts.Length < 5)
						return new UnknownCommand();

					return new NewCharacterCommand
					{
						CharacterName = commandParts[1],
						Philosophy = Int32.Parse(commandParts[3]),
						Gender = commandParts[2],
						Image = Int32.Parse(commandParts[4])
					};
				case CommandType.NewChracterResponse:
					if (commandParts.Length < 2)
						return new UnknownCommand();

					if (!int.TryParse(commandParts[1], out int characterSuccess))
						return new UnknownCommand();

					return new NewCharacterResponseCommand
					{
						Success = characterSuccess
					};

				case CommandType.CharacterList:
					return new GetCharacterListCommand();
				case CommandType.CharacterListResponse:
					if (commandParts.Length < 2)
						return new UnknownCommand();
					return new CharacterListResponseCommand
					{ 
						characters = commandParts[1]
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
