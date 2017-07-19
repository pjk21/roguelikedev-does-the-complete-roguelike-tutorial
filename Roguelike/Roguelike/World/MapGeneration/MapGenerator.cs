﻿using Roguelike.Entities;
using Roguelike.Entities.Components;
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

                    rat.AddComponent(new FighterComponent { MaximumHealth = 3, CurrentHealth = 3, Power = 1, Defense = 0 })
                        .AddComponent(new BasicMonsterComponent());

                    Program.Entities.Add(rat);
                }
                else
                {
                    var hound = new Entity("Hound", x, y, 'h', Colours.Hound, true)
                    {
                        SpriteIndex = EntitySprites.Hound
                    };

                    hound.AddComponent(new FighterComponent { MaximumHealth = 8, CurrentHealth = 8, Power = 3, Defense = 0 })
                        .AddComponent(new BasicMonsterComponent());

                    Program.Entities.Add(hound);
                }
            }
        }
    }
}
