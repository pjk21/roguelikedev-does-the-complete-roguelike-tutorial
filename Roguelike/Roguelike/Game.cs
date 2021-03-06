﻿using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Render;
using Roguelike.UI;
using Roguelike.World;
using Roguelike.World.MapGeneration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Roguelike
{
    [Serializable]
    public class Game : ISerializable
    {
        public const string SaveGameFileName = "save.dat";

        public bool IsDebugModeEnabled { get; set; } = false;

        public Random Random { get; private set; }
        public int Seed { get; private set; } = 123456789;

        public int DungeonLevel { get; private set; } = 1;
        public Map Map { get; private set; }
        public Camera Camera { get; } = new Camera(0, 0, Program.MapDisplayWidth, Program.MapDisplayHeight);

        public List<Entity> Entities { get; } = new List<Entity>();
        public Entity Player { get; private set; }

        public Game()
        {

        }

        public Game(SerializationInfo info, StreamingContext context)
        {
            Seed = info.GetInt32(nameof(Seed));

            Map = (Map)info.GetValue(nameof(Map), typeof(Map));

            Entities = (List<Entity>)info.GetValue(nameof(Entities), typeof(List<Entity>));

            var playerIndex = info.GetInt32(nameof(Player));
            Player = Entities[playerIndex];
        }

        public void Initialize(bool newGame)
        {
            Random = new Random(Seed);

            MessageLog.Clear();

            if (newGame)
            {
                Player = new Entity("Player", 25, 23, '@', Colours.Player, true)
                {
                    SpriteIndex = EntitySprites.Player,
                    RenderLayer = Renderer.ActorLayer
                };

                var fighterComponent = new FighterComponent { DeathFunction = DeathFunctions.PlayerDeath };
                fighterComponent.Health.Base = 30;
                fighterComponent.Power.Base = 5;
                fighterComponent.Defense.Base = 2;

                Player.AddComponent(fighterComponent);
                Player.AddComponent(new PlayerInputComponent());
                Player.AddComponent(new InventoryComponent());

                Entities.Add(Player);

                Map = new BspMapGenerator().Generate(80, 50);
            }

            Map.UpdateFovMap();
            Map.FovMap.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);
        }

        public void DescendStairs()
        {
            DungeonLevel++;

            Entities.Clear();
            Entities.Add(Player);

            Map = new BspMapGenerator().Generate(80, 50);

            Map.UpdateFovMap();
            Map.FovMap.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);
        }

        public void Save()
        {
            using (var file = File.Open(SaveGameFileName, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(file, this);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Seed), Seed);

            info.AddValue(nameof(Map), Map);

            info.AddValue(nameof(Entities), Entities);
            info.AddValue(nameof(Player), Entities.IndexOf(Player));
        }

        public static Game Load()
        {
            Game game;

            using (var file = File.OpenRead(SaveGameFileName))
            {
                var formatter = new BinaryFormatter();
                game = (Game)formatter.Deserialize(file);
            }

            return game;
        }
    }
}
