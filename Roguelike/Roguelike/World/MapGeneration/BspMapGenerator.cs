using RogueSharp;
using System;

// BSP code adapted from: https://gamedevelopment.tutsplus.com/tutorials/how-to-use-bsp-trees-to-generate-game-maps--gamedev-12268

namespace Roguelike.World.MapGeneration
{
    public class BspMapGenerator : IMapGenerator
    {
        public int MinimumRoomSize { get; set; } = 7;

        public Map Generate(int width, int height)
        {
            var map = new Map(width, height);

            var entireMap = new Rectangle(0, 0, map.Width, map.Height);

            var root = new BspNode(null, entireMap);
            root.Split(MinimumRoomSize);

            root.CreateRoom(map, MinimumRoomSize);

            var spawnRoom = root.GetRoom();
            Program.Player.X = spawnRoom.Center.X;
            Program.Player.Y = spawnRoom.Center.Y;

            return map;
        }

        class BspNode
        {
            public BspNode Parent { get; }
            public BspNode ChildA { get; private set; }
            public BspNode ChildB { get; private set; }

            public Rectangle Size { get; set; }
            public Rectangle Room { get; set; }

            public BspNode(BspNode parent, Rectangle size)
            {
                Parent = parent;
                Size = size;
            }

            public void Split(int minimumRoomSize)
            {
                int direction = -1;

                if (Size.Width > Size.Height)
                {
                    direction = 0;
                }
                else if (Size.Height > Size.Width)
                {
                    direction = 1;
                }
                else
                {
                    direction = Program.Random.Next(2);
                }

                if (direction == 0)
                {
                    var childRoomWidth = (Size.Width / 2) + Program.Random.Next(-3, 4);

                    if (childRoomWidth >= minimumRoomSize && Size.Width - childRoomWidth >= minimumRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Size.Left, Size.Top, childRoomWidth, Size.Height));
                        ChildA.Split(minimumRoomSize);

                        ChildB = new BspNode(this, new Rectangle(Size.Left + childRoomWidth, Size.Top, Size.Width - childRoomWidth, Size.Height));
                        ChildB.Split(minimumRoomSize);
                    }
                }
                else
                {
                    var childRoomHeight = (Size.Height / 2) + Program.Random.Next(-3, 4);

                    if (childRoomHeight >= minimumRoomSize && Size.Height - childRoomHeight >= minimumRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Size.Left, Size.Top, Size.Width, childRoomHeight));
                        ChildA.Split(minimumRoomSize);

                        ChildB = new BspNode(this, new Rectangle(Size.Left, Size.Top + childRoomHeight, Size.Width, Size.Height - childRoomHeight));
                        ChildB.Split(minimumRoomSize);
                    }
                }
            }

            public void CreateRoom(Map map, int minimumRoomSize)
            {
                if (ChildA != null || ChildB != null)
                {
                    ChildA?.CreateRoom(map, minimumRoomSize);
                    ChildB?.CreateRoom(map, minimumRoomSize);

                    if (ChildA != null && ChildB != null)
                    {
                        var roomA = ChildA.GetRoom();
                        var roomB = ChildB.GetRoom();

                        if (roomA.Left == roomB.Left || roomA.Right == roomB.Right)
                        {
                            if (roomA.Width >= roomB.Width)
                            {
                                map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomB.Center.X);
                            }
                            else
                            {
                                map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomA.Center.X);
                            }
                        }
                        else if (roomA.Top == roomB.Top || roomA.Bottom == roomB.Bottom)
                        {
                            if (roomA.Height >= roomB.Height)
                            {
                                map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomB.Center.Y);
                            }
                            else
                            {
                                map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomA.Center.Y);
                            }
                        }
                        else
                        {
                            if (roomA.Width >= roomB.Width)
                            {
                                map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomB.Center.X);
                            }
                            else
                            {
                                map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomA.Center.X);
                            }

                            if (roomA.Height >= roomB.Height)
                            {
                                map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomB.Center.Y);
                            }
                            else
                            {
                                map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomA.Center.Y);
                            }
                        }
                    }
                }
                else
                {
                    Room = new Rectangle();

                    int maxShrinkX = Size.Width - minimumRoomSize;
                    int maxShrinkY = Size.Height - minimumRoomSize;

                    Room.Width = Size.Width - Program.Random.Next(maxShrinkX);
                    Room.Height = Size.Height - Program.Random.Next(maxShrinkY);

                    Room.X = Program.Random.Next(Size.Left, Size.Right - Room.Width);
                    Room.Y = Program.Random.Next(Size.Top, Size.Bottom - Room.Height);

                    map.CreateRoom(Room);
                }
            }

            public Rectangle GetRoom()
            {
                if (Room != null)
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
