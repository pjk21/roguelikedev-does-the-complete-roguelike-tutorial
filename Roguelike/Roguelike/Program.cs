using BearLib;
using Roguelike.Render;
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

        public static IRenderer ActiveRenderer { get; set; } = new AsciiRenderer();

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

            Player = new Entity(25, 23, '@', Color.White);
            Entities.Add(Player);

            Map = new BspMapGenerator().Generate(80, 50);
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
                    Player.Move(-1, 0);
                    break;
                case Terminal.TK_RIGHT:
                    Player.Move(1, 0);
                    break;
                case Terminal.TK_UP:
                    Player.Move(0, -1);
                    break;
                case Terminal.TK_DOWN:
                    Player.Move(0, 1);
                    break;
            }

            return true;
        }

        private static void Draw()
        {
            Terminal.Clear();

            ActiveRenderer.RenderMap(Map);
            ActiveRenderer.RenderEntities(Entities);

            Terminal.Refresh();
        }
    }
}
