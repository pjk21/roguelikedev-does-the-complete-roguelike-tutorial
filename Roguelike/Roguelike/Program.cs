using BearLib;
using Roguelike.Entities;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.States;
using Roguelike.World;
using Roguelike.World.MapGeneration;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike
{
    class Program
    {
        public const int ScreenWidth = 80;
        public const int ScreenHeight = 50;
        public const int MapDisplayWidth = ScreenWidth - 20;
        public const int MapDisplayHeight = ScreenHeight - 8;

        public static bool IsDebugModeEnabled { get; set; } = false;

        public static Random Random { get; set; } = new Random(123456789);

        public static IState CurrentState { get; set; } = new GameState();

        public static Map Map { get; set; }

        public static List<Entity> Entities { get; } = new List<Entity>();
        public static Entity Player { get; set; }

        static void Main(string[] args)
        {
            Initialize();

            while (true)
            {
                Draw();

                if (!Update())
                {
                    break;
                }
            }

            Terminal.Close();
        }

        private static void Initialize()
        {
            Terminal.Open();
            Terminal.Set($"window: size={ScreenWidth}x{ScreenHeight};");
            Terminal.Set($"input: filter=[keyboard, mouse];");
            Terminal.Set($"font: Cheepicus_8x8x2.png, size=16x16, codepage=437;");
            Terminal.Set($"0xE000: Tiles.png, size=16x16");
            Terminal.Set($"0xE800: Entities.png, size=16x16;");
            Terminal.Set($"0xEF9B: UI.png, size=16x16;");
            Terminal.Composition(true);

            Player = new Entity("Player", 25, 23, '@', Colours.Player, true)
            {
                SpriteIndex = EntitySprites.Player,
                RenderLayer = Renderer.ActorLayer
            };

            Player.AddComponent(new FighterComponent { MaximumHealth = 30, CurrentHealth = 30, Power = 5, Defense = 2, DeathFunction = DeathFunctions.PlayerDeath });
            Player.AddComponent(new PlayerInputComponent());
            Player.AddComponent(new InventoryComponent());

            Entities.Add(Player);

            for (int i = 0; i < 100; i++)
            {
                var potion = new Entity("Potion", 0, 0, '!', Color.AliceBlue);
                potion.AddComponent(new ItemComponent());

                Player.GetComponent<InventoryComponent>().Add(potion);
            }

            Map = new BspMapGenerator().Generate(80, 50);
            Map.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);

            foreach (var cell in Map.GetAllCells())
            {
                Map.PathfindingMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable);
            }

            CurrentState?.Initialize();
        }

        private static bool Update()
        {
            InputManager.Update();

            if (CurrentState == null)
            {
                throw new NullReferenceException(nameof(CurrentState));
            }

            return CurrentState.Update();
        }

        private static void Draw()
        {
            Terminal.Color(Color.White);
            Terminal.BkColor(Color.Black);
            Terminal.Clear();

            CurrentState?.Draw();

            Terminal.Refresh();
        }
    }
}
