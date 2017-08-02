using Roguelike.Entities;
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

            var mapWidth = info.GetInt32("MapWidth");
            var mapHeight = info.GetInt32("MapHeight");
            var mapData = (byte[])info.GetValue("MapData", typeof(byte[]));

            Map = new Map(mapWidth, mapHeight);
            Map.Deserialize(mapData);

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

                Player.AddComponent(new FighterComponent { MaximumHealth = 30, CurrentHealth = 30, Power = 5, Defense = 2, DeathFunction = DeathFunctions.PlayerDeath });
                Player.AddComponent(new PlayerInputComponent());
                Player.AddComponent(new InventoryComponent());

                Entities.Add(Player);

                Map = new BspMapGenerator().Generate(80, 50);
            }

            Map.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);

            foreach (var cell in Map.GetAllCells())
            {
                Map.PathfindingMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable);
            }
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

            info.AddValue("MapWidth", Map.Width);
            info.AddValue("MapHeight", Map.Height);
            info.AddValue("MapData", Map.Serialize());

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
