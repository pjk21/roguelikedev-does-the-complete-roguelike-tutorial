using BearLib;
using Roguelike.Render;
using Roguelike.States;
using System.Drawing;
using Point = RogueSharp.Point;

namespace Roguelike.UI
{
    public class SelectCellDialog : Dialog<Point>
    {
        public override Point Show()
        {
            Terminal.Layer(Renderer.OverlayLayer);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Layer(Renderer.DialogLayer + 1);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Layer(Renderer.DialogLayer);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Print(Program.MapDisplayWidth / 2 - "Select Cell".Length / 2, 2, "Select Cell");
            Terminal.Refresh();

            bool show = true;

            while (show)
            {
                var mouseX = Terminal.State(Terminal.TK_MOUSE_X);
                var mouseY = Terminal.State(Terminal.TK_MOUSE_Y);

                Terminal.Layer(Renderer.OverlayLayer);
                Terminal.Color(Color.Yellow);
                Terminal.Put(mouseX, mouseY, UISprites.TileHighlighter);
                Terminal.Refresh();

                Terminal.ClearArea(mouseX, mouseY, 1, 1);

                if (Terminal.HasInput())
                {
                    var input = Terminal.Read();

                    if (Terminal.Check(Terminal.TK_MOUSE_LEFT))
                    {
                        return new Point(mouseX + GameState.Camera.X, mouseY + GameState.Camera.Y);
                    }

                    if (input == Terminal.TK_ESCAPE)
                    {
                        show = false;
                    }
                }
            }

            return null;
        }
    }
}
