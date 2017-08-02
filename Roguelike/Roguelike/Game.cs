using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Render;
using Roguelike.UI;
using Roguelike.World;
using Roguelike.World.MapGeneration;
using System;
using System.Collections.Generic;

namespace Roguelike
{
    public class Game
    {
        public bool IsDebugModeEnabled { get; set; } = false;

        public Random Random { get; private set; }
        public int Seed { get; private set; } = 123456789;

        public Map Map { get; private set; }
        public Camera Camera { get; } = new Camera(0, 0, Program.MapDisplayWidth, Program.MapDisplayHeight);

        public List<Entity> Entities { get; } = new List<Entity>();
        public Entity Player { get; private set; }

        public void Initialize()
        {
            Random = new Random(Seed);

            MessageLog.Clear();

            Entities.Clear();

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
            Map.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);

            foreach (var cell in Map.GetAllCells())
            {
                Map.PathfindingMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable);
            }
        }
    }
}
