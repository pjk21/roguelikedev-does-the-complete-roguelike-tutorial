using BearLib;
using System;
using System.Drawing;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike.World
{
    public class Map : RogueSharp.Map
    {
        public static Color WallDark { get; } = Color.FromArgb(0, 0, 100);
        public static Color FloorDark { get; } = Color.FromArgb(50, 50, 150);

        public Map(int width, int height)
            : base(width, height)
        {

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
    }
}
