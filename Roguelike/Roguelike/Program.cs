using BearLib;
using Roguelike.Input;
using Roguelike.States;
using System;
using System.Drawing;

namespace Roguelike
{
    class Program
    {
        public const int ScreenWidth = 80;
        public const int ScreenHeight = 50;
        public const int MapDisplayWidth = ScreenWidth - 20;
        public const int MapDisplayHeight = ScreenHeight - 8;

        public static IState CurrentState { get; private set; } = new MainMenuState();
        public static Game Game { get; set; }

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

            if (InputManager.CheckAction(InputAction.CloseWindow))
            {
                return false;
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
