using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.States;
using Roguelike.Utils;
using System.Drawing;
using Point = RogueSharp.Point;
using System;
using RogueSharp;

namespace Roguelike.UI
{
    public class SelectCellDialog : Dialog<Point>
    {
        public int Radius { get; }

        private int mouseX;
        private int mouseY;

        public SelectCellDialog(int radius)
        {
            Radius = radius;

            mouseX = InputManager.MousePosition.X;
            mouseY = InputManager.MousePosition.Y;
        }

        protected override void Draw()
        {
            Terminal.Layer(Renderer.OverlayLayer);
            Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

            Terminal.Layer(Renderer.DialogLayer);
            string title = "Left-click to select cell";

            Terminal.Color(Color.White);
            RenderUtils.DrawBox(Program.MapDisplayWidth / 2 - title.Length / 2 - 1, 1, title.Length + 2, 3, Color.FromArgb(128, Color.Black), Color.White);
            Terminal.Print(Program.MapDisplayWidth / 2 - title.Length / 2, 2, title);

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
        }

        protected override bool Update(out Point result)
        {
            mouseX = InputManager.MousePosition.X;
            mouseY = InputManager.MousePosition.Y;

            if (InputManager.CheckAction(InputAction.LeftClick))
            {
                result = new Point(mouseX + Program.Game.Camera.X, mouseY + Program.Game.Camera.Y);
                return true;
            }
            else if (InputManager.CheckAction(InputAction.MenuCancel))
            {
                result = null;
                return true;
            }

            result = null;
            return false;
        }

        //public override Point Show()
        //{
        //    Terminal.Layer(Renderer.OverlayLayer);
        //    Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

        //    Terminal.Layer(Renderer.DialogLayer + 1);
        //    Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);

        //    Terminal.Layer(Renderer.DialogLayer);
        //    Terminal.ClearArea(0, 0, Program.ScreenWidth, Program.ScreenHeight);


        //    Terminal.Refresh();

        //    bool show = true;

        //    while (show)
        //    {
        //        InputManager.Update(false);




        //    }

        //    return null;
        //}
    }
}
