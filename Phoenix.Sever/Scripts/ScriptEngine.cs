using MoonSharp.Interpreter;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Network;
using Serilog;
using System;
using static Phoenix.Server.Program;

namespace Phoenix.Server.Scripts
{
    class ScriptEngine
    {
        /*
         * Fix Exp Bar
         * Add Tick Timer to Attack
         * Up Arrow - Last Command
         * Update Command Block When Dead
         */
        #region -- Handlers --
        public static void Initialize()
        {
            game.script.Globals["Attack"] = new LuaAttack();
            game.script.Globals["Character"] = new LuaCharacter();
            game.script.Globals["Command"] = new LuaCommand();
            game.script.Globals["Entity"] = new LuaEntity();
            game.script.Globals["General"] = new LuaGeneral();
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
                    if (script.Contains(scriptFile.ToLower()))
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
                    if (script.Contains(scriptFile.ToLower()))
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
                    if (script.Contains(scriptFile.ToLower()))
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
                    if (script.Contains(scriptFile.ToLower()))
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

        #region -- Lua Attack --
        [MoonSharpUserData]
        class LuaAttack
        {
            public static int Damage(string attackerID, bool attackerIsPlayer, string defenderID, bool defenderIsPlayer)
            {
                if (defenderIsPlayer && attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;

                    return Convert.ToInt32((attackerCharacter.Damage * LuaRandom.NumberDouble(.80, 1)) - (defenderCharacter.CurrentArmor * .5));
                }
                else if (defenderIsPlayer && !attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    NPC attackerCharacter = game.currentNPC[attackerID];

                    return Convert.ToInt32((attackerCharacter.Damage * LuaRandom.NumberDouble(.80, 1)) - (defenderCharacter.CurrentArmor * .5));
                }
                else if (!defenderIsPlayer && attackerIsPlayer)
                {
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;

                    return Convert.ToInt32(attackerCharacter.Damage * LuaRandom.NumberDouble(.80, 1));
                }
                else if (!defenderIsPlayer && !attackerIsPlayer)
                {
                    NPC attackerCharacter = game.currentNPC[attackerID];
                    return Convert.ToInt32(attackerCharacter.Damage * LuaRandom.NumberDouble(.80, 1));
                }
                return 0;
            }
            public static bool Death(int health)
            {
                return health <= 0;
            }
            public static bool Dodge(string attackerID, bool attackerIsPlayer, string defenderID, bool defenderIsPlayer)
            {
                if (defenderIsPlayer && attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;

                    double levelDifference = Convert.ToDouble(defenderCharacter.RankID) / Convert.ToDouble(attackerCharacter.RankID);
                    double dodgeChance = Math.Min(0.5d, (Convert.ToDouble(defenderCharacter.CurrentAgility) * levelDifference) / Convert.ToDouble(attackerCharacter.CurrentAgility));

                    if (LuaRandom.NumberDouble(0, 1) < dodgeChance)
                        return true;
                    return false;
                }
                else if (defenderIsPlayer && !attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    NPC attackerCharacter = game.currentNPC[attackerID];

                    double levelDifference = Convert.ToDouble(defenderCharacter.RankID) / Convert.ToDouble(attackerCharacter.Level);
                    double dodgeChance = Math.Min(0.5d, (Convert.ToDouble(defenderCharacter.CurrentAgility) * levelDifference) / Convert.ToDouble(attackerCharacter.CurrentAgility));

                    if (LuaRandom.NumberDouble(0, 1) < dodgeChance)
                        return true;
                    return false;
                }
                else if (!defenderIsPlayer && attackerIsPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;


                    double levelDifference = Convert.ToDouble(defenderCharacter.Level) / Convert.ToDouble(attackerCharacter.RankID);
                    double dodgeChance = Math.Min(0.5d, (Convert.ToDouble(defenderCharacter.CurrentAgility) * levelDifference) / Convert.ToDouble(attackerCharacter.CurrentAgility));

                    if (LuaRandom.NumberDouble(0, 1) < dodgeChance)
                        return true;
                    return false;
                }
                else
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    NPC attackerCharacter = game.currentNPC[attackerID];

                    double levelDifference = Convert.ToDouble(defenderCharacter.Level) / Convert.ToDouble(attackerCharacter.Level);
                    double dodgeChance = Math.Min(0.5d, (Convert.ToDouble(defenderCharacter.CurrentAgility) * levelDifference) / Convert.ToDouble(attackerCharacter.CurrentAgility));

                    if (LuaRandom.NumberDouble(0, 1) < dodgeChance)
                        return true;
                    return false;
                }
            }
            public static void Full(string attackerID, bool attackerIsPlayer, string defenderID, bool defenderIsPlayer, string itemName)
            {
                if (defenderIsPlayer && attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;

                    if (defenderCharacter.RoomID != attackerCharacter.RoomID)
                    {
                        LuaMessage.Direct(attackerID, $"~w{defenderCharacter.Name} is not here!");
                        return;
                    }

                    if (Dodge(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer))
                    {
                        Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()} ~oattempts to attack you with their ~w{itemName.ToLower()}~o, but ~ymisses~o!", defenderID);
                        Functions.MessageDirect($"~oYou attempt to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~owith your ~w{itemName.ToLower()}~o, but ~ymiss~o!", attackerID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattempted to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}, but missed!", defenderCharacter.RoomID, game.connectedAccounts[defenderID], game.connectedAccounts[attackerID]);
                        attackerCharacter.IsAttacking = true;
                        attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                        attackerCharacter.TargetIsPlayer = true;
                        attackerCharacter.HealthRegen = false;
                        defenderCharacter.HealthRegen = false;
                        if (defenderCharacter.AutoAttack)
                        {
                            defenderCharacter.IsAttacking = true;
                            defenderCharacter.TargetIsPlayer = true;
                            defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                        }
                    }
                    else
                    {
                        int totalDamage = Damage(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer);
                        int death = LuaEntity.Remove.CurrentHealth(defenderID, defenderIsPlayer, totalDamage);
                        Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()} ~oattacks you with their ~w{itemName.ToLower()}~o for ~y{totalDamage}~o points of damage!", defenderID);
                        Functions.MessageDirect($"~oYou attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~owith your ~w{itemName.ToLower()}~o for ~y{totalDamage}~o points of damage!", attackerID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattacks ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}!", defenderCharacter.RoomID, game.connectedAccounts[defenderID], game.connectedAccounts[attackerID]);
                        if (Death(death))
                        {
                            LuaEntity.Kill(defenderID, defenderIsPlayer, attackerID, attackerIsPlayer);
                            attackerCharacter.IsAttacking = false;
                            attackerCharacter.TargetID = "";
                            defenderCharacter.IsAttacking = false;
                            defenderCharacter.TargetID = "";
                            attackerCharacter.HealthRegen = true;
                        }
                        else
                        {
                            Functions.CharacterStatUpdate(game.connectedAccounts[defenderID]);
                            attackerCharacter.IsAttacking = true;
                            attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                            attackerCharacter.TargetIsPlayer = true;
                            attackerCharacter.HealthRegen = false;
                            defenderCharacter.HealthRegen = false;
                            if (defenderCharacter.AutoAttack)
                            {
                                defenderCharacter.IsAttacking = true;
                                defenderCharacter.TargetIsPlayer = true;
                                defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                            }
                        }
                    }
                }
                else if (defenderIsPlayer && !attackerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    NPC attackerCharacter = game.currentNPC[attackerID];
                    if (defenderCharacter.RoomID != attackerCharacter.RoomID)
                    {
                        return;
                    }

                    if (LuaAttack.Dodge(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer))
                    {
                        Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()} ~oattempts to attack you with their ~w{itemName.ToLower()}~o, but ~ymisses~o!", defenderID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattempted to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}, but missed!", defenderCharacter.RoomID, game.connectedAccounts[defenderID]);
                        attackerCharacter.IsAttacking = true;
                        attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                        attackerCharacter.TargetIsPlayer = true;
                        defenderCharacter.HealthRegen = false;
                        if (defenderCharacter.AutoAttack)
                        {
                            defenderCharacter.IsAttacking = true;
                            defenderCharacter.TargetIsPlayer = false;
                            defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                        }
                    }
                    else
                    {
                        int totalDamage = LuaAttack.Damage(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer);
                        int death = LuaEntity.Remove.CurrentHealth(defenderID, defenderIsPlayer, totalDamage);
                        Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()} ~oattacks you with their ~w{itemName.ToLower()}~o for ~y{totalDamage}~o points of damage!", defenderID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattacks ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}!", defenderCharacter.RoomID, game.connectedAccounts[defenderID]);
                        if (Death(death))
                        {
                            LuaEntity.Kill(defenderID, defenderIsPlayer, attackerID, attackerIsPlayer);
                            attackerCharacter.IsAttacking = false;
                            attackerCharacter.TargetID = "";
                            defenderCharacter.IsAttacking = false;
                            defenderCharacter.TargetID = "";
                        }
                        else
                        {
                            Functions.CharacterStatUpdate(game.connectedAccounts[defenderID]);
                            attackerCharacter.IsAttacking = true;
                            attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                            attackerCharacter.TargetIsPlayer = true;
                            defenderCharacter.HealthRegen = false;
                            if (defenderCharacter.AutoAttack)
                            {
                                defenderCharacter.IsAttacking = true;
                                defenderCharacter.TargetIsPlayer = false;
                                defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                            }
                        }
                    }
                }
                else if (!defenderIsPlayer && attackerIsPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    Character attackerCharacter = game.connectedAccounts[attackerID].Account.Character;

                    if (defenderCharacter.RoomID != attackerCharacter.RoomID)
                    {
                        LuaMessage.Direct(attackerID, $"~w{defenderCharacter.Name} is not here!");
                        return;
                    }

                    if (LuaAttack.Dodge(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer))
                    {
                        Functions.MessageDirect($"~oYou attempt to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~owith your ~w{itemName.ToLower()}~o, but ~ymiss~o!", attackerID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattempted to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}, but missed!", defenderCharacter.RoomID, game.connectedAccounts[attackerID]);
                        attackerCharacter.IsAttacking = true;
                        attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                        attackerCharacter.TargetIsPlayer = false;
                        defenderCharacter.IsAttacking = true;
                        defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                        defenderCharacter.TargetIsPlayer = true;
                        attackerCharacter.HealthRegen = false;
                    }
                    else
                    {
                        int totalDamage = LuaAttack.Damage(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer);
                        int death = LuaEntity.Remove.CurrentHealth(defenderID, defenderIsPlayer, totalDamage);
                        Functions.MessageDirect($"~oYou attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~owith your ~w{itemName.ToLower()}~o for ~y{totalDamage}~o points of damage!", attackerID);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattacks ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}!", defenderCharacter.RoomID, game.connectedAccounts[attackerID]);
                        if (LuaAttack.Death(death))
                        {
                            LuaEntity.Kill(defenderID, defenderIsPlayer, attackerID, attackerIsPlayer);
                            LuaCharacter.Add.Experience(attackerID, defenderCharacter.Level);
                            LuaCharacter.Add.Gold(attackerID, defenderCharacter.Gold);
                            LuaNPC.Drops(defenderCharacter, attackerID, attackerIsPlayer);
                            Functions.CharacterStatUpdate(game.connectedAccounts[attackerID]);
                            attackerCharacter.IsAttacking = false;
                            attackerCharacter.TargetID = "";
                            defenderCharacter.IsAttacking = false;
                            defenderCharacter.TargetID = "";
                            attackerCharacter.HealthRegen = true;
                        }
                        else
                        {
                            attackerCharacter.IsAttacking = true;
                            attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                            attackerCharacter.TargetIsPlayer = false;
                            defenderCharacter.IsAttacking = true;
                            defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                            defenderCharacter.TargetIsPlayer = true;
                            attackerCharacter.HealthRegen = false;
                        }
                    }
                }
                else if (!defenderIsPlayer && !attackerIsPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    NPC attackerCharacter = game.currentNPC[attackerID];
                    if (defenderCharacter.RoomID != attackerCharacter.RoomID)
                    {
                        return;
                    }
                    if (LuaAttack.Dodge(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer))
                    {
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattempted to attack ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}, but missed!", defenderCharacter.RoomID);
                        attackerCharacter.IsAttacking = true;
                        attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                        attackerCharacter.TargetIsPlayer = false;
                        defenderCharacter.IsAttacking = true;
                        defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                        defenderCharacter.TargetIsPlayer = false;
                    }
                    else
                    {
                        int totalDamage = LuaAttack.Damage(attackerID, attackerIsPlayer, defenderID, defenderIsPlayer);
                        int death = LuaEntity.Remove.CurrentHealth(defenderID, defenderIsPlayer, totalDamage);
                        Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~cattacks ~w{defenderCharacter.Name.FirstCharToUpper()} ~cwith their {itemName.ToLower()}!", defenderCharacter.RoomID);
                        if (LuaAttack.Death(death))
                        {
                            LuaEntity.Kill(defenderID, defenderIsPlayer, attackerID, attackerIsPlayer);
                            // Do Drops (Cannot Do Until Party System)
                            attackerCharacter.IsAttacking = false;
                            attackerCharacter.Name = "";
                        }
                        else
                        {
                            attackerCharacter.IsAttacking = true;
                            attackerCharacter.TargetID = defenderCharacter.Name.ToLower();
                            attackerCharacter.TargetIsPlayer = false;
                            defenderCharacter.IsAttacking = true;
                            defenderCharacter.TargetID = attackerCharacter.Name.ToLower();
                            defenderCharacter.TargetIsPlayer = false;
                        }
                    }
                }
            }
        }
        #endregion

        #region -- Lua Character --
        [MoonSharpUserData]
        class LuaCharacter
        {
            public static class Add
            {
                public static void Experience(string characterID, int level)
                {
                    Character character = game.connectedAccounts[characterID].Account.Character;
                    int experience = level * 300;
                    double levelDifference = Math.Max(0d, Math.Min(1.5d, Convert.ToDouble(level) / Convert.ToDouble(character.RankID)));
                    double experienceMod = Convert.ToDouble(experience) * levelDifference;
                    character.Experience += Convert.ToInt32(experienceMod);
                    if (experienceMod > 0) LuaMessage.Direct(characterID, $"~cYou have gained ~w{experienceMod} ~cexperience!");
                }
                public static void Gold(string characterID, int gold)
                {
                    game.connectedAccounts[characterID].Account.Gold += gold;
                    LuaMessage.Direct(characterID, $"~cYou loot ~w{gold} ~cgold!");
                }
            }
            public static class Get
            {
                public static int StaffLevel(string characterID)
                {
                    if (game.connectedAccounts.ContainsKey(characterID))
                        return game.connectedAccounts[characterID].Account.Character.TypeID;
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
            public static void Kill(string defenderID, bool defenderIsPlayer, string killerID, bool killerIsPlayer)
            {
                if (defenderIsPlayer && killerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    Character attackerCharacter = game.connectedAccounts[killerID].Account.Character;
                    defenderCharacter.CurrentHealth = 0;
                    defenderCharacter.IsDead = true;
                    defenderCharacter.HealthRegen = false;
                    Functions.MovePlayer(0, game.connectedAccounts[defenderID], $"~r{defenderCharacter.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Name.FirstCharToUpper()} falls to the floor lifelessly!", game.connectedAccounts[defenderID]);
                    Functions.MessageDirect($"~w{attackerCharacter.Name.FirstCharToUpper()}~m has killed you!", game.connectedAccounts[defenderID].Client.Id);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.Name.FirstCharToUpper()}~m!", game.connectedAccounts[killerID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID, game.connectedAccounts[killerID]);
                    Functions.CharacterStatUpdate(game.connectedAccounts[defenderID]);
                    var respawnCommand = new RespawnCharacterServer
                    {
                        RoomID = defenderCharacter.Recall.ToString(),
                        EntityID = defenderID,
                        ArrivalMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gappears back in the world of the living following a bright white flash!",
                        DepartureMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gvanishes back to the world of the living following a bright white flash!"
                    };
                    Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 20000, respawnCommand, game.serverID.ToString());
                }
                else if (defenderIsPlayer && !killerIsPlayer)
                {
                    Character defenderCharacter = game.connectedAccounts[defenderID].Account.Character;
                    NPC attackerCharacter = game.currentNPC[killerID];
                    defenderCharacter.CurrentHealth = 0;
                    defenderCharacter.IsDead = true;
                    defenderCharacter.HealthRegen = false;
                    int roomID = game.connectedAccounts[defenderID].Account.Character.RoomID;
                    Functions.MovePlayer(0, game.connectedAccounts[defenderID], $"~r{defenderCharacter.Name.FirstCharToUpper()} appears suddenly!", $"~r{defenderCharacter.Name.FirstCharToUpper()} falls to the floor lifelessly!", game.connectedAccounts[defenderID]); ;
                    Functions.MessageDirect($"~w{attackerCharacter.BName} {attackerCharacter.Name}~m has killed you!", game.connectedAccounts[defenderID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name} ~mhas killed {defenderCharacter.Name.FirstCharToUpper()}~m!", roomID);
                    Functions.CharacterStatUpdate(game.connectedAccounts[defenderID]);
                    var respawnCommand = new RespawnCharacterServer
                    {
                        RoomID = defenderCharacter.Recall.ToString(),
                        EntityID = defenderID,
                        ArrivalMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gappears back in the world of the living following a bright white flash!",
                        DepartureMessage = $"~w{defenderCharacter.Name.FirstCharToUpper()} ~gvanishes back to the world of the living following a bright white flash!"
                    };
                    Functions.AddToQueue(DateTimeOffset.Now.ToUnixTimeMilliseconds() + 20000, respawnCommand, game.serverID.ToString());
                }
                else if (!defenderIsPlayer && killerIsPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    Character attackerCharacter = game.connectedAccounts[killerID].Account.Character;
                    game.currentNPC.Remove(defenderID);
                    game.rooms[defenderCharacter.RoomID].RoomNPC.Remove(defenderCharacter);
                    Functions.NPCUpdate(2, attackerCharacter.RoomID, defenderCharacter);
                    Functions.MessageDirect($"~mYou have killed ~w{defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}!", game.connectedAccounts[killerID].Client.Id);
                    Functions.MessageRoom($"~w{attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID, game.connectedAccounts[killerID]);
                }
                else if (!defenderIsPlayer && !killerIsPlayer)
                {
                    NPC defenderCharacter = game.currentNPC[defenderID];
                    NPC attackerCharacter = game.currentNPC[killerID];
                    game.currentNPC.Remove(defenderID);
                    game.rooms[defenderCharacter.RoomID].RoomNPC.Remove(defenderCharacter);
                    Functions.NPCUpdate(2, attackerCharacter.RoomID, defenderCharacter);
                    Functions.MessageRoom($"~w{attackerCharacter.BName} {attackerCharacter.Name.FirstCharToUpper()} ~mhas killed {defenderCharacter.BName} {defenderCharacter.Name.FirstCharToUpper()}~m!", attackerCharacter.RoomID);
                }
            }
            public static class Add
            {
                public static int CurrentHealth(string entityID, bool isPlayer, int health)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.CurrentHealth += health : -1;
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].CurrentHealth += health : -1;
                    }
                }
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
                public static bool IsAttacking(string entityID, bool isPlayer)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) && game.connectedAccounts[entityID].Account.Character.IsAttacking;
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) && game.currentNPC[entityID].IsAttacking;
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
                public static int TypeID(string entityID, bool isPlayer)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.TypeID : -1;
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].TypeID : -1;
                    }
                }
            }
            public static class Remove
            {
                public static int CurrentHealth(string entityID, bool isPlayer, int health)
                {
                    if (isPlayer)
                    {
                        return game.connectedAccounts.ContainsKey(entityID) ? game.connectedAccounts[entityID].Account.Character.CurrentHealth -= health : -1;
                    }
                    else
                    {
                        return game.currentNPC.ContainsKey(entityID) ? game.currentNPC[entityID].CurrentHealth -= health : -1;
                    }
                }
            }
            public static class Set
            {

            }
        }
        #endregion

        #region -- Lua General --
        [MoonSharpUserData]
        class LuaGeneral
        {

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
            public static void Drops(NPC npc, string killerID, bool killerIsPlayer)
            {
                if (killerIsPlayer)
                {
                    foreach (NPCItems nPCItems in npc.Drops)
                    {
                        if (LuaRandom.NumberDouble(0,1) < nPCItems.DropChance)
                        {
                            LuaMessage.Room(npc.RoomID, $"~w{npc.BName} {npc.Name} ~cdropped a ~w{game.items[nPCItems.ItemID].Name}~c!");
                        }
                    }
                }
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
                return Math.Abs(random.Next(min, max));
            }
            public static double NumberDouble(double min, double max)
            {
                return Math.Abs(random.NextDouble() * (min - max) + min);
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