using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class RoomMapResponseCommand : Command
    {

		#region -- Properties --

		public bool Success { get; set; }

		public int RoomsWide { get; set; } = 5;

		public int RoomsHigh { get; set; } = 5;

		public List<Room> Rooms { get; set; } = new List<Room>();

		#endregion

		public RoomMapResponseCommand()
		{
			this.CommandType = CommandType.MapResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
            var payload = new List<List<string>>
            {
                new List<string> { (this.Success ? 1 : 0).ToString() },

                //This is just extra metadata JUST IN CASE you'd need it on the client
                new List<string> { this.RoomsWide.ToString(), this.RoomsHigh.ToString() } //5x5 map - TODO Make configurable if you want it to be
            };

            foreach (var room in this.Rooms)
			{
				payload.Add(new List<string>
			{
				room.Type.ToString(),
				(room.CanGoNorth ? 1 : 0).ToString(),
				(room.CanGoEast ? 1 : 0).ToString(),
				(room.CanGoSouth ? 1 : 0).ToString(),
				(room.CanGoWest ? 1 : 0).ToString()
			});
			}

			return payload;
		}

		public Room[,] To2DArray()
		{
			var rooms = new Room[this.RoomsWide, this.RoomsHigh];

			for (int x = 0; x < this.RoomsWide; x++)
			{
				for (int y = 0; y < this.RoomsHigh; y++)
				{
					var roomIdx = x * this.RoomsHigh + y;
					var room = roomIdx < this.Rooms.Count ? this.Rooms[roomIdx] : new Room();
					rooms[x, y] = room;
				}
			}

			return rooms;
		}
	}
}
