using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using static Phoenix.Server.Program;
using System.Collections.Generic;
using System.Linq;
using Phoenix.Common.Commands.Factory;

namespace Phoenix.Server.Functions
{
    public class ToolFunctions
    {
		/// <summary>
		/// Returns a Client ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static ConnectedClient GetClientById(string id)
		{
			if (game.connectedClients.ContainsKey(id))
				return game.connectedClients[id];

			var connectedAccount = game.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			if (connectedAccount == null)
			{
				return null;
			}
			return connectedAccount.Client;
		}

		/// <summary>
		/// Returns an Account ID.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public static ConnectedAccount GetConnectedAccount(string id)
		{
			var connectedAccount = game.connectedAccounts.FirstOrDefault(c => c.Client.Id == id);
			if (connectedAccount == null)
			{
				return null;
			}
			return connectedAccount;
		}

		/// <summary>
		/// Returns Connected Rooms for Map Draw, But Better.
		/// </summary>
		/// <param name="room"></param>
		/// <returns></returns>
		public static List<Room> FindRooms(Room room)
		{
			List<Room> roomList = new();
			int[,] grid = new int[5, 5];
			grid[2, 2] = room.ID;
			for (int x = 2; x < 5; x++)
			{
				for (int y = 2; y < 5; y++)
				{
					if (game.rooms[grid[x, y]].CanGoNorth)
					{
						grid[x - 1, y] = game.rooms[grid[x, y]].North;
						if (game.rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = game.rooms[grid[x - 1, y]].North;
					}
					if (game.rooms[grid[x, y]].CanGoSouth)
					{
						grid[x + 1, y] = game.rooms[grid[x, y]].South;
						if (game.rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = game.rooms[grid[x + 1, y]].South;

					}
					if (game.rooms[grid[x, y]].CanGoWest)
					{
						grid[x, y - 1] = game.rooms[grid[x, y]].West;
						if (game.rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = game.rooms[grid[x, y - 1]].West;

					}
					if (game.rooms[grid[x, y]].CanGoEast)
					{
						grid[x, y + 1] = game.rooms[grid[x, y]].East;
						if (game.rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = game.rooms[grid[x, y + 1]].East;
					}
				}
			}
			for (int x = 2; x > -1; x--)
			{
				for (int y = 2; y > -1; y--)
				{
					if (game.rooms[grid[x, y]].CanGoNorth)
					{
						grid[x - 1, y] = game.rooms[grid[x, y]].North;
						if (game.rooms[grid[x - 1, y]].CanGoNorth)
							grid[x - 2, y] = game.rooms[grid[x - 1, y]].North;
					}
					if (game.rooms[grid[x, y]].CanGoSouth)
					{
						grid[x + 1, y] = game.rooms[grid[x, y]].South;
						if (game.rooms[grid[x + 1, y]].CanGoSouth)
							grid[x + 2, y] = game.rooms[grid[x + 1, y]].South;

					}
					if (game.rooms[grid[x, y]].CanGoWest)
					{
						grid[x, y - 1] = game.rooms[grid[x, y]].West;
						if (game.rooms[grid[x, y - 1]].CanGoWest)
							grid[x, y - 2] = game.rooms[grid[x, y - 1]].West;

					}
					if (game.rooms[grid[x, y]].CanGoEast)
					{
						grid[x, y + 1] = game.rooms[grid[x, y]].East;
						if (game.rooms[grid[x, y + 1]].CanGoEast)
							grid[x, y + 2] = game.rooms[grid[x, y + 1]].East;
					}
				}
			}
			foreach (int gridLoc in grid)
			{
				roomList.Add(game.rooms[gridLoc]);
			}

			return roomList;
		}

		/// <summary>
		/// Added Queues to the Queue Library & ConcurrentQueue.
		/// </summary>
		/// <param name="queue"></param>
		/// <param name="currentTimeStamp"></param>
		/// <param name="command"></param>
		/// <param name="uid"></param>
		public static void AddToQueue(bool queue, double currentTimeStamp, Command command = null, string uid = "")
		{
			if (queue)
			{
				if (game.actionQueue.ContainsKey(currentTimeStamp))
				{
					foreach (ClientCommand clientCommand in game.actionQueue[currentTimeStamp])
					{
						game.queuedCommand.Enqueue(clientCommand);
					}
				}
			}
			else
			{
				if (game.actionQueue.ContainsKey(currentTimeStamp))
				{
					game.actionQueue[currentTimeStamp].Add(new ClientCommand
					{
						Id = uid,
						Command = command
					});
				}
				else
				{
					game.actionQueue.Add(currentTimeStamp, new List<ClientCommand>
					{
						new ClientCommand
						{
							Id = uid,
							Command = command
						}
					});
				}
			}
		}
	}
}
