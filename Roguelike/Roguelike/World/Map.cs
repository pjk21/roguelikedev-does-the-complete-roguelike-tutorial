using System;
using System.Linq;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike.World
{
    public class Map : RogueSharp.Map
    {
        public Map(int width, int height)
            : base(width, height)
        {

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
            if (!IsWalkable(x, y) || Program.Entities.Any(e => e.IsSolid && e.X == x && e.Y == y))
            {
                return false;
            }

            return true;
        }
    }
}
