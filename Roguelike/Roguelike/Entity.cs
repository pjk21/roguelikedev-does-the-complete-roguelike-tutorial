using BearLib;
using System.Drawing;

namespace Roguelike
{
    public class Entity
    {
        public int X { get; set; }
        public int Y { get; set; }

        public char Character { get; set; }
        public Color Colour { get; set; } = Color.White;

        public Entity(int x, int y, char character, Color colour)
        {
            X = x;
            Y = y;

            Character = character;
            Colour = colour;
        }

        public void Move(int x, int y)
        {
            if (Program.Map.IsWalkable(X + x, Y + y))
            {
                X += x;
                Y += y;
            }
        }

        public void Draw()
        {
            Terminal.Color(Colour);
            Terminal.Put(X, Y, Character);
        }
    }
}
