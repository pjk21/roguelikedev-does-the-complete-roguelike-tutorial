using Roguelike.Entities;
using RogueSharp;

namespace Roguelike.World.MapGeneration
{
    public abstract class MapGenerator : IMapGenerator
    {
        public int MaximumMonstersPerRoom { get; set; } = 3;

        public abstract Map Generate(int width, int height);

        protected virtual void SpawnMonsters(Rectangle room)
        {
            int numberOfMonsters = Program.Random.Next(MaximumMonstersPerRoom + 1);

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int x = Program.Random.Next(room.Left + 1, room.Right);
                int y = Program.Random.Next(room.Top + 1, room.Bottom);

                if (Program.Random.NextDouble() < 0.8)
                {
                    var rat = new Entity("Rat", x, y, 'r', Colours.Rat, true)
                    {
                        SpriteIndex = EntitySprites.Rat
                    };

                    Program.Entities.Add(rat);
                }
                else
                {
                    var hound = new Entity("Hound", x, y, 'h', Colours.Hound, true)
                    {
                        SpriteIndex = EntitySprites.Hound
                    };

                    Program.Entities.Add(hound);
                }
            }
        }
    }
}
