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
using Phoenix.Server.Scripts;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using static Phoenix.Server.Program;

namespace Phoenix.Server.Network
{
    class Functions
    {
		#region -- Messages --
		public static void MessageRoom(bool mode, string message, ConnectedAccount account = null, int roomID = -1)
		{
			if (mode)
			{
				foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
				{
					if (connectedAccount.Account.Character != null)
					{
						if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
						{
							game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
							{
								Character = account.Account.Character,
								Message = message
							});
						}
					}
				}
			}
			else
			{
				foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
				{
					if (connectedAccount.Account.Character != null)
					{
						if (connectedAccount.Account.Character.RoomID == roomID)
						{
							game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
							{
								Message = message
							});
						}
					}
				}
			}
		}
		public static void MessageDirect(bool mode, string message, string sending = "", string receiving = "", ConnectedAccount account = null)
		{
			if (mode)
			{
				bool foundPlayer = false;
				foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
				{
					if (connectedAccount.Account.Character != null)
					{
						if (connectedAccount.Account.Character.Name.ToLower() == receiving)
						{
							game.SendCommandToClient(connectedAccount.Client, new MessageDirectServer
							{
								SendingName = sending.FirstCharToUpper(),
								ReceivingName = receiving.FirstCharToUpper(),
								Message = message
							});
							game.SendCommandToClient(account.Client, new MessageDirectServer
							{
								SendingName = sending.FirstCharToUpper(),
								ReceivingName = receiving.FirstCharToUpper(),
								Message = message
							});
							foundPlayer = true;
						}
					}
				}
				if (!foundPlayer)
					game.SendCommandToClient(account.Client, new NoPlayerFailure());
			}
		}
		public static void MessageParty()
		{

		}
		public static void MessageGuild()
		{

		}
		public static void MessageWorld(bool mode, string message, int ID, ConnectedAccount account)
		{
			if (true)
			{
				if (account.Account.Character.Id == ID)
				{
					if (account.Account.Character.TypeID > 2)
					{
						foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
						{
							if (connectedAccount.Account.Character != null)
							{
								game.SendCommandToClient(connectedAccount.Client, new MessageWorldServer
								{
									Message = message
								});
							}
						}
					}
					else
					{
						game.SendCommandToClient(account.Client, new NoCommandFailure());
					}
				}
			}
		}
		#endregion

		#region -- Rooms --
		public static void MovePlayer(bool mode, int type = -1, int roomID = -1, string direction = "", ConnectedAccount account = null, string arrivalMessage = "", string playerName = "", string departureMessage = "")
		{

			if (mode)
			{
				if (ScriptEngine.Movement(account))
				{
					switch (direction)
					{
						case "north":
							{
								if (game.rooms[account.Account.Character.RoomID].CanGoNorth)
								{
									var clientRoomResponseCommand = new ClientRoomResponse();
									foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
									{
										if (connectedAccount.Account.Character == null)
											continue;
										if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 2,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has moved to the {direction}."
												});
											}
										}
										if (connectedAccount.Account.Character.RoomID == game.rooms[account.Account.Character.RoomID].North)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 1,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has arrived from the south."
												});
											}
										}
									}
									game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
									game.rooms[game.rooms[account.Account.Character.RoomID].North].RoomCharacters.Add(account.Account.Character);

									account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].North;

									clientRoomResponseCommand.Success = true;
									clientRoomResponseCommand.Room.Name = game.rooms[account.Account.Character.RoomID].Name;
									clientRoomResponseCommand.Room.Description = game.rooms[account.Account.Character.RoomID].Description;
									clientRoomResponseCommand.Room.Exits = game.rooms[account.Account.Character.RoomID].Exits;
									clientRoomResponseCommand.Room.Type = game.rooms[account.Account.Character.RoomID].Type;
									foreach (Character character in game.rooms[account.Account.Character.RoomID].RoomCharacters)
									{
										Character newCharacter = new();
										newCharacter.Name = character.Name;
										newCharacter.Image = character.Image;
										newCharacter.Type = character.Type;
										clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
									}

									foreach (NPC NPC in game.rooms[account.Account.Character.RoomID].RoomNPC)
									{
										NPC newNPC = new();
										newNPC.Name = NPC.DisplayName;
										newNPC.Image = NPC.Image;
										newNPC.Type = NPC.Type;
										clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
									}

									foreach (Item item in game.rooms[account.Account.Character.RoomID].RoomItems)
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
										Rooms = Functions.FindRooms(game.rooms[account.Account.Character.RoomID])
									});
								}
								else
								{
									game.SendCommandToClient(account.Client, new MessageRoomServer
									{
										Message = $"&tilda&rYou cannot move that direction."
									});
								}
								break;
							}
						case "south":
							{
								if (game.rooms[account.Account.Character.RoomID].CanGoSouth)
								{
									var clientRoomResponseCommand = new ClientRoomResponse();
									foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
									{
										if (connectedAccount.Account.Character == null)
											continue;
										if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 2,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has moved to the {direction}."
												});
											}
										}
										if (connectedAccount.Account.Character.RoomID == game.rooms[account.Account.Character.RoomID].South)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 1,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has arrived from the north."
												});
											}
										}
									}
									game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
									game.rooms[game.rooms[account.Account.Character.RoomID].South].RoomCharacters.Add(account.Account.Character);

									account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].South;

									clientRoomResponseCommand.Success = true;
									clientRoomResponseCommand.Room.Name = game.rooms[account.Account.Character.RoomID].Name;
									clientRoomResponseCommand.Room.Description = game.rooms[account.Account.Character.RoomID].Description;
									clientRoomResponseCommand.Room.Exits = game.rooms[account.Account.Character.RoomID].Exits;
									clientRoomResponseCommand.Room.Type = game.rooms[account.Account.Character.RoomID].Type;
									foreach (Character character in game.rooms[account.Account.Character.RoomID].RoomCharacters)
									{
										Character newCharacter = new();
										newCharacter.Name = character.Name;
										newCharacter.Image = character.Image;
										newCharacter.Type = character.Type;
										clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
									}

									foreach (NPC NPC in game.rooms[account.Account.Character.RoomID].RoomNPC)
									{
										NPC newNPC = new();
										newNPC.Name = NPC.DisplayName;
										newNPC.Image = NPC.Image;
										newNPC.Type = NPC.Type;
										clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
									}

									foreach (Item item in game.rooms[account.Account.Character.RoomID].RoomItems)
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
										Rooms = Functions.FindRooms(game.rooms[account.Account.Character.RoomID])
									});
								}
								else
								{
									game.SendCommandToClient(account.Client, new MessageRoomServer
									{
										Message = $"&tilda&rYou cannot move that direction."
									});
								}
								break;
							}
						case "west":
							{
								if (game.rooms[account.Account.Character.RoomID].CanGoWest)
								{
									var clientRoomResponseCommand = new ClientRoomResponse();
									foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
									{
										if (connectedAccount.Account.Character == null)
											continue;
										if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 2,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has moved to the {direction}."
												});
											}
										}
										if (connectedAccount.Account.Character.RoomID == game.rooms[account.Account.Character.RoomID].West)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 1,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has arrived from the east."
												});
											}
										}
									}
									game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
									game.rooms[game.rooms[account.Account.Character.RoomID].West].RoomCharacters.Add(account.Account.Character);

									account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].West;

									clientRoomResponseCommand.Success = true;
									clientRoomResponseCommand.Room.Name = game.rooms[account.Account.Character.RoomID].Name;
									clientRoomResponseCommand.Room.Description = game.rooms[account.Account.Character.RoomID].Description;
									clientRoomResponseCommand.Room.Exits = game.rooms[account.Account.Character.RoomID].Exits;
									clientRoomResponseCommand.Room.Type = game.rooms[account.Account.Character.RoomID].Type;
									foreach (Character character in game.rooms[account.Account.Character.RoomID].RoomCharacters)
									{
										Character newCharacter = new();
										newCharacter.Name = character.Name;
										newCharacter.Image = character.Image;
										newCharacter.Type = character.Type;
										clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
									}

									foreach (NPC NPC in game.rooms[account.Account.Character.RoomID].RoomNPC)
									{
										NPC newNPC = new();
										newNPC.Name = NPC.DisplayName;
										newNPC.Image = NPC.Image;
										newNPC.Type = NPC.Type;
										clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
									}

									foreach (Item item in game.rooms[account.Account.Character.RoomID].RoomItems)
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
										Rooms = Functions.FindRooms(game.rooms[account.Account.Character.RoomID])
									});
								}
								else
								{
									game.SendCommandToClient(account.Client, new MessageRoomServer
									{
										Message = $"&tilda&rYou cannot move that direction."
									});
								}
								break;
							}
						case "east":
							{
								if (game.rooms[account.Account.Character.RoomID].CanGoEast)
								{
									var clientRoomResponseCommand = new ClientRoomResponse();
									foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
									{
										if (connectedAccount.Account.Character == null)
											continue;
										if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 2,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has moved to the {direction}."
												});
											}
										}
										if (connectedAccount.Account.Character.RoomID == game.rooms[account.Account.Character.RoomID].East)
										{
											game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
											{
												Mode = 1,
												Character = account.Account.Character
											});
											if (connectedAccount.Client != account.Client)
											{
												game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
												{
													Message = $"&tilda&l{account.Account.Character.Name} has arrived from the west."
												});
											}
										}
									}
									game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
									game.rooms[game.rooms[account.Account.Character.RoomID].East].RoomCharacters.Add(account.Account.Character);

									account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].East;

									clientRoomResponseCommand.Success = true;
									clientRoomResponseCommand.Room.Name = game.rooms[account.Account.Character.RoomID].Name;
									clientRoomResponseCommand.Room.Description = game.rooms[account.Account.Character.RoomID].Description;
									clientRoomResponseCommand.Room.Exits = game.rooms[account.Account.Character.RoomID].Exits;
									clientRoomResponseCommand.Room.Type = game.rooms[account.Account.Character.RoomID].Type;
									foreach (Character character in game.rooms[account.Account.Character.RoomID].RoomCharacters)
									{
										Character newCharacter = new();
										newCharacter.Name = character.Name;
										newCharacter.Image = character.Image;
										newCharacter.Type = character.Type;
										clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
									}

									foreach (NPC NPC in game.rooms[account.Account.Character.RoomID].RoomNPC)
									{
										NPC newNPC = new();
										newNPC.Name = NPC.DisplayName;
										newNPC.Image = NPC.Image;
										newNPC.Type = NPC.Type;
										clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
									}

									foreach (Item item in game.rooms[account.Account.Character.RoomID].RoomItems)
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
										Rooms = Functions.FindRooms(game.rooms[account.Account.Character.RoomID])
									});
								}
								else
								{
									game.SendCommandToClient(account.Client, new MessageRoomServer
									{
										Message = $"&tilda&rYou cannot move that direction."
									});
								}
								break;
							}
						default:
							break;
					}
				}
			}
			else if (type != -1)
			{
				if (type == 1)
				{
					var clientRoomResponseCommand = new ClientRoomResponse();
					roomID = account.Account.Character.RoomID;
					ConnectedAccount targetAccount = null;

					if (account.Account.Character.TypeID < 1)
					{
						game.SendCommandToClient(account.Client, new NoCommandFailure());
						return;
					}

					foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
					{
						if (connectedAccount.Account.Character.Name.ToLower() == playerName.ToLower())
						{
							targetAccount = connectedAccount;
							break;
						}
					}

					if (targetAccount == null)
					{
						game.SendCommandToClient(account.Client, new NoPlayerFailure());
						return;
					}

					foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
					{
						if (connectedAccount.Account.Character == null)
							continue;
						if (connectedAccount.Account.Character.RoomID == targetAccount.Account.Character.RoomID)
						{
							game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
							{
								Mode = 2,
								Character = targetAccount.Account.Character
							});
							if (connectedAccount.Client != targetAccount.Client)
							{
								game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
								{
									Message = departureMessage
								});
							}
						}
						if (connectedAccount.Account.Character.RoomID == roomID)
						{
							game.SendCommandToClient(connectedAccount.Client, new RoomPlayerUpdate
							{
								Mode = 1,
								Character = targetAccount.Account.Character
							});
							if (connectedAccount.Client != targetAccount.Client)
							{
								game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
								{
									Message = arrivalMessage
								});
							}
						}
					}

					game.rooms[targetAccount.Account.Character.RoomID].RoomCharacters.Remove(targetAccount.Account.Character);
					game.rooms[roomID].RoomCharacters.Add(targetAccount.Account.Character);

					targetAccount.Account.Character.RoomID = roomID;

					clientRoomResponseCommand.Success = true;
					clientRoomResponseCommand.Room.Name = game.rooms[roomID].Name;
					clientRoomResponseCommand.Room.Description = game.rooms[roomID].Description;
					clientRoomResponseCommand.Room.Exits = game.rooms[roomID].Exits;
					clientRoomResponseCommand.Room.Type = game.rooms[roomID].Type;
					foreach (Character character in game.rooms[roomID].RoomCharacters)
					{
						Character newCharacter = new();
						newCharacter.Name = character.Name;
						newCharacter.Image = character.Image;
						newCharacter.Type = character.Type;
						clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
					}

					foreach (NPC NPC in game.rooms[roomID].RoomNPC)
					{
						NPC newNPC = new();
						newNPC.Name = NPC.DisplayName;
						newNPC.Image = NPC.Image;
						newNPC.Type = NPC.Type;
						clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
					}

					foreach (Item item in game.rooms[roomID].RoomItems)
					{
						Item newItem = new();
						newItem.Name = item.Name;
						newItem.Image = item.Image;
						newItem.Type = item.Type;
						newItem.Amount = item.Amount;
					}
					game.SendCommandToClient(targetAccount.Client, clientRoomResponseCommand);
					game.SendCommandToClient(targetAccount.Client, new MessageRoomServer
					{
						Message = $"&tilda&mYou have been summoned by &tilda&w{account.Account.Character.Name}&tilda&m!"
					});
					game.SendCommandToClient(targetAccount.Client, new RoomMapResponse
					{
						Success = true,
						RoomsHigh = 5,
						RoomsWide = 5,
						Rooms = Functions.FindRooms(game.rooms[roomID])
					});
				}
			}
		}
		#endregion

		#region -- Server --

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

		#region -- Commands --
		public static void SlashCommand(string message, ConnectedAccount account)
		{
			message = Helper.ReturnCaret(message);
			message = Helper.ReturnPipe(message);
			message = Helper.ReturnTilda(message);
			message = Helper.ReturnPercent(message);

			string[] command = message.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			switch (command[0].ToLower()[1..])
			{
				default:
					game.SendCommandToClient(account.Client, new NoCommandFailure());
					return;
			}
		}
		#endregion

		#region -- Creation --
		public static Command NewCharacter(string name, string gender, int philosophy, string image, ConnectedClient client, ConnectedAccount account)
		{
			if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", name) == null)
			{
				Database.InsertNewCharacter(Constants.GAME_MODE, name, gender, philosophy, image, account.Account.Id);
				Log.Information($"{client.Id} has created a new character named: {name}");
				return new NewCharacterResponse
				{
					Success = true
				};
			}
			else
			{
				Log.Information($"{client.Id} has failed to create new character named: {name}. Reason: Character Exists.");
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
				Log.Information($"{client.Id} has created a new account named: {username}.");

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
				Log.Information($"{client.Id} has failed to create a new account named: {username}. Reason: Account Exists.");
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

					foreach (NPC NPC in room.RoomNPC)
					{
						NPC newNPC = new();
						newNPC.Name = NPC.Name;
						newNPC.Image = NPC.Image;
						newNPC.Type = NPC.Type;
						clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
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
						Rooms = Functions.FindRooms(game.rooms[roomID])
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
		public static void SpawnNPC(string characterName, string NPCName)
		{
			if (characterName == "\0" && NPCName == "\0")
			{
				foreach (Room room in game.rooms)
				{
					foreach (NPC NPC in room.NPC)
					{
						if (!game.currentNPC.Contains(NPC))
						{
							game.currentNPC.Add(NPC);
							room.RoomNPC.Add(NPC);
							foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
							{
								if (connectedAccount.Account.Character != null)
								{
									if (connectedAccount.Account.Character.RoomID == room.ID)
									{
										game.SendCommandToClient(connectedAccount.Client, new RoomNPCUpdate
										{
											Mode = 1,
											NPC = NPC
										});
										game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
										{
											Message = $"~l{NPC.Name} wanders into view..."
										});
									}
								}
							}
						}
					}
				}
				Functions.AddToQueue(false, DateTimeOffset.Now.ToUnixTimeSeconds() + 120, new SpawnNPCServer(), game.serverID.ToString());
			}

		}
		#endregion

		#endregion

		#region -- Tools --

		/// <summary>
		/// Returns a Client ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static ConnectedClient GetClientById(string id)
		{
			if (game.connectedClients.ContainsKey(id))
				return game.connectedClients[id];

			var connectedAccount = game.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			if (connectedAccount == null)
			{
				return null;
			}
			return connectedAccount.Client;
		}

		/// <summary>
		/// Returns an Account ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static ConnectedAccount GetConnectedAccount(string id)
		{
			var connectedAccount = game.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			if (connectedAccount == null)
			{
				return null;
			}
			return connectedAccount;
		}

		/// <summary>
		/// Returns Connected Rooms for Map Draw, But Better.
		/// </summary>
		/// <param name="room"></param>
		/// <returns></returns>
		public static List<Room> FindRooms(Room room)
		{
			List<Room> roomList = new();
			int[,] grid = new int[5, 5];
			grid[2, 2] = room.ID;
			for (int x = 2; x < 5; x++)
			{
				for (int y = 2; y < 5; y++)
				{
					if (game.rooms[grid[x, y]].CanGoNorth)
					{
						grid[x - 1, y] = game.rooms[grid[x, y]].North;
						if (game.rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = game.rooms[grid[x - 1, y]].North;
					}
					if (game.rooms[grid[x, y]].CanGoSouth)
					{
						grid[x + 1, y] = game.rooms[grid[x, y]].South;
						if (game.rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = game.rooms[grid[x + 1, y]].South;

					}
					if (game.rooms[grid[x, y]].CanGoWest)
					{
						grid[x, y - 1] = game.rooms[grid[x, y]].West;
						if (game.rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = game.rooms[grid[x, y - 1]].West;

					}
					if (game.rooms[grid[x, y]].CanGoEast)
					{
						grid[x, y + 1] = game.rooms[grid[x, y]].East;
						if (game.rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = game.rooms[grid[x, y + 1]].East;
					}
				}
			}
			for (int x = 2; x > -1; x--)
			{
				for (int y = 2; y > -1; y--)
				{
					if (game.rooms[grid[x, y]].CanGoNorth)
					{
						grid[x - 1, y] = game.rooms[grid[x, y]].North;
						if (game.rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = game.rooms[grid[x - 1, y]].North;
					}
					if (game.rooms[grid[x, y]].CanGoSouth)
					{
						grid[x + 1, y] = game.rooms[grid[x, y]].South;
						if (game.rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = game.rooms[grid[x + 1, y]].South;

					}
					if (game.rooms[grid[x, y]].CanGoWest)
					{
						grid[x, y - 1] = game.rooms[grid[x, y]].West;
						if (game.rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = game.rooms[grid[x, y - 1]].West;

					}
					if (game.rooms[grid[x, y]].CanGoEast)
					{
						grid[x, y + 1] = game.rooms[grid[x, y]].East;
						if (game.rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = game.rooms[grid[x, y + 1]].East;
					}
				}
			}
			foreach (int gridLoc in grid)
			{
				roomList.Add(game.rooms[gridLoc]);
			}

			return roomList;
		}

		/// <summary>
		/// Added Queues to the Queue Library & ConcurrentQueue.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="currentTimeStamp"></param>
		/// <param name="command"></param>
		/// <param name="uid"></param>
		public static void AddToQueue(bool queue, double currentTimeStamp, Command command = null, string uid = "")
		{
			if (queue)
			{
				if (game.actionQueue.ContainsKey(currentTimeStamp))
				{
					foreach (ClientCommand clientCommand in game.actionQueue[currentTimeStamp])
					{
						game.queuedCommand.Enqueue(clientCommand);
					}
				}
			}
			else
			{
				if (game.actionQueue.ContainsKey(currentTimeStamp))
				{
					game.actionQueue[currentTimeStamp].Add(new ClientCommand
					{
						Id = uid,
						Command = command
					});
				}
				else
				{
					game.actionQueue.Add(currentTimeStamp, new List<ClientCommand>
					{
						new ClientCommand
						{
							Id = uid,
							Command = command
						}
					});
				}
			}
		}

		#endregion
	}
}
