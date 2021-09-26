using MoonSharp.Interpreter;
using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Staff;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Data;
using Phoenix.Server.Scripts;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Phoenix.Server.Network
{
    public class Game
    {

        #region -- Game Initialization --

        #region -- Private --

        /// <summary>
        /// Declaration of the Server.
        /// </summary>
        private Listener server;

        /// <summary>
        /// Boolean for Game Thread.
        /// </summary>
        private readonly bool stopGameWorkerThread = false;

        /// <summary>
        /// Declaration of Game Thread that controls Game Logic.
        /// </summary>
        private Thread gameWorkerThread;

        #endregion

        #region -- Public --

        /// <summary>
        /// Contains unique ID for server.
        /// </summary>
        public Guid serverID = Guid.NewGuid();

        /// <summary>
        /// Declaration of Concurrent Queue that handles queue of commands.
        /// </summary>
        public readonly ConcurrentQueue<ClientCommand> queuedCommand = new();

        /// <summary>
        /// Queue Command Library.
        /// </summary>
        public readonly Dictionary<double, List<ClientCommand>> actionQueue = new();

        /// <summary>
        /// Connected clients that are NOT authenticated yet.
        /// </summary>
        public readonly Dictionary<string, ConnectedClient> connectedClients = new();

        /// <summary>
        /// Connected clients that ARE authenticated.
        /// </summary>
        public readonly Dictionary<string, ConnectedAccount> connectedAccounts = new();

        /// <summary>
        /// Declaration of Scripts in ./Scripts/
        /// </summary>
        public readonly List<string> scripts = new();

        /// <summary>
        /// Script Handlers for Game Engine.
        /// </summary>
        public Script script = new();

        /// <summary>
        /// Declaration of Loaded Rooms.
        /// </summary>
        public List<Room> rooms = new();

        /// <summary>
        /// Declaration of Current NPC Spawned.
        /// </summary>
        public readonly Dictionary<string, NPC> currentNPC = new();

        /// <summary>
        /// Declaration of Current Rooms.
        /// </summary>
        public readonly Dictionary<string, Room> currentRooms = new();

        public bool showCommands = false;

        /// <summary>
        /// Declaration of Total Connections This Reboot.
        /// </summary>
        public int totalConnections = 0;

        /// <summary>
        /// Declaration of Maximum Players.
        /// </summary>
        public int maximumPlayers = 0;

        #endregion

        /// <summary>
        /// Start Server.
        /// </summary>
        public void Start()
        {
            this.gameWorkerThread = new Thread(new ThreadStart(GameWorkerThread));
            this.gameWorkerThread.Start();
        }

        #endregion

        #region -- Events --

        /// <summary>
        /// Handles Client Connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_OnClientConnected(object sender, ConnectedClient e)
        {
            //store the connected client
            this.connectedClients.Add(e.Id, e);

            // Log Command
            Log.Information($"{e.Id} has connected.");
        }

        /// <summary>
        /// Handles Incoming Client Commands.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_OnClientMessage(object sender, ConnectedClientMessage e)
        {
            string[] commands = e.Message.Split("%", StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in commands)
            {
                var command = CommandFactory.ParseCommand(c);
                double currentTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                Functions.AddToQueue(currentTimeStamp, command, e.ConnectedClient.Id);
            }
        }

        /// <summary>
        /// Handles Client Disconnection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_OnClientDisconnected(object sender, ConnectedClient e)
        {
            //Remove from EITHER connected clients OR connected accounts (it could be in either)
            if (this.connectedClients.ContainsKey(e.Id))
            {
                this.connectedClients.Remove(e.Id);
                return;
            }

            //coding it this way to be safe, just in case we have a different instance of the same connection
            //I DOUBT we do though
            var connectedAccount = this.connectedAccounts[e.Id];
            if (connectedAccount.Account.Character != null)
            {
                rooms[connectedAccount.Account.Character.RoomID].RoomCharacters.Remove(connectedAccount.Account.Character);
                foreach (ConnectedAccount cAccount in connectedAccounts.Values)
                {
                    if (cAccount.Account.Character.RoomID == connectedAccount.Account.Character.RoomID)
                    {
                        Functions.CharacterUpdate(2, connectedAccount.Account.Character.RoomID, connectedAccount.Account.Character);
                    }
                }
                Functions.MessageWorld($"&tilda&g{connectedAccount.Account.Character.Name}&tilda&w has went &tilda&roffline&tilda&w!");
            }
            Database.SetCharacter(Constants.GAME_MODE, connectedAccount.Account.Character);
            this.connectedAccounts.Remove(connectedAccount.Client.Id);


            // Log Command
            Log.Information($"{e.Id} has disconnected.");
        }

        /// <summary>
        /// Handled Server Boot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_OnServerStarted(object sender, EventArgs e)
        {
            // Log Command
            Log.Information("Server Started!");
        }

        /// <summary>
        /// Handles Server Shutdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Server_OnServerStopped(object sender, EventArgs e)
        {
            // Log Command
            Log.Information("Server Shutdown!");
        }

        #endregion

        #region -- Send Commands --

        /// <summary>
        /// Sends Command to a Client.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="command"></param>
        public void SendCommandToClient(ConnectedClient client, Command command)
        {
            var message = CommandFactory.FormatCommand(command);
            client.Send(message);
            if (showCommands)
                Log.Information($"{command.CommandType} sent to {client.Id}");
        }

        #endregion

        #region -- Game Loop --

        /// <summary>
        /// Game Loop Thread.
        /// </summary>
        private void GameWorkerThread()
        {

            #region --  Initialize Thread & Turn Server On --

            // Load Database .
            Database.InitializeDatabse();

            Log.Information("Loading Rooms...");
            this.rooms = Database.LoadRooms(Constants.GAME_MODE);
            Log.Information("Rooms Loaded!");

            Log.Information("Initializing Script Globals...");
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                UserData.RegisterAssembly(assembly);
            ScriptEngine.Initialize();
            Log.Information("Script Globals Initialized!");

            Log.Information("Loading Scripts...");
            string[] files = Directory.GetFiles("./Scripts/", "*.*", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                this.scripts.Add(file.ToLower());
            }
            foreach (string script in scripts)
            {
                Log.Information($"Located Script: {script}");
            }
            Log.Information("Scripts Loaded!");

            // Initialize Server.
            this.server = new Listener();

            // Register Events.
            this.server.OnClientConnected += Server_OnClientConnected;
            this.server.OnClientDisconnected += Server_OnClientDisconnected;
            this.server.OnClientMessage += Server_OnClientMessage;
            this.server.OnServerStarted += Server_OnServerStarted;
            this.server.OnServerStopped += Server_OnServerStopped;

            // Start Listening.
            this.server.Start(IPAddress.IPv6Any, Constants.LIVE_PORT);

            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 30000, new SpawnNPCServer(), serverID.ToString());
            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 30000, new TickTimerServer(), serverID.ToString());
            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 30000, new SecondTimerServer(), serverID.ToString());
            Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 30000, new MinuteTimerServer(), serverID.ToString());
            #endregion

            while (!this.stopGameWorkerThread)
            {

                #region -- Queue Handling --

                // Establish Time Stamp.
                var currentTimeStamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                // Check Queue Stack & Remove Items with TimeStamp. Hands off to Queue.
                Functions.AddToQueue(currentTimeStamp);

                // Check Queue for items.
                if (!this.queuedCommand.TryDequeue(out ClientCommand cmd))
                {
                    Thread.Sleep(10);
                    continue;
                }

                // Handle Queue items.
                var clientId = cmd.Id;
                var command = cmd.Command;
                var clientWhoSendCommand = Functions.GetClientById(clientId);
                var accountConnected = Functions.GetConnectedAccount(clientId);

                if (showCommands)
                    Log.Information($"{command.CommandType} from {clientId}.");

                // Check if player is connected.
                if (clientWhoSendCommand == null && accountConnected == null && clientId != serverID.ToString())
                    continue;

                #endregion

                switch (command.CommandType)
                {

                    #region -- Authenticate Response --

                    case CommandType.Authenticate:
                        {
                            var parsedCommand = command as AuthenticateRequest;

                            SendCommandToClient(clientWhoSendCommand, Functions.Authenticate(parsedCommand.Version, parsedCommand.Username.ToLower(), parsedCommand.Password, clientWhoSendCommand));

                            break;
                        }
                    #endregion

                    #region -- New Account Response --

                    case CommandType.NewAccount:
                        {
                            var parsedCommand = command as NewAccountRequest;

                            SendCommandToClient(clientWhoSendCommand, Functions.NewAccount(parsedCommand.Version, parsedCommand.Username.ToLower(), parsedCommand.Password, parsedCommand.Email, clientWhoSendCommand));

                            break;
                        }
                    #endregion

                    #region -- New Character Command --

                    case CommandType.NewCharacter:
                        {
                            var parsedCommand = command as NewCharacterCommand;

                            SendCommandToClient(accountConnected.Client, Functions.NewCharacter(parsedCommand.CharacterName.ToLower(), parsedCommand.Gender, parsedCommand.Philosophy, parsedCommand.Image, accountConnected));

                            break;
                        }
                    #endregion

                    #region -- Character List Response --

                    case CommandType.CharacterList:
                        {
                            var parsedCommand = command as GetCharacterListRequest;

                            SendCommandToClient(accountConnected.Client, Functions.CharacterList(accountConnected));

                            break;
                        }
                    #endregion

                    #region -- Character Login Response --

                    case CommandType.CharacterLogin:
                        {
                            var parsedCommand = command as CharacterConnectRequest;

                            SendCommandToClient(accountConnected.Client, Functions.CharacterConnect(parsedCommand.Name.ToLower(), accountConnected));

                            break;
                        }
                    #endregion

                    #region -- Message Room Response --
                    case CommandType.MessageRoom:
                        {
                            var parsedCommand = command as MessageRoomServer;

                            Functions.MessageRoom(parsedCommand.Message, accountConnected.Account.Character.RoomID, accountConnected.Account.Character);

                            break;
                        }
                    #endregion

                    #region  -- Message Direct Response --
                    case CommandType.MessageDirect:
                        {
                            var parsedCommand = command as MessageDirectServer;
                            Functions.MessageDirect(parsedCommand.Message, parsedCommand.SendingName.ToLower(), parsedCommand.ReceivingName.ToLower(), accountConnected);
                            break;
                        }
                    #endregion

                    #region  -- Message Party Response --
                    #endregion

                    #region  -- Message Guild Response --
                    #endregion

                    #region -- Message World Response --
                    case CommandType.MessageWorld:
                        {
                            var parsedCommand = command as MessageWorldServer;
                            Functions.MessageWorld(parsedCommand.Message, parsedCommand.ID.ToString(), accountConnected);
                            break;
                        }
                    #endregion

                    #region  -- Message Broadcast Response --
                    #endregion

                    #region -- Slash Command --
                    case CommandType.SlashCommand:
                        {
                            var parsedCommand = command as SlashCommandRequest;
                            string[] c = parsedCommand.Message.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                            if (c[0].ToLower()[1..] == "attack")
                                if (accountConnected.Account.Character.IsAttacking == true) return;
                            Functions.SlashCommand(parsedCommand.Message, accountConnected);
                            break;
                        }
                    #endregion

                    #region -- Client Connect Response --
                    case CommandType.ClientConnect:
                        {
                            var parsedCommand = command as ClientConnectRequest;
                            SendCommandToClient(accountConnected.Client, Functions.ClientConnect(parsedCommand.Id));
                            break;
                        }
                    #endregion

                    #region -- Client Room Response --
                    case CommandType.ClientRoom:
                        {
                            var parseCommand = command as ClientRoomRequest;
                            Functions.MovementUpdate(accountConnected);
                            break;
                        }
                    #endregion

                    #region  -- Map Response --
                    #endregion

                    #region  -- Room Player Update --
                    #endregion

                    #region  -- Room NPC Update --
                    #endregion

                    #region  -- Player Move Request --
                    case CommandType.CharacterMoveRequest:
                        {
                            var parsedCommand = command as CharacterMoveRequest;

                            Functions.MovePlayer(parsedCommand.Direction, accountConnected);

                            break;
                        }
                    #endregion

                    #region  -- Spawn NPC --
                    case CommandType.SpawnNPC:
                        {
                            var parsedCommand = command as SpawnNPCServer;

                            Functions.SpawnNPC(parsedCommand.CharacterName, parsedCommand.NPCName);
                        }
                        break;
                    #endregion

                    #region -- Tick Timer --

                    case CommandType.TickTimer:
                        {
                            Functions.TickTimer();
                            break;
                        }
                    #endregion

                    #region -- Second Timer --

                    case CommandType.SecondTimer:
                        {
                            Functions.SecondTimer();
                            break;
                        }
                    #endregion

                    #region -- Minute Timer --

                    case CommandType.MinuteTimer:
                        {
                            Functions.MinuteTimer();
                            break;
                        }
                    #endregion

                    #region  -- Respawn Character --
                    case CommandType.RespawnCharacter:
                        {
                            var parsedCommand = command as RespawnCharacterServer;

                            Functions.RespawnCharacter(Int32.Parse(parsedCommand.RoomID), this.connectedAccounts[parsedCommand.EntityID], parsedCommand.ArrivalMessage, parsedCommand.DepartureMessage);
                        }
                        break;
                    #endregion

                    #region  -- Summon Player --
                    case CommandType.SummonPlayer:
                        {
                            var parsedCommand = command as SummonPlayerStaff;

                            Functions.MovePlayer(1, accountConnected, parsedCommand.Name, $"&tilda&mA bright white portal opens and &tilda&w{parsedCommand.Name.ToLower().FirstCharToUpper()}&tilda&m steps through.", $"&tilda&mA bright white portal opens and &tilda&w{parsedCommand.Name.ToLower().FirstCharToUpper()}&tilda&m is pulled into it!");
                        }
                        break;
                    #endregion

                    #region -- Unknown Command --

                    case CommandType.Unknown:
                    default:
                        {
                            Log.Error($"Unknown command from {clientId}.");
                            break;
                        }
                        #endregion

                }
                Thread.Sleep(10);
            }
        }
        #endregion
    }
}
