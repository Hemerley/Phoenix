using Phoenix.Common.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Common
{
	public class CommandFactory
	{
		public static Command ParseCommand(string rawCommand)
		{
			if (string.IsNullOrWhiteSpace(rawCommand))
				return new UnknownCommand();

			var commandParts = rawCommand.Split("^", StringSplitOptions.RemoveEmptyEntries);
	
			if (!Enum.TryParse<CommandType>(commandParts[0], out CommandType commandType))
				return new UnknownCommand();

			string[] commandDataParts = new string[] { };
			if (commandParts.Length > 1)
				commandDataParts = commandParts[1].Split("|", StringSplitOptions.RemoveEmptyEntries);

			switch (commandType)
			{
				case CommandType.Authenticate:
					
					/// <summary>
					/// Validate Incoming Command is Proper Format.
					/// </summary>
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					/// <summary>
					/// Return Authenticate Command.
					/// </summary>
					return new AuthenticateCommand
					{
						Username = commandDataParts[0],
						Password = commandDataParts[1]
					};
				case CommandType.AuthenticateResponse:
					//get the message
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					if (!int.TryParse(commandDataParts[0], out int authSuccess))
						return new UnknownCommand();

					return new AuthenticateResponseCommand
					{
						Success = authSuccess
					};
				case CommandType.MessageRoom:
					//get the message
					if (commandDataParts.Length < 2)
						return new UnknownCommand();

					return new MessageRoomCommand
					{
						Message = commandDataParts[1]
					};
				case CommandType.NewAccount:
					if (commandDataParts.Length < 2)
						return new UnknownCommand();

					return new NewAccountCommand
					{
						Username = commandDataParts[0],
						Email = commandDataParts[2],
						Password = commandDataParts[1]
					};
				case CommandType.NewAccountResponse:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					if (!int.TryParse(commandDataParts[0], out int accountSuccess))
						return new UnknownCommand();

					return new NewAccountResponseCommand
					{
						Success = accountSuccess
					};
				case CommandType.NewCharacter:
					if (commandDataParts.Length < 2)
						return new UnknownCommand();

					return new NewCharacterCommand
					{
						CharacterName = commandDataParts[0],
						Philosophy = Int32.Parse(commandDataParts[2]),
						Gender = commandDataParts[1],
						Image = Int32.Parse(commandDataParts[3])
					};
				case CommandType.NewChracterResponse:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					if (!int.TryParse(commandDataParts[0], out int characterSuccess))
						return new UnknownCommand();

					return new NewCharacterResponseCommand
					{
						Success = characterSuccess
					};

				case CommandType.CharacterList:
					return new GetCharacterListCommand();
				case CommandType.CharacterListResponse:
					if (commandDataParts.Length < 0)
						return new UnknownCommand();

					string[] s = commandParts[1].Split("~");

					List<Character> characters = new();

					foreach (string cString in s)
                    {
						string[] c = cString.Split("|");
						characters.Add(new Character
						{
							Name = c[0],
							Caste = c[1],
							Philosophy = c[2]
						});
                    }

					return new CharacterListResponseCommand
					{ 

						Characters = characters
					};
				case CommandType.CharacterLogin:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();
					return new CharacterConnectCommand
					{
						Name = commandDataParts[0]
                    };
				case CommandType.CharacterLoginResponse:
					if (commandDataParts.Length < 1)
					return new UnknownCommand();

					var character =  new Character
					{
						Id = Int32.Parse(commandDataParts[0]),
						AccountId = Int32.Parse(commandDataParts[1]),
						Name = commandDataParts[2],
						Type = Int32.Parse(commandDataParts[3]),
						Image = Int32.Parse(commandDataParts[4]),
						Gender = commandDataParts[5],
						HisHer = commandDataParts[6],
						HeShe = commandDataParts[7],
						Experience = Int32.Parse(commandDataParts[8]),
						Title = commandDataParts[9],
						Caste = commandDataParts[10],
						Rank = Int32.Parse(commandDataParts[11]),
						Philosophy = commandDataParts[12],
						Alignment = Int32.Parse(commandDataParts[13]),
						Creation = Int32.Parse(commandDataParts[14]),
						Strength = Int32.Parse(commandDataParts[15]),
						Agility = Int32.Parse(commandDataParts[16]),
						Intellect = Int32.Parse(commandDataParts[17]),
						Stamina = Int32.Parse(commandDataParts[18]),
						Damage = Int32.Parse(commandDataParts[19]),
						Health = Int32.Parse(commandDataParts[20]),
						Mana = Int32.Parse(commandDataParts[21]),
						RoomID = Int32.Parse(commandDataParts[22]),
						Crit = Int32.Parse(commandDataParts[23]),
						Mastery = Int32.Parse(commandDataParts[24]),
						Haste = Int32.Parse(commandDataParts[25]),
						Versatility = Int32.Parse(commandDataParts[26])
					};

					return new CharacterConnectResponseCommand
					{
						// Structure Character
						Success = Int32.Parse(commandDataParts[0]),
						Character = character
					};
				default:
					return new UnknownCommand();
			}
		}

		public static string FormatCommand(Command command)
		{
			return $"{(int)command.CommandType}^{string.Join("~", command.GetCommandParts().Select(x => string.Join("|", x)))}";
		}
	}
}
