using System.Drawing;

namespace Roguelike
{
    public static class Colours
    {
        public static Color WallDark { get; } = Color.FromArgb(39, 37, 39);
        public static Color WallLight { get; } = Color.FromArgb(78, 74, 78);
        public static Color FloorDark { get; } = Color.FromArgb(66, 38, 24);
        public static Color FloorLight { get; } = Color.FromArgb(133, 76, 48);

        public static Color Player { get; } = Color.FromArgb(222, 238, 214);

        public static Color Rat { get; } = Color.FromArgb(66, 38, 24);
        public static Color Hound { get; } = Color.FromArgb(66, 38, 24);
    }
}
