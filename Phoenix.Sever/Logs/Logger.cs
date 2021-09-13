using System;

namespace Phoenix.Server
{
    public class Logger
    {
        public static void ConsoleLog(string type, string msg)
        {
            switch (type)
            {
                case "Debug":

                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                case "System":

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                case "Command":

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                case "Error":

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                case "Connection":

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                case "Database":

                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[{DateTime.Now}][{type}]: {msg}");
                    Console.ResetColor();
                    return;

                default:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{DateTime.Now}][Unknown]: Unknown Console Write Request.");
                    Console.ResetColor();
                    return;
            }
        }
    }
}
