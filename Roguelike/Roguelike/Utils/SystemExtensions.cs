using System;

namespace Roguelike
{
    public static class SystemExtensions
    {
        public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
            {
                return minimum;
            }

            if (value.CompareTo(maximum) > 0)
            {
                return maximum;
            }

            return value;
        }
    }
}
