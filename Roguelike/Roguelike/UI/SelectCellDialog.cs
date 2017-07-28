﻿using BearLib;
using Roguelike.Input;
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

                InputManager.Update(false);

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
