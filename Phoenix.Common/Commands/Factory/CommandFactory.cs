using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Phoenix.Common.Commands.Factory
{
	public class CommandFactory
	{
        #region -- Parse Logic --
        public static Command ParseCommand(string rawCommand)
		{
			if (string.IsNullOrWhiteSpace(rawCommand))
				return new UnknownCommand();

			string[] commandParts = rawCommand.Split("^", StringSplitOptions.RemoveEmptyEntries);

			if (!Enum.TryParse<CommandType>(commandParts[0], out CommandType commandType))
				return new UnknownCommand();

			string[] commandDataParts = Array.Empty<string>();
			if (commandParts.Length > 1)
				commandDataParts = commandParts[1].Split("|", StringSplitOptions.RemoveEmptyEntries);
			#endregion

			switch (commandType)
			{

				#region -- Authenticate --

				case CommandType.Authenticate:
					{
						/// <summary>
						/// Validate Incoming Command is Proper Format.
						/// </summary>
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						/// <summary>
						/// Return Authenticate Command.
						/// </summary>
						return new AuthenticateRequest
						{
							Version = new Version(commandDataParts[0]),
							Username = commandDataParts[1],
							Password = commandDataParts[2]
						};
					}
				#endregion

				#region -- Authenticate Response --

				case CommandType.AuthenticateResponse:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						return new AuthenticateResponse
						{
							Success = bool.Parse(commandDataParts[0]),
							Message = commandDataParts[1]
						};
					}
				#endregion

				#region -- New Account --

				case CommandType.NewAccount:
					{
						if (commandDataParts.Length < 3)
							return new UnknownCommand();

						return new NewAccountRequest
						{
							Version = new Version(commandDataParts[0]),
							Username = commandDataParts[1],
							Password = commandDataParts[2],
							Email = commandDataParts[3]
						};
					}
				#endregion

				#region -- New Account Response --

				case CommandType.NewAccountResponse:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						return new NewAccountResponse
						{
							Success = bool.Parse(commandDataParts[0]),
							Message = commandDataParts[1]
						};
					}
				#endregion

				#region -- New Character --

				case CommandType.NewCharacter:
					{
						if (commandDataParts.Length < 4)
							return new UnknownCommand();

						return new NewCharacterCommand
						{
							CharacterName = commandDataParts[0],
							Philosophy = Int32.Parse(commandDataParts[2]),
							Gender = commandDataParts[1],
							Image = commandDataParts[3]
						};
					}
				#endregion

				#region -- New Character Response --

				case CommandType.NewCharacterResponse:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						return new NewCharacterResponse
						{
							Success = bool.Parse(commandDataParts[0]),
							Message = commandDataParts[1]
						};
					}
				#endregion

				#region -- Character List --

				case CommandType.CharacterList:
					{
						return new GetCharacterListRequest();
					}
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

						return new CharacterListResponse
						{
							Success = bool.Parse(s[0]),
							Characters = characters
						};
					}
				#endregion

				#region -- Character Login --

				case CommandType.CharacterLogin:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();
						return new CharacterConnectRequest
						{
							Name = commandDataParts[0]
						};
					}
				#endregion

				#region -- Character Login Response --

				case CommandType.CharacterLoginResponse:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						string[] s = commandParts[1].Split("~");
						string[] m = s[0].Split("|");
						Character character = null;
						if (commandDataParts.Length > 2)
						{
							string[] c = s[1].Split("|");
							character = new()
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
						}

						return new CharacterConnectResponse
						{
							Success = bool.Parse(m[0]),
							Message = m[1],
							Character = character
						};
					}
				#endregion

				#region -- Message Room --

				case CommandType.MessageRoom:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						if (commandDataParts.Length > 1)
						{
							Character character = new();
							character.Name = commandDataParts[1];

							return new MessageRoomServer
							{
								Character = character,
								Message = commandDataParts[0]
							};
						}
						else
						{
							return new MessageRoomServer
							{
								Message = commandDataParts[0]
							};
						}
					}
				#endregion

				#region -- Message Player --
				case CommandType.MessagePlayer:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						return new MessagePlayerServer
						{
							SendingName = commandDataParts[0],
							ReceivingName = commandDataParts[1],
							Message = commandDataParts[2]
						};						
					}
				#endregion

				#region -- Message Party --
				#endregion

				#region -- Message Guild --
				#endregion

				#region -- Message World --

				case CommandType.MessageWorld:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						return new MessageWorldServer
						{
							Message = commandDataParts[0],
							ID = Int32.Parse(commandDataParts[1])
						};
					}
				#endregion

				#region -- Message Broadcast --
				#endregion

				#region -- Client Connect --
				case CommandType.ClientConnect:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();
						return new ClientConnectRequest
						{
							Id = Int32.Parse(commandDataParts[0])
						};
					}
				#endregion

				#region -- Client Connect Response --

				case CommandType.ClientConnectResponse:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();
						return new ClientConnectResponse
						{
							Success = bool.Parse(commandDataParts[0]),
							Message = commandDataParts[1]
						};
					}

				#endregion

				#region -- Client Room --

				case CommandType.ClientRoom:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();
						return new ClientRoomRequest
						{
							RoomID = Int32.Parse(commandDataParts[0])
						};
					}

				#endregion

				#region -- Client Room Response --

				case CommandType.ClientRoomResponse:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						string[] s = commandParts[1].Split("~");
						string[] r = s[1].Split("|");

						Room room = new();

						room.Name = r[0];
						room.Description = r[1];
						room.Exits = r[2];
						room.Type = Int32.Parse(r[3]);

						for (int i = 2; i < s.Length; i++)
						{
							string[] c = s[i].Split("|");
							if (c[0] == "Character")
							{
								room.RoomCharacters.Add(new Character
								{
									Name = c[1],
									Image = c[2],
									Type = c[3]
								});
							}
							else if (c[0] == "Entity")
							{
								room.RoomEntities.Add(new Entity
								{
									Name = c[1],
									Image = c[2],
									Type = c[3]
								});
							}
							else if (c[0] == "Item")
							{
								room.RoomItems.Add(new Item
								{
									Name = c[1],
									Image = c[2],
									Type = c[3],
									Amount = Int32.Parse(c[4])
								});
							}
						}

						return new ClientRoomResponse
						{
							Success = bool.Parse(s[0]),
							Room = room
						};
					}

				#endregion

				#region -- Room Map --
				case CommandType.MapRequest:
					{
						if (commandParts.Length < 1)
							return new UnknownCommand();
						return new RoomMapRequest
						{
							RoomID = Int32.Parse(commandDataParts[1])
						};
					}
				#endregion

				#region -- Room Map Response --
				case CommandType.MapResponse:
					{
						if (commandParts.Length < 1)
							return new UnknownCommand();

						string[] s = commandParts[1].Split("~");
						string[] roomSpecs = s[1].Split("|");

						List<Room> rooms = new();

						for (int i = 2; i < s.Length; i++)
						{
							string[] c = s[i].Split("|");
							rooms.Add(new Room
							{
								Type = Int32.Parse(c[0]),
								CanGoNorth = c[1] == "1",
								CanGoEast = c[2] == "1",
								CanGoSouth = c[3] == "1",
								CanGoWest = c[4] == "1",
							});
						}

						return new RoomMapResponse
						{
							Success = s[0] == "1",
							RoomsWide = Int32.Parse(roomSpecs[0]),
							RoomsHigh = Int32.Parse(roomSpecs[1]),
							Rooms = rooms
						};
					}
				#endregion

				#region -- Room Player Update --
				case CommandType.RoomPlayerUpdate:
					{
						if (commandParts.Length < 1)
							return new UnknownCommand();

						Character character = new();

						character.Name = commandDataParts[1];
						character.Image = commandDataParts[2];
						character.Type = commandDataParts[3];

						return new RoomPlayerUpdate
						{
							Mode = Int32.Parse(commandDataParts[0]),
							Character = character
						};
					}
				#endregion

				#region -- Room Entity Update --
				case CommandType.RoomEntityUpdate:
					{
						if (commandParts.Length < 1)
							return new UnknownCommand();

						Entity entity = new();

						entity.Name = commandDataParts[1];
						entity.Image = commandDataParts[2];
						entity.Type = commandDataParts[3];

						return new RoomEntityUpdate
						{
							Mode = Int32.Parse(commandDataParts[0]),
							Entity = entity
						};
					}
				#endregion

				#region -- Player Move Request --
				case CommandType.PlayerMoveRequest:
					{
						if (commandDataParts.Length < 1)
							return new UnknownCommand();

						return new PlayerMoveRequest
						{
							Direction = commandDataParts[0]
						};
					}
				#endregion

				#region -- Spawn Entity --
				case CommandType.SpawnEntity:
					{
						if (commandDataParts.Length < 2)
							return new UnknownCommand();

						return new SpawnEntityServer
						{
							EntityName = commandDataParts[0],
							CharacterName = commandDataParts[1]
						};
					}
				#endregion

				#region -- No Player --
				case CommandType.NoPlayer:
					return new NoPlayerFailure();
				#endregion

				#region -- No Command --
				case CommandType.NoCommand:
					return new NoCommandFailure();
				#endregion

				#region -- Default --
				default:
					return new UnknownCommand();
					#endregion

			}
		}

		public static string FormatCommand(Command command)
		{
			Debug.WriteLine($"{(int)command.CommandType}^{string.Join("~", command.GetCommandParts().Select(x => string.Join("|", x)))}%");
			return $"{(int)command.CommandType}^{string.Join("~", command.GetCommandParts().Select(x => string.Join("|", x)))}%";
		}
	}
}
