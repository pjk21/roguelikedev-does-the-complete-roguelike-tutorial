using RogueSharp;
using System;

namespace Roguelike
{
    public static class RandomExtensions
    {
        public static Point GetPoint(this Random random, int minX, int maxX, int minY, int maxY)
        {
            var x = random.Next(minX, maxX);
            var y = random.Next(minY, maxY);

            return new Point(x, y);
        }
    }
}
