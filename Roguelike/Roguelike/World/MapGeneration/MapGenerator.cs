using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Render;
using System.Drawing;
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
                    var rat = new Entity("Rat", x, y, 'r', Colours.Rat, true)
                    {
                        SpriteIndex = EntitySprites.Rat,
                        RenderLayer = Renderer.ActorLayer
                    };

                    rat.AddComponent(new FighterComponent { MaximumHealth = 3, CurrentHealth = 3, Power = 1, Defense = 0, AttackElement = ElementType.Poison, DeathFunction = DeathFunctions.MonsterDeath });
                    rat.AddComponent(new BasicMonsterComponent());

                    Program.Game.Entities.Add(rat);
                }
                else
                {
                    var hound = new Entity("Hound", x, y, 'h', Colours.Hound, true)
                    {
                        SpriteIndex = EntitySprites.Hound,
                        RenderLayer = Renderer.ActorLayer
                    };

                    hound.AddComponent(new FighterComponent { MaximumHealth = 8, CurrentHealth = 8, Power = 3, Defense = 1, DeathFunction = DeathFunctions.MonsterDeath });
                    hound.AddComponent(new BasicMonsterComponent());

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
