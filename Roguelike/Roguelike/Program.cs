using BearLib;
using Roguelike.Input;
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

        public static Random Random { get; set; } = new Random(123456789);

        public static IState CurrentState { get; } = new GameState();

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
            Terminal.Set($"font: Cheepicus_8x8x2.png, size=16x16, codepage=437;");
            Terminal.Set($"0xE000: Tiles.png, size=16x16");
            Terminal.Set($"0xE800: Entities.png, size=16x16;");

            Player = new Entity("Player", 25, 23, '@', Colours.Player, true)
            {
                SpriteIndex = EntitySprites.Player
            };

            Entities.Add(Player);

            Map = new BspMapGenerator().Generate(80, 50);
            Map.ComputeFov(Player.X, Player.Y, Entity.PlayerFovRadius, true);
        }

        private static bool Update()
        {
            InputManager.Update();

            return CurrentState?.Update() ?? true;
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
