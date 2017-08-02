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
                        var potion = new Entity("Healing Potion", x, y, '!', Color.Violet)
                        {
                            SpriteIndex = EntitySprites.Potion,
                            RenderLayer = Renderer.ItemLayer
                        };
                        potion.AddComponent(new ItemComponent { Description = "Heals you for a small amount of HP.", UseFunction = ItemFunctions.PotionFunction });

                        Program.Game.Entities.Add(potion);
                    }
                    else if (itemChance < 0.8)
                    {
                        var lightningScroll = new Entity("Lightning Scroll", x, y, '[', Color.LightBlue)
                        {
                            SpriteIndex = EntitySprites.Scroll,
                            SpriteTint = Color.LightBlue,
                            RenderLayer = Renderer.ItemLayer
                        };
                        lightningScroll.AddComponent(new ItemComponent { Description = "Fires bolts of lightning at your enemies.", UseFunction = ItemFunctions.LightningScroll });

                        Program.Game.Entities.Add(lightningScroll);
                    }
                    else if (itemChance < 0.9)
                    {
                        var confuseScroll = new Entity("Confuse Scroll", x, y, '[', Color.LightYellow)
                        {
                            SpriteIndex = EntitySprites.Scroll,
                            SpriteTint = Color.LightYellow,
                            RenderLayer = Renderer.ItemLayer
                        };
                        confuseScroll.AddComponent(new ItemComponent { Description = "Complicated math formulas that confuse your enemies.", UseFunction = ItemFunctions.ConfuseScroll });

                        Program.Game.Entities.Add(confuseScroll);
                    }
                    else
                    {
                        var fireballScroll = new Entity("Fireball Scroll", x, y, '[', Color.Orange)
                        {
                            SpriteIndex = EntitySprites.Scroll,
                            SpriteTint = Color.Orange,
                            RenderLayer = Renderer.ItemLayer
                        };
                        fireballScroll.AddComponent(new ItemComponent { Description = "Launch a ball of fire at your enemies.", UseFunction = ItemFunctions.FireballScroll });

                        Program.Game.Entities.Add(fireballScroll);
                    }
                }
            }
        }
    }
}
