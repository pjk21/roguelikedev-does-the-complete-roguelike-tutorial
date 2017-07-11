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

            root.CreateRoom(map.CreateRoom, map.CreateHorizontalTunnel, map.CreateVerticalTunnel);

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

            public Rectangle Room { get; set; }
            public bool IsRoomCreated { get; set; }

            public BspNode(BspNode parent, Rectangle room)
            {
                Parent = parent;
                Room = room;
            }

            public void Split(int minimumRoomSize)
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

                    if (childRoomWidth >= minimumRoomSize && Room.Width - childRoomWidth >= minimumRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Room.Left, Room.Top, childRoomWidth, Room.Height));
                        ChildA.Split(minimumRoomSize);

                        ChildB = new BspNode(this, new Rectangle(Room.Left + childRoomWidth, Room.Top, Room.Width - childRoomWidth, Room.Height));
                        ChildB.Split(minimumRoomSize);
                    }
                }
                else
                {
                    var childRoomHeight = (Room.Height / 2) + Program.Random.Next(-3, 4);

                    if (childRoomHeight >= minimumRoomSize && Room.Height - childRoomHeight >= minimumRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Room.Left, Room.Top, Room.Width, childRoomHeight));
                        ChildA.Split(minimumRoomSize);

                        ChildB = new BspNode(this, new Rectangle(Room.Left, Room.Top + childRoomHeight, Room.Width, Room.Height - childRoomHeight));
                        ChildB.Split(minimumRoomSize);
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
