using System.Drawing;

namespace Roguelike
{
    public static class ColorExtensions
    {
        public static Color Lerp(this Color color, Color other, float t = 0.5f)
        {
            var r = (int)(color.R + (other.R - color.R) * t);
            var g = (int)(color.G + (other.G - color.G) * t);
            var b = (int)(color.B + (other.B - color.B) * t);

            return Color.FromArgb(r, g, b);
        }
    }
}
