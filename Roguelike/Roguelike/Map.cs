using BearLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike
{
    public class Map : RogueSharp.Map
    {
        private const int MinimumRoomSize = 6;
        private const int MaximumRoomSize = 10;
        private const int MaximumRooms = 30;

        public static Color WallDark { get; } = Color.FromArgb(0, 0, 100);
        public static Color FloorDark { get; } = Color.FromArgb(50, 50, 150);

        public Map(int width, int height)
            : base(width, height)
        {

        }

        public void Generate()
        {
            var random = Program.Random;
            var rooms = new List<Rectangle>();

            for (int i = 0; i < MaximumRooms; i++)
            {
                int roomWidth = random.Next(MinimumRoomSize, MaximumRoomSize + 1);
                int roomHeight = random.Next(MinimumRoomSize, MaximumRoomSize + 1);
                int roomX = random.Next(Width - roomWidth);
                int roomY = random.Next(Height - roomHeight);

                var room = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                if (!rooms.Any(room.Intersects))
                {
                    CreateRoom(room);

                    if (rooms.Count == 0)
                    {
                        Program.Player.X = room.Center.X;
                        Program.Player.Y = room.Center.Y;
                    }
                    else
                    {
                        var previousRoom = rooms.Last();

                        if (random.Next(2) == 0)
                        {
                            CreateHorizontalTunnel(previousRoom.Center.X, room.Center.X, previousRoom.Center.Y);
                            CreateVerticalTunnel(previousRoom.Center.Y, room.Center.Y, room.Center.X);
                        }
                        else
                        {
                            CreateHorizontalTunnel(previousRoom.Center.X, room.Center.X, room.Center.Y);
                            CreateVerticalTunnel(previousRoom.Center.Y, room.Center.Y, previousRoom.Center.X);
                        }
                    }

                    rooms.Add(room);
                }
            }
        }

        public void Draw()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (IsWalkable(x, y))
                    {
                        Terminal.BkColor(FloorDark);
                    }
                    else
                    {
                        Terminal.BkColor(WallDark);
                    }

                    Terminal.Put(x, y, 0x0020);
                }
            }
        }

        private void CreateRoom(Rectangle room)
        {
            for (int x = Math.Max(room.Left + 1, 1); x < Math.Min(room.Right, Width - 1); x++)
            {
                for (int y = Math.Max(room.Top + 1, 1); y < Math.Min(room.Bottom, Height - 1); y++)
                {
                    SetCellProperties(x, y, true, true);
                }
            }
        }

        private void CreateHorizontalTunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            {
                SetCellProperties(x, y, true, true);
            }
        }

        private void CreateVerticalTunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                SetCellProperties(x, y, true, true);
            }
        }
    }
}
