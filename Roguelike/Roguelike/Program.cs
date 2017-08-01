using BearLib;
using Roguelike.Entities;
using Roguelike.Input;
using Roguelike.States;
using Roguelike.World;
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

        public static IState CurrentState { get; private set; } = new MainMenuState();

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

        public static void ChangeState(IState state)
        {
            CurrentState = state;
            state.Initialize();
        }
    }
}
