using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.States;
using Roguelike.Utils;
using System.Drawing;
using Point = RogueSharp.Point;

namespace Roguelike.UI
{
    public class SelectCellDialog : Dialog<Point>
    {
        public int Radius { get; }

        public SelectCellDialog(int radius)
        {
            Radius = radius;
        }

        public override Point Show()
        {
            Terminal.Layer(Renderer.OverlayLayer);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Layer(Renderer.DialogLayer + 1);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Layer(Renderer.DialogLayer);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            string title = "Left-click to select cell";

            Terminal.Color(Color.White);
            RenderUtils.DrawBox(Program.MapDisplayWidth / 2 - title.Length / 2 - 1, 1, title.Length + 2, 3, Color.FromArgb(128, Color.Black), Color.White);
            Terminal.Print(Program.MapDisplayWidth / 2 - title.Length / 2, 2, title);
            Terminal.Refresh();

            bool show = true;

            while (show)
            {
                InputManager.Update(false);

                var mouseX = InputManager.MousePosition.X;
                var mouseY = InputManager.MousePosition.Y;

                Terminal.Layer(Renderer.OverlayLayer);

                for (int ox = mouseX - Radius; ox <= mouseX + Radius; ox++)
                {
                    for (int oy = mouseY - Radius; oy <= mouseY + Radius; oy++)
                    {
                        if (MathUtils.Distance(mouseX, mouseY, ox, oy) <= Radius)
                        {
                            Terminal.Color(Color.FromArgb(128, Color.Yellow));
                            Terminal.Put(ox, oy, UISprites.DialogBackground);
                        }
                    }
                }

                Terminal.Color(Color.Yellow);
                Terminal.Put(mouseX, mouseY, UISprites.TileHighlighter);
                Terminal.Refresh();

                Terminal.ClearArea(mouseX - Radius, mouseY - Radius, Radius * 2 + 1, Radius * 2 + 1);

                if (InputManager.CheckAction(InputAction.LeftClick))
                {
                    return new Point(mouseX + GameState.Camera.X, mouseY + GameState.Camera.Y);
                }
                else if (InputManager.CheckAction(InputAction.MenuCancel))
                {
                    show = false;
                }
            }

            return null;
        }
    }
}
