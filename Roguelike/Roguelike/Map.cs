using BearLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Rectangle = RogueSharp.Rectangle;

// BSP code adapted from: https://gamedevelopment.tutsplus.com/tutorials/how-to-use-bsp-trees-to-generate-game-maps--gamedev-12268

namespace Roguelike
{
    public class Map : RogueSharp.Map
    {
        private const int MinimumRoomSize = 6;
        private const int MaximumRoomSize = 10;
        private const int MaximumRooms = 30;

        private const int MinimumBspRoomSize = 7;

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

        public void GenerateBsp()
        {
            var entireMap = new Rectangle(0, 0, Width, Height);

            var root = new BspNode(null, entireMap);
            root.Split();

            root.CreateRoom(CreateRoom, CreateHorizontalTunnel, CreateVerticalTunnel);
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

        class BspNode
        {
            public BspNode Parent { get; }
            public BspNode ChildA { get; private set; }
            public BspNode ChildB { get; private set; }

            public Rectangle Room { get; set; }
            public bool IsRoomCreated { get; set; }

            public BspNode(BspNode parent, Rectangle room)
            {
                Parent = parent;
                Room = room;
            }

            public void Split()
            {
                int direction = -1;

                if (Room.Width > Room.Height)
                {
                    direction = 0;
                }
                else if (Room.Height > Room.Width)
                {
                    direction = 1;
                }
                else
                {
                    direction = Program.Random.Next(2);
                }

                if (direction == 0)
                {
                    var childRoomWidth = (Room.Width / 2) + Program.Random.Next(-3, 4);

                    if (childRoomWidth >= MinimumBspRoomSize && Room.Width - childRoomWidth >= MinimumBspRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Room.Left, Room.Top, childRoomWidth, Room.Height));
                        ChildA.Split();

                        ChildB = new BspNode(this, new Rectangle(Room.Left + childRoomWidth, Room.Top, Room.Width - childRoomWidth, Room.Height));
                        ChildB.Split();
                    }
                }
                else
                {
                    var childRoomHeight = (Room.Height / 2) + Program.Random.Next(-3, 4);

                    if (childRoomHeight >= MinimumBspRoomSize && Room.Height - childRoomHeight >= MinimumBspRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Room.Left, Room.Top, Room.Width, childRoomHeight));
                        ChildA.Split();

                        ChildB = new BspNode(this, new Rectangle(Room.Left, Room.Top + childRoomHeight, Room.Width, Room.Height - childRoomHeight));
                        ChildB.Split();
                    }
                }
            }

            public void CreateRoom(Action<Rectangle> createRoomFunction, Action<int, int, int> horizontalTunnelFunction, Action<int, int, int> verticalTunnelFunction)
            {
                if (ChildA != null || ChildB != null)
                {
                    ChildA?.CreateRoom(createRoomFunction, horizontalTunnelFunction, verticalTunnelFunction);
                    ChildB?.CreateRoom(createRoomFunction, horizontalTunnelFunction, verticalTunnelFunction);

                    if (ChildA != null && ChildB != null)
                    {
                        var roomA = ChildA.GetRoom();
                        var roomB = ChildB.GetRoom();

                        if (roomA.Left == roomB.Left || roomA.Right == roomB.Right)
                        {
                            if (roomA.Width >= roomB.Width)
                            {
                                verticalTunnelFunction(roomA.Center.Y, roomB.Center.Y, roomB.Center.X);
                            }
                            else
                            {
                                verticalTunnelFunction(roomA.Center.Y, roomB.Center.Y, roomA.Center.X);
                            }
                        }
                        else if (roomA.Top == roomB.Top || roomA.Bottom == roomB.Bottom)
                        {
                            if (roomA.Height >= roomB.Height)
                            {
                                horizontalTunnelFunction(roomA.Center.X, roomB.Center.X, roomB.Center.Y);
                            }
                            else
                            {
                                horizontalTunnelFunction(roomA.Center.X, roomB.Center.X, roomA.Center.Y);
                            }
                        }
                        else
                        {
                            if (roomA.Width >= roomB.Width)
                            {
                                verticalTunnelFunction(roomA.Center.Y, roomB.Center.Y, roomB.Center.X);
                            }
                            else
                            {
                                verticalTunnelFunction(roomA.Center.Y, roomB.Center.Y, roomA.Center.X);
                            }

                            if (roomA.Height >= roomB.Height)
                            {
                                horizontalTunnelFunction(roomA.Center.X, roomB.Center.X, roomB.Center.Y);
                            }
                            else
                            {
                                horizontalTunnelFunction(roomA.Center.X, roomB.Center.X, roomA.Center.Y);
                            }
                        }
                    }
                }
                else
                {
                    IsRoomCreated = true;
                    createRoomFunction(Room);
                }
            }

            public Rectangle GetRoom()
            {
                if (IsRoomCreated)
                {
                    return Room;
                }
                else
                {
                    if (ChildA != null && ChildB == null)
                    {
                        return ChildA.GetRoom();
                    }
                    else if (ChildA == null && ChildB != null)
                    {
                        return ChildB.GetRoom();
                    }
                    else
                    {
                        if (Program.Random.Next(2) == 0)
                        {
                            return ChildA.GetRoom();
                        }
                        else
                        {
                            return ChildB.GetRoom();
                        }
                    }
                }
            }
        }
    }
}
