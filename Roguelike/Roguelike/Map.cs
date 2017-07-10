using BearLib;
using System.Drawing;

namespace Roguelike
{
    public class Map : RogueSharp.Map
    {
        public static Color WallDark { get; } = Color.FromArgb(0, 0, 100);
        public static Color FloorDark { get; } = Color.FromArgb(50, 50, 150);

        public Map(int width, int height)
            : base(width, height)
        {

        }

        public void Generate()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    SetCellProperties(x, y, true, true);
                }
            }

            SetCellProperties(30, 22, false, false);
            SetCellProperties(50, 22, false, false);
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
    }
}
