using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Staff;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using MoonSharp.Interpreter;
using Serilog;
using System;
using Phoenix.Server.Connections;
using static Phoenix.Server.Program;
using Phoenix.Server.Network;
using MoonSharp.VsCodeDebugger;

namespace Phoenix.Server.Scripts
{
    class ScriptEngine
    {
        public static void Initialize()
        {
            game.script.Globals["Character"] = new LuaCharacter();
            game.script.Globals["Entity"] = new LuaEntity();
            game.script.Globals["Message"] = new LuaMessage();
            game.script.Globals["Room"] = new LuaRoom();

        }

        public static void Dispose()
        {
            game.script.Globals["entityID"] = null;
            game.script.Globals["isPlayer"] = null;
            game.script.Globals["defenderID"] = null;
            game.script.Globals["isDefenderPlayer"] = null;
        }

        public static bool Movement(ConnectedAccount account)
        {
            try
            {

                game.script.Globals["entityID"] = account.Client.Id;
                game.script.Globals["isPlayer"] = true;

                DynValue result = game.script.DoFile("./Scripts/Main/Movement.lua");

                ScriptEngine.Dispose();

                return result.Boolean;
            }
            catch(Exception _ex)
            {
                Log.Error(_ex, "Error attempting player movement script.");
            }
            return false;
        }

    }

    #region -- Lua Characters --
    [MoonSharpUserData]
    class LuaCharacter
    {
        public static class Get
        {
            public static int Room(string id, bool type)
            {
                if (type)
                {
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                    {
                        if (connectedAccount.Client.Id == id)
                        {
                            return connectedAccount.Account.Character.RoomID;
                        }
                    }
                    return -1;
                }
                else
                {
                    foreach (Entity entity in game.currentEntities)
                    {
                        if (entity.InstanceID.ToString() == id)
                        {
                            foreach (Room room in game.rooms)
                            {
                                if (room.RoomEntities.Contains(entity))
                                {
                                    return room.ID;
                                }
                            }
                        }
                    }
                    return -1;
                }
            }
        }
        public static class Set
        {

        }
    }
    #endregion

    #region -- Lua Entities --
    [MoonSharpUserData]
    class LuaEntity
    {

        public static class Get
        {

        }

        public static class Set
        {

        }

    }
    #endregion

    #region -- Lua Message --
    [MoonSharpUserData]
    class LuaMessage
    {
        public static void Room(int roomID, string message)
        {
            Functions.MessageRoom(false, Helper.RemoveCaret(Helper.RemovePercent(Helper.RemovePipe(Helper.RemoveTilda(message)))), null, roomID);
        }
    }
    #endregion

    #region -- Lua Rooms --
    [MoonSharpUserData]
    class LuaRoom
    {

        public static class Get
        {

        }

        public static class Set
        {

        }
    
    }
    #endregion

}
