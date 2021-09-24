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
                public static int StaffLevel(string entityID)
                {
                    if (game.connectedAccounts.ContainsKey(entityID))
                        return game.connectedAccounts[entityID].Account.Character.TypeID;
                    else
                        return -1;
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

                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    Character attackerCharacter = game.connectedAccounts[entityID].Account.Character;

                    defenderCharacter.CurrentHealth = 0;
                    defenderCharacter.IsDead = true;
                    Functions.MovePlayer(0, game.connectedAccounts[defenderID], $"~r{defenderCharacter.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Name.FirstCharToUpper()} falls to the floor lifelessly!", game.connectedAccounts[defenderID]);
                    Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()}~m has killed you!", game.connectedAccounts[defenderID].Client.Id);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.Name.FirstCharToUpper()}~m!", game.connectedAccounts[entityID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID, game.connectedAccounts[entityID]);
                    Functions.CharacterStatUpdate(game.connectedAccounts[defenderID], defenderCharacter);
                    var respawnCommand = new RespawnCharacterServer
                    {
                        RoomID = defenderCharacter.Recall.ToString(),
                        EntityID = defenderID,
                        ArrivalMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gappears back in the world of the living following a bright white flash!",
                        DepartureMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gvanishes back to the world of the living following a bright white flash!"
                    };
                    Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeSeconds() + 20, respawnCommand, game.serverID.ToString());
                }
                else if (defenderIsPlayer && !isPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    NPC attackerCharacter = game.currentNPC[entityID];

                    defenderCharacter.CurrentHealth = 0;
                    defenderCharacter.IsDead = true;
                    int roomID = game.connectedAccounts[defenderID].Account.Character.RoomID;
                    Functions.MovePlayer(0, game.connectedAccounts[defenderID], $"~r{defenderCharacter.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Name.FirstCharToUpper()} falls to the floor lifelessly!", game.connectedAccounts[defenderID]); ;
                    Functions.MessageDirect($"~w{attackerCharacter.BName} {attackerCharacter.Name}~m has killed you!", game.connectedAccounts[defenderID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name} ~mhas killed {defenderCharacter.Name.FirstCharToUpper()}~m!", roomID);
                    Functions.CharacterStatUpdate(game.connectedAccounts[defenderID], defenderCharacter);
                    var respawnCommand = new RespawnCharacterServer
                    {
                        RoomID = defenderCharacter.Recall.ToString(),
                        EntityID = defenderID,
                        ArrivalMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gappears back in the world of the living following a bright white flash!",
                        DepartureMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gvanishes back to the world of the living following a bright white flash!"
                    };
                    Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeSeconds() + 20, respawnCommand, game.serverID.ToString());
                }
                else if (!defenderIsPlayer && isPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    Character attackerCharacter = game.connectedAccounts[entityID].Account.Character;

                    game.currentNPC.Remove(defenderID);
                    Functions.NPCUpdate(2, attackerCharacter.RoomID, defenderCharacter);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}!", game.connectedAccounts[entityID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID, game.connectedAccounts[entityID]);
                }
                else if (!defenderIsPlayer && !isPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    NPC attackerCharacter = game.currentNPC[entityID];
                    game.currentNPC.Remove(defenderID);
                    Functions.NPCUpdate(2, attackerCharacter.RoomID, defenderCharacter);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID);
                }
            }

            public static class Add
            {

            }

            public static class Get
            {
                public static string HisHer(string entityID, bool isPlayer)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.HisHer : "";
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].HisHer : "";
                    }
                }
                public static string Name(string entityID, bool isPlayer)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.Name : "";
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].Name : "";
                    }
                }
                public static int Room(string entityID, bool isPlayer)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.RoomID : -1;
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].RoomID : -1;
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
            public static void Room(int roomID, string message, string entityID)
            {
                Functions.MessageRoom(Helper.RemoveCaret(Helper.RemovePercent(Helper.RemovePipe(Helper.RemoveTilda(message)))), roomID, game.connectedAccounts[entityID]);
            }
            public static void Room(int roomID, string message, string entityID, string defenderID)
            {
                Functions.MessageRoom(Helper.RemoveCaret(Helper.RemovePercent(Helper.RemovePipe(Helper.RemoveTilda(message)))), roomID, game.connectedAccounts[entityID], game.connectedAccounts[defenderID]);
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
            private static readonly Random random = new();
            public static int Number(int min, int max)
            {
                return random.Next(min, max);
            }
            public static double NumberDouble(double min, double max)
            {
                return random.NextDouble() * (min - max) + min;
            }
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