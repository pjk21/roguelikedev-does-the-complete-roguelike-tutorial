using Roguelike.Entities;
using Roguelike.Utils;
using System;
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

            var monsterPool = new WeightedPool<Func<int, int, Entity>>();
            monsterPool.Add(MonsterFactory.CreateRat, 80);
            monsterPool.Add(MonsterFactory.CreateHound, 20);

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                var monsterFactory = monsterPool.Pick();
                var monster = monsterFactory.Invoke(x, y);
                Program.Game.Entities.Add(monster);
            }
        }

        protected virtual void SpawnItems(Map map, Rectangle room)
        {
            int numberOfItems = Program.Game.Random.Next(MaximumItemsPerRoom + 1);

            var itemPool = new WeightedPool<Func<int, int, Entity>>();
            itemPool.Add(ItemFactory.CreatePotion, 70);
            itemPool.Add(ItemFactory.CreateLightningScroll, 10);
            itemPool.Add(ItemFactory.CreateConfuseScroll, 10);
            itemPool.Add(ItemFactory.CreateFireballScroll, 10);

            for (int i = 0; i < numberOfItems; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                if (map.IsWalkable(x, y))
                {
                    var itemFactory = itemPool.Pick();

                    var item = itemFactory?.Invoke(x, y);
                    Program.Game.Entities.Add(item);
                }
            }
        }
    }
}
