using Roguelike.Entities;
using Rectangle = RogueSharp.Rectangle;

namespace Roguelike.World.MapGeneration
{
    public abstract class MapGenerator : IMapGenerator
    {
        public int MaximumMonstersPerRoom { get; set; } = 3;
        public int MaximumItemsPerRoom { get; set; } = 2;

        public abstract Map Generate(int width, int height);

        protected virtual void SpawnMonsters(Rectangle room)
        {
            int numberOfMonsters = Program.Game.Random.Next(MaximumMonstersPerRoom + 1);

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                if (Program.Game.Random.NextDouble() < 0.8)
                {
                    var rat = MonsterFactory.CreateRat(x, y);
                    Program.Game.Entities.Add(rat);
                }
                else
                {
                    var hound = MonsterFactory.CreateHound(x, y);
                    Program.Game.Entities.Add(hound);
                }
            }
        }

        protected virtual void SpawnItems(Map map, Rectangle room)
        {
            int numberOfItems = Program.Game.Random.Next(MaximumItemsPerRoom + 1);

            for (int i = 0; i < numberOfItems; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                if (map.IsWalkable(x, y))
                {
                    var itemChance = Program.Game.Random.NextDouble();

                    if (itemChance < 0.7)
                    {
                        var potion = ItemFactory.CreatePotion(x, y);
                        Program.Game.Entities.Add(potion);
                    }
                    else if (itemChance < 0.8)
                    {
                        var lightningScroll = ItemFactory.CreateLightningScroll(x, y);
                        Program.Game.Entities.Add(lightningScroll);
                    }
                    else if (itemChance < 0.9)
                    {
                        var confuseScroll = ItemFactory.CreateConfuseScroll(x, y);
                        Program.Game.Entities.Add(confuseScroll);
                    }
                    else
                    {
                        var fireballScroll = ItemFactory.CreateFireballScroll(x, y);
                        Program.Game.Entities.Add(fireballScroll);
                    }
                }
            }
        }
    }
}
