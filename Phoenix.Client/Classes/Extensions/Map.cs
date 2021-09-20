using Phoenix.Common.Data.Types;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Phoenix.Client.Classes.Extensions
{
	public class Map : PictureBox
	{
		#region -- Fields --

		/// <summary>The current brushes, indexed by room type, used to draw the rooms.</summary>
		private readonly Dictionary<int, Brush> roomBrushes = new();

		/// <summary>The pen to draw the lines showing you can go that direction.</summary>
		private readonly Pen linePen = new(Brushes.White);

		private readonly Pen outlinePen = new(Brushes.LightSlateGray, 3);

		/// <summary>The list of current rooms that was most recently drawn.</summary>
		private Room[,] currentRooms = null;

		#endregion

		/// <summary>
		/// Constructor.
		/// </summary>
		public Map()
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.Black;

			this.roomBrushes.Add(0, Brushes.Black);
			this.roomBrushes.Add(1, Brushes.White);
			this.roomBrushes.Add(2, Brushes.Tan);
			this.roomBrushes.Add(3, Brushes.PeachPuff);
			this.roomBrushes.Add(4, Brushes.Moccasin);
			this.roomBrushes.Add(5, Brushes.BurlyWood);
			this.roomBrushes.Add(6, Brushes.Brown);
			this.roomBrushes.Add(7, Brushes.LawnGreen);

			this.Paint += Map_Paint;
		}

		/// <summary>
		/// Draws the rooms to the map.
		/// </summary>
		/// <param name="rooms">The rooms to draw.</param>
		public void DrawMap(Room[,] rooms)
		{
			if (rooms == null)
				return;

			this.currentRooms = rooms;
			this.Invalidate();
		}

		/// <summary>
		/// Fires when the app tells the map it needs to re-draw everything.
		/// </summary>
		/// <param name="sender">The sender of the event.</param>
		/// <param name="e">Contains the graphics object that is needed to draw the map.</param>
		private void Map_Paint(object sender, PaintEventArgs e)
		{
			var g = e.Graphics;
			g.Clear(this.BackColor);

			if (this.currentRooms == null)
				return;

			var roomGap = (int)(this.Size.Width * 0.05);
			var roomsWide = this.currentRooms.GetLength(0);
			var roomsHigh = this.currentRooms.GetLength(1);
			var gapsWide = roomsWide + 1;
			var gapsHigh = roomsHigh + 1;

			var roomWidth = (this.Size.Width - (roomGap * gapsWide)) / roomsWide;
			var roomHeight = (this.Size.Height - (roomGap * gapsHigh)) / roomsHigh;

			var baseX = roomGap;
			var baseY = roomGap;

			for (int y = 0; y < roomsHigh; y++)
			{
				for (int x = 0; x < roomsWide; x++)
				{
					var room = this.currentRooms[x, y];

					//don't draw nothing rooms
					if (room.Type < 1)
						continue;

					var roomBrush = this.roomBrushes[room.Type];

					var drawX = baseY + (roomWidth * y) + (roomGap * y);
					var drawY = baseX + (roomHeight * x) + (roomGap * x);

					g.FillRectangle(roomBrush, new Rectangle(drawX, drawY, roomWidth, roomHeight));
					g.DrawRectangle(outlinePen, new Rectangle(drawX, drawY, roomWidth, roomHeight));

					if (room.CanGoNorth)
					{
						var x1 = drawX + (roomWidth / 2);
						var y1 = drawY;
						var x2 = x1;
						var y2 = y1 - roomGap;
						g.DrawLine(this.linePen, x1, y1, x2, y2);
					}

					if (room.CanGoSouth)
					{
						var x1 = drawX + (roomWidth / 2);
						var y1 = drawY + roomHeight;
						var x2 = x1;
						var y2 = y1 + roomGap;
						g.DrawLine(this.linePen, x1, y1, x2, y2);
					}

					if (room.CanGoEast)
					{
						var x1 = drawX + roomWidth;
						var y1 = drawY + (roomHeight / 2);
						var x2 = x1 + roomGap;
						var y2 = y1;
						g.DrawLine(this.linePen, x1, y1, x2, y2);
					}

					if (room.CanGoWest)
					{
						var x1 = drawX;
						var y1 = drawY + (roomHeight / 2);
						var x2 = x1 - roomGap;
						var y2 = y1;
						g.DrawLine(this.linePen, x1, y1, x2, y2);
					}

					//draw player in the middle
					if (x == 2 && y == 2)
					{
						var roomPct = 0.50f;
						var width = (int)(roomWidth * roomPct);
						var height = (int)(roomHeight * roomPct);
						g.FillEllipse(Brushes.Red, drawX + (width / 2), drawY + (height / 2), width, height);
					}
				}
			}
		}

		/// <summary>
		/// If the control gets resized, this will be called.
		/// It will redraw the map.
		/// </summary>
		/// <param name="e">See MSDN. Unused here.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			this.Invalidate();
		}
	}
}
