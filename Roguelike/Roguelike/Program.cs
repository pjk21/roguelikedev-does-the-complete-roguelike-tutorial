using BearLib;
using System.Drawing;

namespace Roguelike
{
    class Program
    {
        public const int ScreenWidth = 80;
        public const int ScreenHeight = 50;

        public static int PlayerX { get; set; } = ScreenWidth / 2;
        public static int PlayerY { get; set; } = ScreenHeight / 2;

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
        }

        private static bool Update()
        {
            var input = Terminal.Read();

            switch (input)
            {
                case Terminal.TK_ESCAPE:
                case Terminal.TK_CLOSE:
                    return false;

                case Terminal.TK_LEFT:
                    PlayerX--;
                    break;
                case Terminal.TK_RIGHT:
                    PlayerX++;
                    break;
                case Terminal.TK_UP:
                    PlayerY--;
                    break;
                case Terminal.TK_DOWN:
                    PlayerY++;
                    break;
            }

            return true;
        }

        private static void Draw()
        {
            Terminal.Clear();

            Terminal.Put(PlayerX, PlayerY, '@');

            Terminal.Refresh();
        }
    }
}
