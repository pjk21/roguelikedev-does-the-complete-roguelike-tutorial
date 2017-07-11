using System.Drawing;

namespace Roguelike
{
    public static class Colours
    {
        public static Color WallDark { get; } = Color.FromArgb(0, 0, 100);
        public static Color WallLight { get; } = Color.FromArgb(130, 110, 50);
        public static Color FloorDark { get; } = Color.FromArgb(50, 50, 150);
        public static Color FloorLight { get; } = Color.FromArgb(200, 180, 50);

        public static Color Rat { get; } = Color.SaddleBrown;
        public static Color Hound { get; } = Color.SaddleBrown;
    }
}
