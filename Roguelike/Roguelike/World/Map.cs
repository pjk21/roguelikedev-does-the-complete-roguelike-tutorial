using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike.World
{
    public class Map : RogueSharp.Map
    {
        public RogueSharp.Map PathfindingMap { get; }

        public Map(int width, int height)
            : base(width, height)
        {
            PathfindingMap = new RogueSharp.Map(Width, Height);
        }

        public void CreateRoom(Rectangle room)
        {
            for (int x = Math.Max(room.Left + 1, 1); x < Math.Min(room.Right, Width - 1); x++)
            {
                for (int y = Math.Max(room.Top + 1, 1); y < Math.Min(room.Bottom, Height - 1); y++)
                {
                    SetCellProperties(x, y, true, true);
                }
            }
        }

        public void CreateHorizontalTunnel(int x1, int x2, int y)
        {
            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            {
                SetCellProperties(x, y, true, true);
            }
        }

        public void CreateVerticalTunnel(int y1, int y2, int x)
        {
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                SetCellProperties(x, y, true, true);
            }
        }

        public bool CanEnter(int x, int y)
        {
            if (!IsWalkable(x, y) || Program.Game.Entities.Any(e => e.IsSolid && e.X == x && e.Y == y))
            {
                return false;
            }

            return true;
        }

        public byte[] Serialize()
        {
            byte[] data;

            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            writer.Write(IsWalkable(x, y));
                            writer.Write(IsTransparent(x, y));
                            writer.Write(IsExplored(x, y));
                        }
                    }
                }

                data = stream.ToArray();
            }

            return data;
        }

        public void Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    for (int x = 0; x < Width; x++)
                    {
                        for (int y = 0; y < Height; y++)
                        {
                            var walkable = reader.ReadBoolean();
                            var transparent = reader.ReadBoolean();
                            var explored = reader.ReadBoolean();

                            SetCellProperties(x, y, transparent, walkable, explored);
                        }
                    }
                }
            }
        }
    }
}
