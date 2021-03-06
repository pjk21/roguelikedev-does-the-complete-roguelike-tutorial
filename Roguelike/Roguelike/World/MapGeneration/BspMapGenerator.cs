﻿using Roguelike.Entities;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;

// BSP code adapted from: https://gamedevelopment.tutsplus.com/tutorials/how-to-use-bsp-trees-to-generate-game-maps--gamedev-12268

namespace Roguelike.World.MapGeneration
{
    public class BspMapGenerator : MapGenerator
    {
        private List<Rectangle> rooms;

        public int MinimumRoomSize { get; set; } = 7;

        public override Map Generate(int width, int height)
        {
            var map = new Map(width, height);
            rooms = new List<Rectangle>();

            var entireMap = new Rectangle(0, 0, map.Width, map.Height);

            var root = new BspNode(null, entireMap);
            root.Split(MinimumRoomSize);

            CreateRooms(root, map);

            var spawnRoom = root.GetRoom();
            Program.Game.Player.X = spawnRoom.Center.X;
            Program.Game.Player.Y = spawnRoom.Center.Y;

            var stairsRoom = rooms.Last();

            var stairs = ItemFactory.CreateStairs(stairsRoom.Center.X, stairsRoom.Center.Y, Program.Game.DungeonLevel);
            Program.Game.Entities.Add(stairs);

            foreach (var room in rooms)
            {
                // Don't spawn monsters in the player's room.
                if (room != spawnRoom)
                {
                    SpawnMonsters(room);
                }

                SpawnItems(map, room);
            }

            return map;
        }

        private void CreateRooms(BspNode node, Map map)
        {
            if (node.ChildA != null || node.ChildB != null)
            {
                CreateRooms(node.ChildA, map);
                CreateRooms(node.ChildB, map);

                if (node.ChildA != null && node.ChildB != null)
                {
                    var roomA = node.ChildA.GetRoom();
                    var roomB = node.ChildB.GetRoom();

                    if (roomA.Left == roomB.Left || roomA.Right == roomB.Right)
                    {
                        if (roomA.Width >= roomB.Width)
                        {
                            map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomB.Center.X, Tile.Floor);
                        }
                        else
                        {
                            map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomA.Center.X, Tile.Floor);
                        }
                    }
                    else if (roomA.Top == roomB.Top || roomA.Bottom == roomB.Bottom)
                    {
                        if (roomA.Height >= roomB.Height)
                        {
                            map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomB.Center.Y, Tile.Floor);
                        }
                        else
                        {
                            map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomA.Center.Y, Tile.Floor);
                        }
                    }
                    else
                    {
                        if (roomA.Width >= roomB.Width)
                        {
                            map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomB.Center.X, Tile.Floor);
                        }
                        else
                        {
                            map.CreateVerticalTunnel(roomA.Center.Y, roomB.Center.Y, roomA.Center.X, Tile.Floor);
                        }

                        if (roomA.Height >= roomB.Height)
                        {
                            map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomB.Center.Y, Tile.Floor);
                        }
                        else
                        {
                            map.CreateHorizontalTunnel(roomA.Center.X, roomB.Center.X, roomA.Center.Y, Tile.Floor);
                        }
                    }
                }
            }
            else
            {
                var room = new Rectangle();

                int maxShrinkX = node.Size.Width - MinimumRoomSize;
                int maxShrinkY = node.Size.Height - MinimumRoomSize;

                room.Width = node.Size.Width - Program.Game.Random.Next(maxShrinkX);
                room.Height = node.Size.Height - Program.Game.Random.Next(maxShrinkY);

                room.X = Program.Game.Random.Next(node.Size.Left, node.Size.Right - room.Width);
                room.Y = Program.Game.Random.Next(node.Size.Top, node.Size.Bottom - room.Height);

                node.Room = room;
                rooms.Add(room);

                map.CreateRoom(room, Tile.Floor);
            }
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
                    direction = Program.Game.Random.Next(2);
                }

                if (direction == 0)
                {
                    var childRoomWidth = (Size.Width / 2) + Program.Game.Random.Next(-3, 4);

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
                    var childRoomHeight = (Size.Height / 2) + Program.Game.Random.Next(-3, 4);

                    if (childRoomHeight >= minimumRoomSize && Size.Height - childRoomHeight >= minimumRoomSize)
                    {
                        ChildA = new BspNode(this, new Rectangle(Size.Left, Size.Top, Size.Width, childRoomHeight));
                        ChildA.Split(minimumRoomSize);

                        ChildB = new BspNode(this, new Rectangle(Size.Left, Size.Top + childRoomHeight, Size.Width, Size.Height - childRoomHeight));
                        ChildB.Split(minimumRoomSize);
                    }
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
                        if (Program.Game.Random.Next(2) == 0)
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
