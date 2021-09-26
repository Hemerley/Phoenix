using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
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
using System.IO;
using System.Linq;
using static Phoenix.Server.Program;

namespace Phoenix.Server.Network
{
    class Functions
    {
        #region -- Messages --
        public static void MessageRoom(string message, int roomID)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);
            foreach (ConnectedAccount account in accounts)
            {
                game.SendCommandToClient(account.Client, new MessageRoomServer
                {
                    Message = message
                });
            }
        }
        public static void MessageRoom(string message, int roomID, Character character)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);
            foreach (ConnectedAccount account in accounts)
            {
                game.SendCommandToClient(account.Client, new MessageRoomServer
                {
                    Message = message,
                    Character = character
                });
            }
        }
        public static void MessageRoom(string message, int roomID, ConnectedAccount account)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);
            foreach (ConnectedAccount accountx in accounts)
            {
                if (accountx != account)
                    game.SendCommandToClient(accountx.Client, new MessageRoomServer
                    {
                        Message = message
                    });
            }
        }
        public static void MessageRoom(string message, int roomID, ConnectedAccount account, ConnectedAccount sAccount)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);
            foreach (ConnectedAccount accountx in accounts)
            {
                if (accountx != account && accountx != sAccount)
                    game.SendCommandToClient(accountx.Client, new MessageRoomServer
                    {
                        Message = message
                    });
            }
        }
        public static void MessageDirect(string message, string sending, string receiving, ConnectedAccount account)
        {
            var character = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == receiving);

            if (character != null)
            {
                game.SendCommandToClient(character.Client, new MessageDirectServer
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
            }
            else
            {
                game.SendCommandToClient(account.Client, new NoPlayerFailure());

            }
        }
        public static void MessageDirect(string message, string id)
        {

            game.SendCommandToClient(game.connectedAccounts[id].Client, new MessageDirectServer
            {
                SendingName = "1",
                ReceivingName = "1",
                Message = message
            });

        }
        public static void MessageParty()
        {

        }
        public static void MessageGuild()
        {

        }
        public static void MessageWorld(string message, string ID, ConnectedAccount account)
        {
            var playerAccount = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Id.ToString() == ID);
            if (playerAccount != null)
            {
                if (playerAccount.Account.Character.TypeID > 2)
                {
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts.Values)
                    {
                        if (connectedAccount.Account.Character == null)
                        {
                            continue;
                        }
                        game.SendCommandToClient(connectedAccount.Client, new MessageWorldServer
                        {
                            Message = message
                        });
                    }
                }
                else
                {
                    game.SendCommandToClient(account.Client, new NoCommandFailure());
                }
            }
            else
            {
                game.SendCommandToClient(account.Client, new NoPlayerFailure());
            }
        }
        public static void MessageWorld(string message)
        {
            foreach (ConnectedAccount connectedAccount in game.connectedAccounts.Values)
            {
                if (connectedAccount.Account.Character == null)
                {
                    continue;
                }
                game.SendCommandToClient(connectedAccount.Client, new MessageWorldServer
                {
                    Message = message
                });
            }
        }
        #endregion

        #region -- Rooms --
        public static void MovePlayer(string direction, ConnectedAccount account)
        {
            if (ScriptEngine.ReturnScript("movement", account.Client.Id, true))
            {
                switch (direction)
                {
                    case "north":
                        {
                            if (game.rooms[account.Account.Character.RoomID].CanGoNorth)
                            {

                                MessageRoom($"&tilda&l{account.Account.Character.Name} has moved to the {direction}.", account.Account.Character.RoomID, account);
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has arrived from the south.", game.rooms[account.Account.Character.RoomID].North, account);
                                CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
                                CharacterUpdate(1, game.rooms[account.Account.Character.RoomID].North, account.Account.Character);
                                game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
                                game.rooms[game.rooms[account.Account.Character.RoomID].North].RoomCharacters.Add(account.Account.Character);

                                account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].North;
                                account.Account.Character.IsAttacking = false;
                                account.Account.Character.TargetID = "";

                                #region -- Hostile Mob Loop --
                                {
                                    var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                                    foreach (NPC npc in npcs)
                                    {
                                        var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                                        if (characters != null)
                                        {
                                            npc.IsAttacking = true;
                                            npc.TargetID = characters[0].Name.ToLower();
                                            npc.TargetIsPlayer = true;
                                        }
                                    }
                                }
                                #endregion

                                MovementUpdate(account);
                            }
                            else
                            {
                                MessageDirect($"&tilda&rYou cannot move that direction.", account.Client.Id);
                            }
                            break;
                        }
                    case "south":
                        {
                            if (game.rooms[account.Account.Character.RoomID].CanGoSouth)
                            {
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has moved to the {direction}.", account.Account.Character.RoomID, account);
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has arrived from the north.", game.rooms[account.Account.Character.RoomID].South, account); CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
                                CharacterUpdate(1, game.rooms[account.Account.Character.RoomID].South, account.Account.Character);
                                game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
                                game.rooms[game.rooms[account.Account.Character.RoomID].South].RoomCharacters.Add(account.Account.Character);

                                account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].South;
                                account.Account.Character.IsAttacking = false;
                                account.Account.Character.TargetID = "";

                                #region -- Hostile Mob Loop --
                                {
                                    var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                                    foreach (NPC npc in npcs)
                                    {
                                        var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                                        if (characters != null)
                                        {
                                            npc.IsAttacking = true;
                                            npc.TargetID = characters[0].Name.ToLower();
                                            npc.TargetIsPlayer = true;
                                        }
                                    }
                                }
                                #endregion

                                MovementUpdate(account);
                            }
                            else
                            {
                                MessageDirect($"&tilda&rYou cannot move that direction.", account.Client.Id);
                            }
                            break;
                        }
                    case "west":
                        {
                            if (game.rooms[account.Account.Character.RoomID].CanGoWest)
                            {
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has moved to the {direction}.", account.Account.Character.RoomID, account);
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has arrived from the east.", game.rooms[account.Account.Character.RoomID].West, account); CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
                                CharacterUpdate(1, game.rooms[account.Account.Character.RoomID].West, account.Account.Character);
                                game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
                                game.rooms[game.rooms[account.Account.Character.RoomID].West].RoomCharacters.Add(account.Account.Character);

                                account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].West;
                                account.Account.Character.IsAttacking = false;
                                account.Account.Character.TargetID = "";

                                #region -- Hostile Mob Loop --
                                {
                                    var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                                    foreach (NPC npc in npcs)
                                    {
                                        var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                                        if (characters != null)
                                        {
                                            npc.IsAttacking = true;
                                            npc.TargetID = characters[0].Name.ToLower();
                                            npc.TargetIsPlayer = true;
                                        }
                                    }
                                }
                                #endregion

                                MovementUpdate(account);
                            }
                            else
                            {
                                MessageDirect($"&tilda&rYou cannot move that direction.", account.Client.Id);
                            }
                            break;
                        }
                    case "east":
                        {
                            if (game.rooms[account.Account.Character.RoomID].CanGoEast)
                            {
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has moved to the {direction}.", account.Account.Character.RoomID, account);
                                MessageRoom($"&tilda&l{account.Account.Character.Name} has arrived from the west.", game.rooms[account.Account.Character.RoomID].East, account); CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
                                CharacterUpdate(1, game.rooms[account.Account.Character.RoomID].East, account.Account.Character);
                                game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
                                game.rooms[game.rooms[account.Account.Character.RoomID].East].RoomCharacters.Add(account.Account.Character);

                                account.Account.Character.RoomID = game.rooms[account.Account.Character.RoomID].East;
                                account.Account.Character.IsAttacking = false;
                                account.Account.Character.TargetID = "";

                                #region -- Hostile Mob Loop --
                                {
                                    var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                                    foreach (NPC npc in npcs)
                                    {
                                        var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                                        if (characters != null)
                                        {
                                            npc.IsAttacking = true;
                                            npc.TargetID = characters[0].Name.ToLower();
                                            npc.TargetIsPlayer = true;
                                        }
                                    }
                                }
                                #endregion

                                MovementUpdate(account);
                            }
                            else
                            {
                                MessageDirect($"&tilda&rYou cannot move that direction.", account.Client.Id);
                            }
                            break;
                        }
                    default:
                        MessageDirect($"&tilda&rYou cannot move that direction.", account.Client.Id);
                        break;
                }
            }
        }
        public static void MovePlayer(int roomID, ConnectedAccount account, string arrivalMessage, string departureMessage, ConnectedAccount ignoreAccount)
        {
            CharacterUpdate(1, roomID, account.Account.Character);
            MessageRoom(arrivalMessage, roomID, ignoreAccount);
            CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
            MessageRoom(departureMessage, account.Account.Character.RoomID, ignoreAccount);
            game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
            game.rooms[roomID].RoomCharacters.Add(account.Account.Character);

            account.Account.Character.RoomID = roomID;
            account.Account.Character.IsAttacking = false;
            account.Account.Character.TargetID = "";

            #region -- Hostile Mob Loop --
            {
                var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                foreach (NPC npc in npcs)
                {
                    var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                    if (characters != null)
                    {
                        npc.IsAttacking = true;
                        npc.TargetID = characters[0].Name.ToLower();
                        npc.TargetIsPlayer = true;
                    }
                }
            }
            #endregion

            MovementUpdate(account);
        }
        public static void MovePlayer(int roomID, ConnectedAccount account, string arrivalMessage, string departureMessage, ConnectedAccount ignoreAccount, string directMessage)
        {

            CharacterUpdate(2, account.Account.Character.RoomID, account.Account.Character);
            MessageRoom(departureMessage, account.Account.Character.RoomID, ignoreAccount);
            CharacterUpdate(1, roomID, account.Account.Character);
            MessageRoom(arrivalMessage, roomID);
            game.rooms[account.Account.Character.RoomID].RoomCharacters.Remove(account.Account.Character);
            game.rooms[roomID].RoomCharacters.Add(account.Account.Character);

            account.Account.Character.RoomID = roomID;
            account.Account.Character.IsAttacking = false;
            account.Account.Character.TargetID = "";

            #region -- Hostile Mob Loop --
            {
                var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                foreach (NPC npc in npcs)
                {
                    var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                    if (characters != null)
                    {
                        npc.IsAttacking = true;
                        npc.TargetID = characters[0].Name.ToLower();
                        npc.TargetIsPlayer = true;
                    }
                }
            }
            #endregion

            MovementUpdate(account);
            Functions.MessageDirect(directMessage, account.Client.Id);
        }
        public static void MovePlayer(int type, ConnectedAccount account, string playerName, string arrivalMessage, string departureMessage)
        {

            if (type == 1)
            {
                int roomID = account.Account.Character.RoomID;
                ConnectedAccount targetAccount = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == playerName.ToLower());

                if (account.Account.Character.TypeID < 1)
                {
                    game.SendCommandToClient(account.Client, new NoCommandFailure());
                    return;
                }

                if (targetAccount == null)
                {
                    game.SendCommandToClient(account.Client, new NoPlayerFailure());
                    return;
                }

                CharacterUpdate(2, targetAccount.Account.Character.RoomID, targetAccount.Account.Character);
                MessageRoom(departureMessage, targetAccount.Account.Character.RoomID, targetAccount);
                CharacterUpdate(1, roomID, targetAccount.Account.Character);
                MessageRoom(arrivalMessage, roomID);
                game.rooms[targetAccount.Account.Character.RoomID].RoomCharacters.Remove(targetAccount.Account.Character);
                game.rooms[roomID].RoomCharacters.Add(targetAccount.Account.Character);
                targetAccount.Account.Character.RoomID = roomID;
                targetAccount.Account.Character.IsAttacking = false;
                targetAccount.Account.Character.TargetID = "";

                #region -- Hostile Mob Loop --
                {
                    var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                    foreach (NPC npc in npcs)
                    {
                        var characters = game.rooms[account.Account.Character.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                        if (characters != null)
                        {
                            npc.IsAttacking = true;
                            npc.TargetID = characters[0].Name.ToLower();
                            npc.TargetIsPlayer = true;
                        }
                    }
                }
                #endregion

                MovementUpdate(targetAccount);
                MessageDirect($"~mYou have be summoned by ~w{account.Account.Character.Name.FirstCharToUpper()}~m!", targetAccount.Client.Id);
            }
        }
        #endregion

        #region -- Server --

        #region -- Authentication --
        public static Command Authenticate(Version version, string username, string password, ConnectedClient client)
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

            game.connectedAccounts.Add(client.Id, new ConnectedAccount
            {
                Client = client,
                Account = new Account
                {
                    Id = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "ID", "Name", username)),
                    Gold = Int32.Parse(Database.GetAccountField(Constants.GAME_MODE, "Gold", "Name", username)),
                    Character = new Character()
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
            if (account.Account.Character.IsDead)
            {
                Functions.MessageDirect("~rYou are currently dead and cannot do that!", account.Client.Id);
                return;
            }
            message = Helper.ReturnCaret(message);
            message = Helper.ReturnPipe(message);
            message = Helper.ReturnTilda(message);
            message = Helper.ReturnPercent(message);
            string[] command = message.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (command[0].ToLower()[1..] == "scriptglobalreload")
            {
                if (account.Account.Character.TypeID < 2)
                {
                    Functions.MessageDirect("~cNo command found!", account.Client.Id);
                    return;
                }

                game.scripts.Clear();
                string[] files = Directory.GetFiles("./Scripts/", "*.*", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    game.scripts.Add(file.ToLower());
                }
                Functions.MessageDirect("~qAll Scripts Reloaded!", account.Client.Id);
                return;
            }
            else if (command[0].ToLower()[1..] == "who")
            {
                var characterNames = game.connectedAccounts.Values.Where(x => x.Account.Character.Name != "").Select(x => x.Account.Character.Name);

                Functions.MessageDirect($"~gThere are currrently ~w{characterNames.Count()} ~gplayers online: ~w{string.Join(", ", characterNames)}~g.", account.Client.Id);
                return;
            }
            else if (command[0].ToLower()[1..] == "autoattack")
            {
                account.Account.Character.AutoAttack = !account.Account.Character.AutoAttack;
                Functions.MessageDirect($"~wAutoAttack~c has been set to: ~w{account.Account.Character.AutoAttack}~c.", account.Client.Id);
                return;
            }
            else if (command[0].ToLower()[1..] == "autoloot")
            {
                account.Account.Character.AutoLoot = !account.Account.Character.AutoLoot;
                Functions.MessageDirect($"~wAutoLoot~c has been set to: ~w{account.Account.Character.AutoLoot}~c.", account.Client.Id);
                return;
            }

            foreach (string script in game.scripts)
            {
                string[] x = script.Split("/");
                string[] z = x[^1].Split("\\");
                string scriptNamez = z[^1][0..^4].ToLower();

                if (scriptNamez == command[0].ToLower()[1..])
                {
                    if (command.Length < 1)
                    {
                        ScriptEngine.RunScript(command[0].ToLower()[1..], account.Client.Id, true);
                        return;
                    }
                    else
                    {
                        string defenderID = "";
                        string scriptName = command[0].ToLower()[1..];
                        command = command.Skip(1).ToArray();
                        string name = string.Join(" ", command).ToLower();
                        if (game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == name) != null)
                            defenderID = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == name).Client.Id;
                        if (defenderID == "")
                        {
                            if (game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == name && x.RoomID == account.Account.Character.RoomID) != null)
                                defenderID = game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == name && x.RoomID == account.Account.Character.RoomID).InstanceID.ToString();
                        }
                        else
                        {
                            ScriptEngine.RunScript(scriptName, account.Client.Id, true, defenderID, true);
                            return;
                        }
                        if (defenderID == "")
                        {
                            Functions.MessageDirect($"~c{name} isn't here!", account.Client.Id);
                            if (command[0].ToLower()[1..] == "attack")
                            {
                                account.Account.Character.IsAttacking = false;
                                account.Account.Character.TargetID = "";
                            }
                            return;
                        }
                        else
                        {
                            ScriptEngine.RunScript(scriptName, account.Client.Id, true, defenderID, false);
                            return;
                        }
                    }
                }
            }
            game.SendCommandToClient(account.Client, new NoCommandFailure());
        }
        #endregion

        #region -- Creation --
        public static Command NewCharacter(string name, string gender, int philosophy, string image, ConnectedAccount account)
        {
            if (Database.GetCharacterField(Constants.GAME_MODE, "Name", "Name", name) == null)
            {
                Database.InsertNewCharacter(Constants.GAME_MODE, name, gender, philosophy, image, account.Account.Id);
                Log.Information($"{account.Client.Id} has created a new character named: {name}");
                return new NewCharacterResponse
                {
                    Success = true
                };
            }
            else
            {
                Log.Information($"{account.Client.Id} has failed to create new character named: {name}. Reason: Character Exists.");
                return new NewCharacterResponse
                {
                    Success = false,
                    Message = $"Failed to create new character named: {name}. Reason: Character Exists."
                };
            }
        }
        public static Command NewAccount(Version version, string username, string password, string email, ConnectedClient client)
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

                game.connectedAccounts.Add(client.Id, new ConnectedAccount
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
        public static Command CharacterList(ConnectedAccount account)
        {
            List<Character> characters = Database.GetCharacterList(Constants.GAME_MODE, account.Account.Id);

            return new CharacterListResponse
            {
                Success = characters.Count > 0,
                Characters = characters
            };
        }
        public static Command CharacterConnect(string name, ConnectedAccount account)
        {
            Character loginCharacter = Database.GetCharacter(Constants.GAME_MODE, account.Account.Id, name);
            ConnectedAccount playerAccount = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == loginCharacter.Name.ToLower());

            if (playerAccount != null)
            {
                return new CharacterConnectResponse
                {
                    Success = false,
                    Message = "Character is already online!",
                    Character = null
                };
            }

            if (loginCharacter != null)
            {
                var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == loginCharacter.RoomID);

                foreach (ConnectedAccount cAccount in accounts)
                {
                    game.SendCommandToClient(cAccount.Client, new RoomCharacterUpdate
                    {
                        Mode = 1,
                        Character = loginCharacter
                    });
                }


                MessageWorld($"&tilda&g{loginCharacter.Name}&tilda&w has come &tilda&gonline&tilda&w!");
                account.Account.Character = loginCharacter;
                game.rooms[loginCharacter.RoomID].RoomCharacters.Add(loginCharacter);
                game.totalConnections++;
                if (game.maximumPlayers < game.connectedAccounts.Values.Where(x => x.Account.Character.Name != "").ToList().Count) game.maximumPlayers++;

                return new CharacterConnectResponse
                {
                    Success = true,
                    Message = "\0",
                    Character = loginCharacter
                };
            }
            else
            {
                return new CharacterConnectResponse
                {
                    Success = loginCharacter != null,
                    Message = loginCharacter != null ? "\0" : "Failed to locate character, please contact a God for further assistance!",
                    Character = loginCharacter
                };
            }
        }
        public static Command ClientConnect(int ID)
        {
            var character = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Id == ID);
            if (character != null)
            {
                return new ClientConnectResponse
                {
                    Success = true,
                    Message = $"&tilda&w&tilda&gWelcome to &tilda&w{Constants.GAME_NAME}&tilda&g, &tilda&w{character.Account.Character.Name}&tilda&g! There are &tilda&w{game.connectedAccounts.Values.Where(x => x.Account.Character.Name != "").ToList().Count}&tilda&g players online. We have had &tilda&w{game.totalConnections}&tilda&g total connections and a maximum of &tilda&w{game.maximumPlayers}&tilda&g players online this reboot. The current time is &tilda&w{DateTime.Now.ToShortTimeString()}&tilda&g.\n"
                };
            }
            return new ClientConnectResponse
            {
                Success = false
            };
        }
        #endregion

        #region -- Timers --
        public static void MinuteTimer()
        {
            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 60000, new MinuteTimerServer(), game.serverID.ToString());
        }
        public static void RespawnCharacter(int roomID, ConnectedAccount account, string arrivalMessage, string departureMessage)
        {
            account.Account.Character.CurrentHealth = Convert.ToInt32(account.Account.Character.Health * .50);
            account.Account.Character.HealthRegen = true;
            account.Account.Character.IsDead = false;
            MovePlayer(roomID, account, arrivalMessage, departureMessage, account, "~gYou slowly feel your heart begin to thunder as you return back to life!");
            Functions.CharacterStatUpdate(account);
        }
        public static void SecondTimer()
        {
            #region -- Attack Loop --
            {
                var characters = game.connectedAccounts.Values.Where(x => x.Account.Character.IsAttacking == true);
                var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == true);
                var defenderID = "";
                foreach (ConnectedAccount connectedAccount in characters)
                {
                    if (connectedAccount.Account.Character.TargetIsPlayer)
                    {
                        if (game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == connectedAccount.Account.Character.TargetID) != null)
                        {
                            defenderID = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == connectedAccount.Account.Character.TargetID).Client.Id;
                            ScriptEngine.RunScript("attack", connectedAccount.Client.Id, true, defenderID, true);
                            defenderID = "";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == connectedAccount.Account.Character.TargetID) != null)
                        {
                            defenderID = game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == connectedAccount.Account.Character.TargetID).InstanceID.ToString();
                            ScriptEngine.RunScript("attack", connectedAccount.Client.Id, true, defenderID, false);
                            defenderID = "";
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                foreach (NPC npc in npcs)
                {
                    if (npc.TargetIsPlayer)
                    {
                        if (game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == npc.TargetID) != null)
                        {
                            defenderID = game.connectedAccounts.Values.FirstOrDefault(x => x.Account.Character.Name.ToLower() == npc.TargetID).Client.Id;
                            ScriptEngine.RunScript("attack", npc.InstanceID.ToString(), false, defenderID, true);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == npc.TargetID) != null)
                        {
                            defenderID = game.currentNPC.Values.FirstOrDefault(x => x.Name.ToLower() == npc.TargetID).InstanceID.ToString();
                            ScriptEngine.RunScript("attack", npc.InstanceID.ToString(), false, defenderID, false);
                            continue;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

            }
            #endregion

            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 1000, new SecondTimerServer(), game.serverID.ToString());
        }
        public static void SpawnNPC(string characterName, string NPCName)
        {
            if (characterName == "\0" && NPCName == "\0")
            {
                foreach (Room room in game.rooms)
                {
                    foreach (NPC NPC in room.NPC)
                    {
                        if (!game.currentNPC.ContainsKey(NPC.InstanceID.ToString()))
                        {
                            NPC.RoomID = room.ID;
                            game.currentNPC.Add(NPC.InstanceID.ToString(), NPC);
                            room.RoomNPC.Add(NPC);
                            NPCUpdate(1, room.ID, NPC);
                            MessageRoom($"~l{NPC.Name} wanders into view...", room.ID);
                            #region -- Hostile Mob Loop --
                            {
                                var npcs = game.currentNPC.Values.Where(x => x.IsAttacking == false && x.TypeID == 3);
                                foreach (NPC npc in npcs)
                                {
                                    var characters = game.rooms[NPC.RoomID].RoomCharacters.OrderBy(x => x.RankID).ToList();
                                    if (characters != null)
                                    {
                                        npc.IsAttacking = true;
                                        npc.TargetID = characters[0].Name.ToLower();
                                        npc.TargetIsPlayer = true;
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                }
                Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 120000, new SpawnNPCServer(), game.serverID.ToString());
            }
        }
        public static void TickTimer()
        {
            var characters = game.connectedAccounts.Values.Where(x => x.Account.Character.HealthRegen == true && x.Account.Character.Name != "");
            foreach (ConnectedAccount character in characters)
            {
                character.Account.Character.CurrentHealth += 2;
                if (character.Account.Character.CurrentHealth > character.Account.Character.Health)
                {
                    character.Account.Character.CurrentHealth = character.Account.Character.Health;
                    character.Account.Character.HealthRegen = false;
                    Functions.CharacterStatUpdate(character);
                }

            }
            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 250, new TickTimerServer(), game.serverID.ToString());
        }
        #endregion

        #region -- Updates --
        public static void MapUpdate(ConnectedAccount account)
        {
            game.SendCommandToClient(account.Client, new RoomMapResponse
            {
                Success = true,
                RoomsHigh = 5,
                RoomsWide = 5,
                Rooms = Functions.FindRooms(game.rooms[account.Account.Character.RoomID])
            });
        }
        public static void MovementUpdate(ConnectedAccount account)
        {
            var clientRoomResponseCommand = new ClientRoomResponse
            {
                Success = true
            };
            clientRoomResponseCommand.Room.Name = game.rooms[account.Account.Character.RoomID].Name;
            clientRoomResponseCommand.Room.Description = game.rooms[account.Account.Character.RoomID].Description;
            clientRoomResponseCommand.Room.Exits = game.rooms[account.Account.Character.RoomID].Exits + " ";
            clientRoomResponseCommand.Room.Type = game.rooms[account.Account.Character.RoomID].Type;
            foreach (Character character in game.rooms[account.Account.Character.RoomID].RoomCharacters)
            {
                Character newCharacter = new();
                newCharacter.Name = character.Name;
                newCharacter.Image = character.Image;
                newCharacter.Type = character.Type;
                if (character.Name.ToLower() != account.Account.Character.Name.ToLower())
                    clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits + "&tilda&g" + character.Name + "&tilda&w, ";
                clientRoomResponseCommand.Room.RoomCharacters.Add(newCharacter);
            }

            foreach (NPC NPC in game.rooms[account.Account.Character.RoomID].RoomNPC)
            {
                NPC newNPC = new();
                newNPC.Name = NPC.DisplayName;
                newNPC.Image = NPC.Image;
                newNPC.Type = NPC.Type;
                clientRoomResponseCommand.Room.RoomNPC.Add(newNPC);
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits + "&tilda&p" + NPC.BName + " " + NPC.Name + "&tilda&w, ";
            }
            int roomCount = clientRoomResponseCommand.Room.RoomCharacters.Count + clientRoomResponseCommand.Room.RoomNPC.Count;
            if (roomCount > 1 && roomCount < 3)
            {
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits.Remove(clientRoomResponseCommand.Room.Exits.Length - 2, 2) + " is standing here with you. ";
            }
            else if (roomCount > 2)
            {
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits.Remove(clientRoomResponseCommand.Room.Exits.Length - 2, 2) + " are standing here with you. ";
            }


            foreach (Item item in game.rooms[account.Account.Character.RoomID].RoomItems)
            {
                Item newItem = new();
                newItem.Name = item.Name;
                newItem.Image = item.Image;
                newItem.Type = item.Type;
                newItem.Amount = item.Amount;
                clientRoomResponseCommand.Room.RoomItems.Add(newItem);
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits + "&tilda&c" + item.Name + "&tilda&w, ";
            }
            if (clientRoomResponseCommand.Room.RoomItems.Count == 1)
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits.Remove(clientRoomResponseCommand.Room.Exits.Length - 2, 2) + " is laying on the ground here.";
            else if (clientRoomResponseCommand.Room.RoomItems.Count > 1)
                clientRoomResponseCommand.Room.Exits = clientRoomResponseCommand.Room.Exits.Remove(clientRoomResponseCommand.Room.Exits.Length - 2, 2) + " are laying on the ground here.";
            game.SendCommandToClient(account.Client, clientRoomResponseCommand);
            MapUpdate(account);
        }
        public static void NPCUpdate(int mode, int roomID, NPC npc)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);

            foreach (ConnectedAccount connectedAccount in accounts)
            {
                game.SendCommandToClient(connectedAccount.Client, new RoomNPCUpdate
                {
                    Mode = mode,
                    NPC = npc
                });
            }
        }
        public static void CharacterUpdate(int mode, int roomID, Character character)
        {
            var accounts = game.connectedAccounts.Values.Where(x => x.Account.Character.RoomID == roomID);

            foreach (ConnectedAccount connectedAccount in accounts)
            {
                game.SendCommandToClient(connectedAccount.Client, new RoomCharacterUpdate
                {
                    Mode = mode,
                    Character = character
                });
            }
        }
        public static void CharacterStatUpdate(ConnectedAccount account)
        {
            game.SendCommandToClient(account.Client, new CharacterStatUpdate
            {
                Character = account.Account.Character
            });
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
            return null;
        }

        /// <summary>
        /// Returns an Account ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static ConnectedAccount GetConnectedAccount(string id)
        {
            if (game.connectedAccounts.ContainsKey(id))
            {
                return game.connectedAccounts[id];
            }
            return null;
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
        /// Added Item To Concurrent Queue.
        /// </summary>
        /// <param name="currentTimeStamp"></param>
        public static void AddToQueue(double currentTimeStamp)
        {
            try
            {
                foreach (double timestamp in game.actionQueue.Keys.Where(x => x <= currentTimeStamp))
                {
                    foreach (ClientCommand clientCommand in game.actionQueue[timestamp])
                    {
                        game.queuedCommand.Enqueue(clientCommand);
                        game.actionQueue.Remove(timestamp);
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Adds Item To Queue Library.
        /// </summary>
        /// <param name="currentTimeStamp"></param>
        /// <param name="command"></param>
        /// <param name="uid"></param>
        public static void AddToQueue(double currentTimeStamp, Command command, string uid)
        {
            try
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
            catch
            {

            }
        }
        #endregion
    }
}
