using RogueSharp;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.World.MapGeneration
{
    public class RoomsMapGenerator : IMapGenerator
    {
        public int MinimumRoomSize { get; set; } = 6;
        public int MaximumRoomSize { get; set; } = 10;
        public int MaximumRooms { get; set; } = 30;

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);

            var random = Program.Random;
            var rooms = new List<Rectangle>();

            for (int i = 0; i < MaximumRooms; i++)
            {
                int roomWidth = random.Next(MinimumRoomSize, MaximumRoomSize + 1);
                int roomHeight = random.Next(MinimumRoomSize, MaximumRoomSize + 1);
                int roomX = random.Next(map.Width - roomWidth);
                int roomY = random.Next(map.Height - roomHeight);

                var room = new Rectangle(roomX, roomY, roomWidth, roomHeight);

                if (!rooms.Any(room.Intersects))
                {
                    map.CreateRoom(room);

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
                            map.CreateHorizontalTunnel(previousRoom.Center.X, room.Center.X, previousRoom.Center.Y);
                            map.CreateVerticalTunnel(previousRoom.Center.Y, room.Center.Y, room.Center.X);
                        }
                        else
                        {
                            map.CreateHorizontalTunnel(previousRoom.Center.X, room.Center.X, room.Center.Y);
                            map.CreateVerticalTunnel(previousRoom.Center.Y, room.Center.Y, previousRoom.Center.X);
                        }
                    }

                    rooms.Add(room);
                }
            }

            return map;
        }
    }
}
