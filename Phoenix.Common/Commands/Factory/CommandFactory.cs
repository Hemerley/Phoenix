using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Phoenix.Common.Commands.Factory
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

                #region -- Authenticate --

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

                #endregion

                #region -- Authenticate Response --

                case CommandType.AuthenticateResponse:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					return new AuthenticateResponseCommand
					{
						Success = bool.Parse(commandDataParts[0])
					};

                #endregion

                #region -- Message Room --

                case CommandType.MessageRoom:

					if (commandDataParts.Length < 2)
						return new UnknownCommand();

					return new MessageRoomCommand
					{
						Message = commandDataParts[1]
					};

                #endregion

                #region -- New Account --

                case CommandType.NewAccount:
					if (commandDataParts.Length < 3)
						return new UnknownCommand();

					return new NewAccountCommand
					{
						Username = commandDataParts[0],
						Password = commandDataParts[1],
						Email = commandDataParts[2]
					};

                #endregion

                #region -- New Account Response --

                case CommandType.NewAccountResponse:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					return new NewAccountResponseCommand
					{
						Success = bool.Parse(commandDataParts[0])
					};

                #endregion

                #region -- New Character --

                case CommandType.NewCharacter:
					if (commandDataParts.Length < 4)
						return new UnknownCommand();

					return new NewCharacterCommand
					{
						CharacterName = commandDataParts[0],
						Philosophy = Int32.Parse(commandDataParts[2]),
						Gender = commandDataParts[1],
						Image = commandDataParts[3]
					};

                #endregion

                #region -- New Character Response --

                case CommandType.NewChracterResponse:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();

					return new NewCharacterResponseCommand
					{
						Success = bool.Parse(commandDataParts[0])
					};

                #endregion

                #region -- Character List --

                case CommandType.CharacterList:
					return new GetCharacterListCommand();

                #endregion

                #region -- Character List Response --

                case CommandType.CharacterListResponse:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						string[] s = commandParts[1].Split("~");

						List<Character> characters = new();

						for (int i = 1; i < s.Length; i++)
						{
							string[] c = s[i].Split("|");
							characters.Add(new Character
							{
								Name = c[0],
								Caste = c[1],
								Philosophy = c[2]
							});
						}

						return new CharacterListResponseCommand
						{
							Success = bool.Parse(s[0]),
							Characters = characters
						};
					}
				#endregion

				#region -- Character Login --

				case CommandType.CharacterLogin:
					if (commandDataParts.Length < 1)
						return new UnknownCommand();
					return new CharacterConnectCommand
					{
						Name = commandDataParts[0]
                    };

                #endregion

                #region -- Character Login Response --

                case CommandType.CharacterLoginResponse:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						string[] s = commandParts[1].Split("~");

						Character character = new();

						string[] c = s[1].Split("|");
						int i = 0;
						// Add Index Based Increment i= 0 [i++]

						character = new Character
						{
							Id = Int32.Parse(c[0]),
							AccountId = Int32.Parse(c[1]),
							Name = c[2],
							Type = c[3],
							TypeID = Int32.Parse(c[4]),
							Image = c[5],
							Gender = c[6],
							HisHer = c[7],
							HeShe = c[8],
							Experience = Int32.Parse(c[9]),
							Title = c[10],
							Caste = c[11],
							CasteID = Int32.Parse(c[12]),
							Rank = c[13],
							RankID = Int32.Parse(c[14]),
							Philosophy = c[15],
							PhilosophyID = Int32.Parse(c[16]),
							Alignment = Int32.Parse(c[17]),
							Creation = Int32.Parse(c[18]),
							Strength = Int32.Parse(c[19]),
							Agility = Int32.Parse(c[20]),
							Intellect = Int32.Parse(c[21]),
							Stamina = Int32.Parse(c[22]),
							Damage = Int32.Parse(c[23]),
							Health = Int32.Parse(c[24]),
							Mana = Int32.Parse(c[25]),
							RoomID = Int32.Parse(c[26]),
							Crit = Int32.Parse(c[27]),
							Mastery = Int32.Parse(c[28]),
							Haste = Int32.Parse(c[29]),
							Versatility = Int32.Parse(c[30])
						};

						return new CharacterConnectResponseCommand
						{
							Success = bool.Parse(s[0]),
							Character = character
						};
					}
                #endregion

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
