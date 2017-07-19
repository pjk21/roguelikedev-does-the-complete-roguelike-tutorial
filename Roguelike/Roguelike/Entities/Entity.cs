using System.Drawing;

namespace Roguelike.Entities
{
    public class Entity
    {
        public const int PlayerFovRadius = 10;

        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public char Character { get; set; }
        public Color Colour { get; set; } = Color.White;

        public int? SpriteIndex { get; set; }
        public Color SpriteTint { get; set; } = Color.White;

        public bool IsSolid { get; set; } = false;

        public Entity(string name, int x, int y, char character, Color colour, bool solid = false)
        {
            Name = name;

            X = x;
            Y = y;

            Character = character;
            Colour = colour;

            IsSolid = solid;
        }

        public bool Move(int x, int y)
        {
            if (Program.Map.CanEnter(X + x, Y + y))
            {
                X += x;
                Y += y;

                return true;
            }

            return false;
        }
    }
}
