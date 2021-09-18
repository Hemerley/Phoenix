using Phoenix.Common.Data;
using Phoenix.Server.Network;
using System;

namespace Phoenix.Server
{
	class Program
	{
        static void Main()
		{
			Console.Title = Constants.GAME_NAME + " Server - V" + Constants.GAME_VERSION;
			
			new Game().Start();

			while (true)
            {
				string input = Console.ReadLine();

                switch (input)
                {
					default:
						return;
                }
			}
        }
	}
}
