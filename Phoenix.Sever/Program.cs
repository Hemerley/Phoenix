using Phoenix.Common.Data;
using Phoenix.Server.Bot;
using Phoenix.Server.Network;
using Serilog;
using System;

namespace Phoenix.Server
{
    class Program
    {
        public static Game game = new();
        public static Discord bot = new();
        public static double uptime;

        static void Main()
        {
            Console.Title = Constants.GAME_NAME + " Server - V" + Constants.GAME_VERSION;
            uptime = DateTimeOffset.Now.ToUnixTimeSeconds();

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .WriteTo.Console()
               .WriteTo.File("logs/server/console-output.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();
            bot.RunAsync().GetAwaiter().GetResult();
            game.Start();

            while (true)
            {
                string input = Console.ReadLine();

                switch (input)
                {
                    case "/showcommands":
                        game.showCommands = !game.showCommands;
                        Log.Information($"Command Status: {game.showCommands}");
                        continue;
                    default:
                        continue;
                }
            }
        }
    }
}
