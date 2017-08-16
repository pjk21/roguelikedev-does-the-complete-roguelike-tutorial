using Roguelike.Entities;
using Roguelike.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var monsterPool = new WeightedPool<Entity.EntityFactoryDelegate>();
            monsterPool.Add(MonsterFactory.CreateRat, 50);
            monsterPool.Add(MonsterFactory.CreateHound, GetWeightFromDungeonLevel(new SortedDictionary<int, int> { { 1, 10 }, { 3, 30 }, { 5, 50 } }));

            for (int i = 0; i < numberOfMonsters; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                var monsterFactory = monsterPool.Pick();

                var monster = monsterFactory(x, y, Program.Game.DungeonLevel);
                Program.Game.Entities.Add(monster);
            }
        }

        protected virtual void SpawnItems(Map map, Rectangle room)
        {
            int numberOfItems = Program.Game.Random.Next(MaximumItemsPerRoom + 1);

            var itemPool = new WeightedPool<Entity.EntityFactoryDelegate>();
            itemPool.Add(ItemFactory.CreatePotion, 70);
            itemPool.Add(ItemFactory.CreateLightningScroll, GetWeightFromDungeonLevel(new SortedDictionary<int, int> { { 2, 10 }, { 4, 25 } }));
            itemPool.Add(ItemFactory.CreateConfuseScroll, GetWeightFromDungeonLevel(new SortedDictionary<int, int> { { 2, 25 } }));
            itemPool.Add(ItemFactory.CreateFireballScroll, GetWeightFromDungeonLevel(new SortedDictionary<int, int> { { 4, 20 } }));
            itemPool.Add(ItemFactory.CreateSword, 30);

            for (int i = 0; i < numberOfItems; i++)
            {
                int x = Program.Game.Random.Next(room.Left + 1, room.Right);
                int y = Program.Game.Random.Next(room.Top + 1, room.Bottom);

                if (map.IsWalkable(x, y))
                {
                    var itemFactory = itemPool.Pick();

                    var item = itemFactory(x, y, Program.Game.DungeonLevel);
                    Program.Game.Entities.Add(item);
                }
            }
        }

        private int GetWeightFromDungeonLevel(SortedDictionary<int, int> weightLevelPairs)
        {
            var weight = 0;

            foreach (var dungeonLevel in weightLevelPairs.Keys.Reverse())
            {
                if (Program.Game.DungeonLevel >= dungeonLevel)
                {
                    return weightLevelPairs[dungeonLevel];
                }
            }

            return weight;
        }
    }
}
