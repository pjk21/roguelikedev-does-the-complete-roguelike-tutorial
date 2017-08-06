using System;
using System.Linq;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike.World
{
    [Serializable]
    public class Map
    {
        private readonly int[,] tiles;
        private readonly bool[,] walkable;
        private readonly bool[,] transparent;
        private readonly bool[,] explored;

        [NonSerialized]
        private RogueSharp.Map fovMap;

        public int Width { get; }
        public int Height { get; }

        public RogueSharp.Map FovMap
        {
            get { return fovMap; }
        }

        public Map(int width, int height)
        {
            Width = width;
            Height = height;

            tiles = new int[Width, Height];
            walkable = new bool[Width, Height];
            transparent = new bool[Width, Height];
            explored = new bool[Width, Height];

            fovMap = new RogueSharp.Map(Width, Height);
        }

        public bool IsWalkable(int x, int y) => walkable[x, y];
        public bool IsTransparent(int x, int y) => transparent[x, y];
        public bool IsExplored(int x, int y) => explored[x, y];

        public void SetExplored(int x, int y, bool explored)
        {
            this.explored[x, y] = explored;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            tiles[x, y] = tile.Id;

            transparent[x, y] = tile.IsTransparent;
            walkable[x, y] = tile.MoveCost > 0f;
        }

        public Tile GetTile(int x, int y)
        {
            var id = tiles[x, y];
            return Tile.Tiles[id];
        }

        public void CreateRoom(Rectangle room, Tile tile)
        {
            for (int x = Math.Max(room.Left + 1, 1); x < Math.Min(room.Right, Width - 1); x++)
            {
                for (int y = Math.Max(room.Top + 1, 1); y < Math.Min(room.Bottom, Height - 1); y++)
                {
                    SetTile(x, y, tile);
                }
            }
        }

        public void CreateHorizontalTunnel(int x1, int x2, int y, Tile tile)
        {
            for (int x = Math.Min(x1, x2); x <= Math.Max(x1, x2); x++)
            {
                SetTile(x, y, tile);
            }
        }

        public void CreateVerticalTunnel(int y1, int y2, int x, Tile tile)
        {
            for (int y = Math.Min(y1, y2); y <= Math.Max(y1, y2); y++)
            {
                SetTile(x, y, tile);
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

        public void UpdateFovMap()
        {
            if (fovMap == null)
            {
                fovMap = new RogueSharp.Map(Width, Height);
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    FovMap.SetCellProperties(x, y, IsTransparent(x, y), IsWalkable(x, y));
                }
            }
        }
    }
}
