using MoonSharp.Interpreter;
using Serilog;
using System;

namespace Phoenix.Server.Scripts
{
    class ScriptEngine
    {

        public static bool Movement()
        {
            try
            {
                DynValue result = Script.RunFile("./Scripts/Main/Movement.lua");
                return result.Boolean;
            }
            catch(Exception _ex)
            {
                Log.Error(_ex, "Error attempting player movement script.");
            }
            return false;
        }

    }
}
