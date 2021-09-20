using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using static Phoenix.Server.Program;
using Phoenix.Server.Scripts;

namespace Phoenix.Server.Functions
{
    public class RoomFunctions
    {
		#region -- Move --
		public static void MovePlayer(bool mode, int type = -1, int roomID = -1, string direction = "", ConnectedAccount account = null, string arrivalMessage = "", string playerName = "", string departureMessage = "")
		{
			if (mode)
			{
				if (ScriptEngine.Movement())
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

									foreach (Entity entity in game.rooms[account.Account.Character.RoomID].RoomEntities)
									{
										Entity newEntity = new();
										newEntity.Name = entity.DisplayName;
										newEntity.Image = entity.Image;
										newEntity.Type = entity.Type;
										clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
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
										Rooms = ToolFunctions.FindRooms(game.rooms[account.Account.Character.RoomID])
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

									foreach (Entity entity in game.rooms[account.Account.Character.RoomID].RoomEntities)
									{
										Entity newEntity = new();
										newEntity.Name = entity.DisplayName;
										newEntity.Image = entity.Image;
										newEntity.Type = entity.Type;
										clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
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
										Rooms = ToolFunctions.FindRooms(game.rooms[account.Account.Character.RoomID])
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

									foreach (Entity entity in game.rooms[account.Account.Character.RoomID].RoomEntities)
									{
										Entity newEntity = new();
										newEntity.Name = entity.DisplayName;
										newEntity.Image = entity.Image;
										newEntity.Type = entity.Type;
										clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
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
										Rooms = ToolFunctions.FindRooms(game.rooms[account.Account.Character.RoomID])
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

									foreach (Entity entity in game.rooms[account.Account.Character.RoomID].RoomEntities)
									{
										Entity newEntity = new();
										newEntity.Name = entity.DisplayName;
										newEntity.Image = entity.Image;
										newEntity.Type = entity.Type;
										clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
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
										Rooms = ToolFunctions.FindRooms(game.rooms[account.Account.Character.RoomID])
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

					foreach (Entity entity in game.rooms[roomID].RoomEntities)
					{
						Entity newEntity = new();
						newEntity.Name = entity.DisplayName;
						newEntity.Image = entity.Image;
						newEntity.Type = entity.Type;
						clientRoomResponseCommand.Room.RoomEntities.Add(newEntity);
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
						Rooms = ToolFunctions.FindRooms(game.rooms[roomID])
					});
				}
			}
		}
		#endregion
	}
}
