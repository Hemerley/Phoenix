using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Data;
using static Phoenix.Server.Program;
using System;
using Phoenix.Server.Logs;
using System.Collections.Generic;

namespace Phoenix.Server.Functions
{
    public class ServerFunctions
    {
        #region -- Authentication --

        public static Command Authenticate(Version version, string username, string password, ConnectedClient client, ConnectedAccount account)
        {
			if (version != new Version(Constants.GAME_VERSION))
			{
				return new AuthenticateResponse
				{
					Success = false,
					Message = "Incorrect Version Number. Please use the Patcher to run the application."
				};
			}

			if (Database.GetAccountField(Constants.GAME_MODE, "Password", "Name", username) != password)
			{
				return new AuthenticateResponse
				{
					Success = false,
					Message = "Account Name or Password doesn't match."
				};
			}

            game.connectedAccounts.Add(new ConnectedAccount
			{
				Client = client,
				Account = new Account
				{
					Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", username)),
					Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", username))
				}
			});
			game.connectedClients.Remove(client.Id.ToString());

			return new AuthenticateResponse
			{
				Success = true
			};
		}

        #endregion

        #region -- Creation --

        public static Command NewCharacter(string name, string gender, int philosophy, string image, ConnectedClient client, ConnectedAccount account)
        {
			if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", name) == null)
			{
				Database.InsertNewCharacter(Constants.GAME_MODE, name, gender, philosophy, image, account.Account.Id);
				Logger.ConsoleLog("System", $"{client.Id} has created a new character named: {name}");
				return new NewCharacterResponse
				{
					Success = true
				};
			}
			else
			{
				Logger.ConsoleLog("System", $"{client.Id} has failed to create new character named: {name}. Reason: Character Exists.");
				return new NewCharacterResponse
				{
					Success = false,
					Message = $"Failed to create new character named: {name}. Reason: Character Exists."
				};
			}
		}

		public static Command NewAccount(Version version, string username, string password, string email, ConnectedClient client, ConnectedAccount account)
		{
			if (version != new Version(Constants.GAME_VERSION))
			{
				return new AuthenticateResponse
				{
					Success = false,
					Message = "Incorrect Version Number. Please use the Patcher to run the application."
				};

			}

			if (Database.GetAccountField(Constants.GAME_MODE, "Name", "Name", username) == null)
			{
				Database.InsertNewAccount(Constants.GAME_MODE, username, password, email);
				Logger.ConsoleLog("System", $"{client.Id} has created a new account named: {username}.");

				game.connectedAccounts.Add(new ConnectedAccount
				{
					Client = client,
					Account = new Account
					{
						Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", username)),
						Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", username))
					}
				});
				game.connectedClients.Remove(client.Id);
				return new NewAccountResponse
				{
					Success = true
				};
			}
			else
			{
				Logger.ConsoleLog("System", $"{client.Id} has failed to create a new account named: {username}. Reason: Account Exists.");
				return new NewAccountResponse
				{
					Success = false,
					Message = $"Failed to create a new account named: {username}. Reason: Account Exists."
				};
			}
		}

        #endregion

        #region -- Login --

		public static Command CharacterList(ConnectedClient client, ConnectedAccount account)
        {
			List<Character> characters = Database.GetCharacterList(Constants.GAME_MODE, account.Account.Id);

			return new CharacterListResponse
			{
				Success = characters.Count > 0,
				Characters = characters
			};
		}

		public static Command CharacterConnect(string name, ConnectedClient client, ConnectedAccount account)
        {
			Character loginCharacter = Database.GetCharacter(Constants.GAME_MODE, account.Account.Id, name);

			foreach (Character character in game.connectedCharacters)
			{
				if (character.Name == loginCharacter.Name)
				{
					return new CharacterConnectResponse
					{
						Success = false,
						Message = "Character is already online!",
						Character = null
					};
				}
			}

			if (loginCharacter != null)
			{
				account.Account.Character = loginCharacter;
				game.connectedCharacters.Add(loginCharacter);

				foreach (Room room in game.rooms)
				{
					if (loginCharacter.RoomID == room.ID)
					{
						foreach (Character character in room.RoomCharacters)
						{
							foreach (ConnectedAccount cAccount in game.connectedAccounts)
							{
								if (cAccount.Account.Character.Id == character.Id)
								{
									game.SendCommandToClient(cAccount.Client, new RoomPlayerUpdate
									{
										Mode = 1,
										Character = loginCharacter
									});
									game.SendCommandToClient(cAccount.Client, new MessageWorldServer
									{
										Message = $"&tilda&g{loginCharacter.Name}&tilda&w has come &tilda&gonline&tilda&w!"
									});
								}
							}
						}
						room.RoomCharacters.Add(loginCharacter);
					}
				}
				game.totalConnections++;
				if (game.maximumPlayers < game.connectedCharacters.Count) game.maximumPlayers++;

				return new CharacterConnectResponse
				{
					Success = loginCharacter != null,
					Message = loginCharacter != null ? "\0" : "Failed to locate character, please contact a God for further assistance!",
					Character = loginCharacter
				};
			}
			return new CharacterConnectResponse
			{
				Success = loginCharacter != null,
				Message = loginCharacter != null ? "\0" : "Failed to locate character, please contact a God for further assistance!",
				Character = loginCharacter
			};
		}

		public static Command ClientConnect(int ID, ConnectedAccount account)
        {
			foreach (Character character in game.connectedCharacters)
			{
				if (character.Id == ID)
				{
					return new ClientConnectResponse
					{
						Success = true,
						Message = $"&tilda&w&tilda&gWelcome to &tilda&w{Constants.GAME_NAME}&tilda&g, &tilda&w{character.Name}&tilda&g! There are &tilda&w{game.connectedCharacters.Count}&tilda&g players online. We have had &tilda&w{game.totalConnections}&tilda&g total connections and a maximum of &tilda&w{game.maximumPlayers}&tilda&g players online this reboot. The current time is &tilda&w{DateTime.Now.ToShortTimeString()}&tilda&g.\n"
					};
				}
			}
			return new ClientConnectResponse
			{
				Success = false
			};
		}

		public static void ClientRoom(int roomID, ConnectedAccount account)
        {
			var clientRoomResponseCommand = new ClientRoomResponse();

			foreach (Room room in game.rooms)
			{
				if (room.ID == roomID)
				{
					clientRoomResponseCommand.Success = true;
					clientRoomResponseCommand.Room.Name = room.Name;
					clientRoomResponseCommand.Room.Description = room.Description;
					clientRoomResponseCommand.Room.Exits = room.Exits;
					clientRoomResponseCommand.Room.Type = room.Type;

					foreach (Character character in room.RoomCharacters)
					{
						Character newCharacter = new();
						newCharacter.Name = character.Name;
						newCharacter.Image = character.Image;
						newCharacter.Type = character.Type;
						clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
					}

					foreach (Entity entity in room.RoomEntities)
					{
						Entity newEntity = new();
						newEntity.Name = entity.Name;
						newEntity.Image = entity.Image;
						newEntity.Type = entity.Type;
						clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
					}

					foreach (Item item in room.RoomItems)
					{
						Item newItem = new();
						newItem.Name = item.Name;
						newItem.Image = item.Image;
						newItem.Type = item.Type;
						newItem.Amount = item.Amount;
					}

					game.SendCommandToClient(account.Client, clientRoomResponseCommand);
					game.SendCommandToClient(account.Client, new RoomMapResponse
					{
						Success = true,
						RoomsHigh = 5,
						RoomsWide = 5,
						Rooms = ToolFunctions.FindRooms(game.rooms[roomID])
					});
				}
			}
			if (!clientRoomResponseCommand.Success)
			{
				clientRoomResponseCommand.Success = false;
				clientRoomResponseCommand.Room = new Room();
				game.SendCommandToClient(account.Client, clientRoomResponseCommand);
			}
		}

        #endregion

        #region -- Timers --
		public static void SpawnEntity(string characterName, string entityName)
        {
			if (characterName == "\0" && entityName == "\0")
			{
				foreach (Room room in game.rooms)
				{
					foreach (Entity entity in room.Entities)
					{
						if (!game.currentEntities.Contains(entity))
						{
							game.currentEntities.Add(entity);
							room.RoomEntities.Add(entity);
							foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
							{
								if (connectedAccount.Account.Character != null)
								{
									if (connectedAccount.Account.Character.RoomID == room.ID)
									{
										game.SendCommandToClient(connectedAccount.Client, new RoomEntityUpdate
										{
											Mode = 1,
											Entity = entity
										});
										game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
										{
											Message = $"~l{entity.Name} wanders into view..."
										});
									}
								}
							}
						}
					}
				}
				ToolFunctions.AddToQueue(false, DateTimeOffset.Now.ToUnixTimeSeconds() + 120, new SpawnEntityServer(), game.serverID.ToString());
			}

		}
		#endregion

	}
}
