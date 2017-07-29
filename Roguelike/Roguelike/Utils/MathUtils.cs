using System;

namespace Roguelike.Utils
{
    public static class MathUtils
    {
        public static float Distance(int x1, int y1, int x2, int y2)
        {
            int dx = x2 - x1;
            int dy = y2 - y1;

            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
