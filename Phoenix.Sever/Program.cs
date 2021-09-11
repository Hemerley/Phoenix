using Phoenix.Common;
using System;
using System.Net;

namespace Phoenix.Server
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Title = Constants.GAME_NAME + " Server - V" + Constants.GAME_VERSION;
			
			new Game().Start();

			// Get Under Input
			string input = Console.ReadLine();
		}
	}
}
