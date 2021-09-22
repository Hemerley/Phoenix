using MoonSharp.Interpreter;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Network;
using Serilog;
using System;
using static Phoenix.Server.Program;

namespace Phoenix.Server.Scripts
{
    class ScriptEngine
    {

        #region -- Handlers --
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
            game.script.Globals["defenderIsPlayer"] = null;
        }
        public static dynamic ReturnScript(string scriptFile, string entityID, bool isPlayer)
        {
            try
            {
                game.script.Globals["entityID"] = entityID;
                game.script.Globals["isPlayer"] = isPlayer;
                game.script.Globals["defenderID"] = -1;
                game.script.Globals["defenderIsPlayer"] = false;

                foreach (string script in game.scripts)
                {
                    if (script.Contains(scriptFile))
                    {
                        DynValue result = game.script.DoFile(script);

                        Dispose();

                        if (result.Type == DataType.Boolean)
                        {
                            return result.Boolean;
                        }
                        else if (result.Type == DataType.Number)
                        {
                            return result.Number;
                        }
                        else if (result.Type == DataType.String)
                        {
                            return result.String;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }

                Dispose();
            }
            catch (ScriptRuntimeException _ex)
            {
                Log.Error(_ex.DecoratedMessage, "Error in Run Script.");
            }
            return false;
        }
        public static dynamic ReturnScript(string scriptFile, string entityID, bool isPlayer, string defenderID, bool defenderIsPlayer)
        {
            try
            {
                game.script.Globals["entityID"] = entityID;
                game.script.Globals["isPlayer"] = isPlayer;
                game.script.Globals["defenderID"] = defenderID;
                game.script.Globals["defenderIsPlayer"] = defenderIsPlayer;


                foreach (string script in game.scripts)
                {
                    if (script.Contains(scriptFile))
                    {
                        DynValue result = game.script.DoFile(script);

                        Dispose();

                        if (result.Type == DataType.Boolean)
                        {
                            return result.Boolean;
                        }
                        else if (result.Type == DataType.Number)
                        {
                            return result.Number;
                        }
                        else if (result.Type == DataType.String)
                        {
                            return result.String;
                        }
                        else
                        {
                            return null;
                        }
                    }

                }

                Dispose();
            }
            catch (ScriptRuntimeException _ex)
            {
                Log.Error(_ex.DecoratedMessage, "Error in Run Script.");
            }
            return false;
        }
        public static void RunScript(string scriptFile, string entityID, bool isPlayer)
        {
           try
            {

                game.script.Globals["entityID"] = entityID;
                game.script.Globals["isPlayer"] = isPlayer;
                game.script.Globals["defenderID"] = -1;
                game.script.Globals["defenderIsPlayer"] = false;

                foreach (string script in game.scripts)
                {
                    if (script.Contains(scriptFile))
                    {
                        game.script.DoFile(script);
                        Dispose();
                        break;
                    }
                }
            }
            catch (ScriptRuntimeException _ex)
            {
                Log.Error(_ex.DecoratedMessage, "Error in Run Script.");
            }
        }
        public static void RunScript(string scriptFile, string entityID, bool isPlayer, string defenderID, bool defenderIsPlayer)
        {
            try
            {

                game.script.Globals["entityID"] = entityID;
                game.script.Globals["isPlayer"] = isPlayer;
                game.script.Globals["defenderID"] = defenderID;
                game.script.Globals["defenderIsPlayer"] = defenderIsPlayer;

                foreach (string script in game.scripts)
                {
                    if (script.Contains(scriptFile))
                    {
                        game.script.DoFile(script);
                        Dispose();
                        break;
                    }
                }
            }
            catch (ScriptRuntimeException _ex)
            {
                Log.Error(_ex.DecoratedMessage, "Error in Run Script.");
            }
        }
        #endregion

        #region -- Lua Public Methods --

        #region -- Combat --
        public static int Attack(int damage, string itemName, int targetID, bool targetIsPlayer)
        {
            return 0;
        }
        public static int Attack(int damage, string itemName, string weaponName, int attackerID, bool attackerIsPlayer)
        {
            return 0;
        }
        public static int DamageNegation(int damage, string itemName, int attackerID, bool attackerIsPlayer)
        {
            return 0;
        }
        public static int DamageNegation(int damage, string itemName, int attackerID, bool attackerIsPlayer, int defenderID, bool defenderIsPlayer)
        {
            return 0;
        }
        public static bool Death(int killerID, bool killerIsPlayer)
        {
            return false;
        }
        public static bool Death(int killerID, bool killerIsPlayer, int defenderID, bool defenderIsPlayer)
        {
            return false;
        }
        public static int SpellNegation(int damage, int spellType, string spellName, int attackerID, bool attackerIsPlayer)
        {
            return 0;
        }
        public static int SpellNegation(int damage, int spellType, string spellName, int attackerID, bool attackerIsPlayer, int defenderID, bool defenderIsPlayer)
        {
            return 0;
        }
        #endregion

        #region -- Items --
        public static bool Drop(string name, int inventoryNumber, int amount)
        {
            return false;
        }
        public static bool Equip(string name, int inventoryNumber, int itemType, int slotType)
        {
            return false;
        }
        public static bool Get(string name, int roomNumber)
        {
            return false;
        }
        public static bool Give(string name, int inventorySlot, int amount)
        {
            return false;
        }
        public static bool OnUse(string name, int inventoryNumber, int itemType)
        {
            return false;
        }
        public static bool UnEquip(int equipSlot)
        {
            return false;
        }
        #endregion

        #region -- Movement --
        public static bool Movement(int roomID, int entityID, bool isPlayer, string direction)
        {
            return false;
        }
        #endregion

        #region -- Timers --
        public static bool MinuteTimer()
        {
            return false;
        }
        public static bool SecondTimer()
        {
            return false;
        }
        public static bool TickTimer()
        {
            return false;
        }
        #endregion

        #endregion

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

            public static void Kill(string defenderID, bool defenderIsPlayer, string entityID, bool isPlayer)
            {
                if (defenderIsPlayer && isPlayer)
                {
                    ConnectedAccount defenderCharacter = new();
                    ConnectedAccount attackerCharacter = new();
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                    {
                        if (connectedAccount.Account.Character == null)
                        {
                            continue;
                        }
                        if (connectedAccount.Client.Id == defenderID)
                        {
                            defenderCharacter = connectedAccount;
                            break;
                        }
                    }
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                    {
                        if (connectedAccount.Account.Character == null)
                        {
                            continue;
                        }
                        if (connectedAccount.Client.Id == entityID)
                        {
                            attackerCharacter = connectedAccount;
                            break;
                        }
                    }

                    defenderCharacter.Account.Character.CurrentHealth = 0;
                    defenderCharacter.Account.Character.IsDead = true;
                    Functions.MovePlayer(0, defenderCharacter, $"~r{defenderCharacter.Account.Character.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Account.Character.Name} falls to the floor lifelessly!");
                    Functions.MessageDirect($"~w{attackerCharacter.Account.Character.Name}~m has killed you!", defenderCharacter.Client.Id);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.Account.Character.Name}~m!", attackerCharacter.Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Account.Character.Name} ~mhas killed {defenderCharacter.Account.Character.Name}~m!", attackerCharacter.Account.Character.RoomID);
                }
                else if (defenderIsPlayer && !isPlayer)
                {
                    ConnectedAccount defenderCharacter = new();
                    NPC attackerCharacter = new();
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                    {
                        if (connectedAccount.Account.Character == null)
                        {
                            continue;
                        }
                        if (connectedAccount.Client.Id == defenderID)
                        {
                            defenderCharacter = connectedAccount;
                            break;
                        }
                    }
                    foreach (NPC npc in game.currentNPC)
                    {
                        if (npc.InstanceID.ToString() == entityID)
                        {
                            attackerCharacter = npc;
                            break;
                        }
                    }

                    defenderCharacter.Account.Character.CurrentHealth = 0;
                    defenderCharacter.Account.Character.IsDead = true;
                    int roomID = defenderCharacter.Account.Character.RoomID;
                    Functions.MovePlayer(0, defenderCharacter, $"~r{defenderCharacter.Account.Character.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Account.Character} falls to the floor lifelessly!");
                    Functions.MessageDirect($"~w{attackerCharacter.BName} {attackerCharacter.Name}~m has killed you!", defenderCharacter.Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name} ~mhas killed {defenderCharacter.Account.Character.Name}~m!", roomID);
                }
                else if (!defenderIsPlayer && isPlayer)
                {
                    NPC defenderCharacter = new();
                    ConnectedAccount attackerCharacter = new();
                    foreach (NPC npc in game.currentNPC)
                    {
                        if (npc.InstanceID.ToString() == entityID)
                        {
                            defenderCharacter = npc;
                            break;
                        }
                    }
                    foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                    {
                        if (connectedAccount.Account.Character == null)
                        {
                            continue;
                        }
                        if (connectedAccount.Client.Id == entityID)
                        {
                            attackerCharacter = connectedAccount;
                            break;
                        }
                    }
                    game.currentNPC.Remove(defenderCharacter);
                    Functions.EntityUpdate(2, attackerCharacter.Account.Character.RoomID, defenderCharacter);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.BName} {defenderCharacter.Name}!", attackerCharacter.Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Account.Character.Name} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name}~m!", attackerCharacter.Account.Character.RoomID);
                }
                else if (!defenderIsPlayer && !isPlayer)
                {
                    NPC defenderCharacter = new();
                    NPC attackerCharacter = new();
                    int roomID = -1;
                    foreach (NPC npc in game.currentNPC)
                    {
                        if (npc.InstanceID.ToString() == entityID)
                        {
                            defenderCharacter = npc;
                            break;
                        }
                    }
                    foreach (NPC npc in game.currentNPC)
                    {
                        if (npc.InstanceID.ToString() == entityID)
                        {
                            attackerCharacter = npc;
                            break;
                        }
                    }
                    foreach (Room room in game.rooms)
                    {
                        if (room.RoomNPC.Contains(attackerCharacter))
                        {
                            roomID = room.ID;
                            break;
                        }
                    }
                    game.currentNPC.Remove(defenderCharacter);
                    Functions.EntityUpdate(2, roomID, defenderCharacter);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name}~m!", roomID);
                }
            }

            public static class Add
            {

            }

            public static class Get
            {
                public static string HisHer(string id, bool type)
                {
                    if (type)
                    {
                        foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                        {
                            if (connectedAccount.Client.Id == id)
                            {
                                return connectedAccount.Account.Character.HisHer;
                            }
                        }
                        return "";
                    }
                    else
                    {
                        foreach (NPC npc in game.currentNPC)
                        {
                            if (npc.InstanceID.ToString() == id)
                            {
                                return npc.HisHer;
                            }
                        }
                        return "";
                    }
                }
                public static string Name(string id, bool type)
                {
                    if (type)
                    {
                        foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
                        {
                            if (connectedAccount.Client.Id == id)
                            {
                                return connectedAccount.Account.Character.Name;
                            }
                        }
                        return "";
                    }
                    else
                    {
                        foreach (NPC npc in game.currentNPC)
                        {
                            if (npc.InstanceID.ToString() == id)
                            {
                                return npc.Name;
                            }
                        }
                        return "";
                    }
                }
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
            public static void Direct(string id, string message)
            {
                Functions.MessageDirect(Helper.RemoveCaret(Helper.RemovePercent(Helper.RemovePipe(Helper.RemoveTilda(message)))), id);
            }
            public static void Room(int roomID, string message)
            {
                Functions.MessageRoom(Helper.RemoveCaret(Helper.RemovePercent(Helper.RemovePipe(Helper.RemoveTilda(message)))), roomID);
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
            public static string FirstToUpper(string value)
            {
                return value.FirstCharToUpper();
            }
        }
        #endregion

    }
}