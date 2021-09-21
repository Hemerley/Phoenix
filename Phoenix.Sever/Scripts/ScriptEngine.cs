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
            game.script.Globals["Attack"] = new LuaAttack();
            game.script.Globals["Character"] = new LuaCharacter();
            game.script.Globals["Command"] = new LuaCommand();
            game.script.Globals["Entity"] = new LuaEntity();
            game.script.Globals["Message"] = new LuaMessage();
            game.script.Globals["NPC"] = new LuaMessage();
            game.script.Globals["Party"] = new LuaParty();
            game.script.Globals["Random"] = new LuaRandom();
            game.script.Globals["Room"] = new LuaRoom();
            game.script.Globals["Spell"] = new LuaSpell();
            game.script.Globals["Storage"] = new LuaStorage();
            game.script.Globals["Tools"] = new LuaTools();
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

    #region -- Lua Attack --
    [MoonSharpUserData]
    class LuaAttack
    {

    }
    #endregion

    #region -- Lua Character --
    [MoonSharpUserData]
    class LuaCharacter
    {
        public static class Add
        {

        }

        public static class Get
        {

        }

        public static class Remove
        {

        }

        public static class Set
        {

        }
    }
    #endregion

    #region -- Lua Command --
    [MoonSharpUserData]
    class LuaCommand
    {

    }
    #endregion

    #region -- Lua Entity --
    [MoonSharpUserData]
    class LuaEntity
    {

        public static class Add
        {

        }

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
                    foreach (NPC npc in game.currentNPC)
                    {
                        if (npc.InstanceID.ToString() == id)
                        {
                            foreach (Room room in game.rooms)
                            {
                                if (room.RoomNPC.Contains(npc))
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

        public static class Remove
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

    #region -- Lua NPC --
    [MoonSharpUserData]
    class LuaNPC
    {

        public static class Get
        {

        }

        public static class Remove
        {

        }

        public static class Set
        {

        }

    }
    #endregion

    #region -- Lua Party --
    [MoonSharpUserData]
    class LuaParty
    {

    }
    #endregion

    #region -- Lua Random --
    [MoonSharpUserData]
    class LuaRandom
    {

    }
    #endregion

    #region -- Lua Rooms --
    [MoonSharpUserData]
    class LuaRoom
    {

        public static class Character
        {

        }

        public static class Clear
        {
            
        }

        public static class Get
        {

        }

        public static class Item
        {

        }

        public static class NPC
        {

        }

        public static class Remove
        {

        }

        public static class Set
        {

        }
    
    }
    #endregion

    #region -- Lua Spell --
    [MoonSharpUserData]
    class LuaSpell
    {

    }
    #endregion

    #region -- Lua Storage --
    [MoonSharpUserData]
    class LuaStorage
    {

    }
    #endregion

    #region -- Lua Tools --
    [MoonSharpUserData]
    class LuaTools
    {

    }
    #endregion
        
}   