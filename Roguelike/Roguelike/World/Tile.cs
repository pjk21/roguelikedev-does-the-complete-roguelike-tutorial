using System.Drawing;

namespace Roguelike.World
{
    public class Tile
    {
        private static int lastId = 0;

        public static Tile[] Tiles { get; } = new Tile[2];

        public static Tile Wall { get; }
        public static Tile Floor { get; }

        public int Id { get; }
        public string Name { get; }

        public float MoveCost { get; }
        public bool IsTransparent { get; }

        public int? Glyph { get; }
        public Color? Colour { get; }
        public Color? BackColour { get; }

        public int SpriteIndex { get; }
        public Color SpriteTint { get; }
        public bool IsAutoTile { get; }

        public Tile(string name, float moveCost, bool transparent, int? glyph, Color? colour, Color? backColour, int spriteIndex, Color? spriteTint, bool autoTile)
        {
            Id = lastId++;
            Name = name;

            MoveCost = moveCost;
            IsTransparent = transparent;

            Glyph = glyph;
            Colour = colour;
            BackColour = backColour;

            if (Glyph == null && BackColour != null)
            {
                Glyph = 0x0020;
            }

            SpriteIndex = spriteIndex;
            SpriteTint = spriteTint ?? Color.White;
            IsAutoTile = autoTile;

            Tiles[Id] = this;
        }

        public int GetSpriteIndex(Map map, int x, int y)
        {
            if (!IsAutoTile)
            {
                return SpriteIndex;
            }

            int index = 0;

            if (y > 0 && !map.IsWalkable(x, y - 1))
            {
                index += 1;
            }

            if (x < map.Width - 1 && !map.IsWalkable(x + 1, y))
            {
                index += 2;
            }

            if (y < map.Height - 1 && !map.IsWalkable(x, y + 1))
            {
                index += 4;
            }

            if (x > 0 && !map.IsWalkable(x - 1, y))
            {
                index += 8;
            }

            return SpriteIndex + index;
        }

        static Tile()
        {
            Wall = new Tile("Wall", 0f, false, 0x2591, Color.DimGray, Color.FromArgb(78, 74, 78), TileSprites.Wall, Color.White, true);
            Floor = new Tile("Floor", 1f, true, null, null, Color.FromArgb(133, 76, 48), TileSprites.Floor, Color.White, false);
        }
    }
}
