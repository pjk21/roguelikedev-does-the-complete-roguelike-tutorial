using BearLib;
using Roguelike.Entities;
using Roguelike.Entities.Commands;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using System;
using System.Drawing;

namespace Roguelike.UI
{
    public class InventoryDialog : Dialog<Command>
    {
        private readonly Rectangle bounds;

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle Bounds => bounds;

        public InventoryDialog(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;

            bounds = new Rectangle(X, Y, Width, Height);
        }

        public override Command Show()
        {
            bool show = true;

            Terminal.Layer(Renderer.DialogLayer);

            for (int x = Bounds.Left; x < Bounds.Right; x++)
            {
                for (int y = Bounds.Top; y < Bounds.Bottom; y++)
                {
                    Terminal.Color(Color.FromArgb(128, Color.Black));
                    Terminal.Put(x, y, UISprites.DialogBackground);

                    if (x == Bounds.Left && y == Bounds.Top)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.TopLeftCornerBorder);
                    }
                    else if (x == Bounds.Left && y == Bounds.Bottom - 1)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.BottomLeftCornerBorder);
                    }
                    else if (x == Bounds.Right - 1 && y == Bounds.Top)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.TopRightCornerBorder);
                    }
                    else if (x == Bounds.Right - 1 && y == Bounds.Bottom - 1)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.BottomRightCornerBorder);
                    }
                    else if (x == Bounds.Left || x == Bounds.Right - 1)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.VerticalBorder);
                    }
                    else if (y == Bounds.Top || y == Bounds.Bottom - 1)
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, UISprites.HorizontalBorder);
                    }
                }
            }

            Terminal.Layer(Renderer.DialogLayer + 1);

            int selectedIndex = 0;
            var inventory = Program.Player.GetComponent<InventoryComponent>();

            int itemsPerPage = Height - 6;
            int pageCount = inventory.Items.Length / itemsPerPage;
            int currentPage = 0;

            while (show)
            {
                Terminal.Color(Color.White);
                Terminal.BkColor(Color.Black);
                Terminal.ClearArea(X, Y, Width, Height);

                string title = $"Inventory ({currentPage}/{pageCount})";
                Terminal.Print(X + Width / 2 - title.Length / 2, Y + 1, title);
                Terminal.Print(X + Width / 2, Y + 2, $"{selectedIndex}");

                int y = Y + 3;

                int start = itemsPerPage * currentPage;
                int end = start + itemsPerPage;
                for (int i = start; i < Math.Min(end, inventory.Items.Length); i++)
                {
                    var item = inventory.Items[i];

                    if (i == selectedIndex)
                    {
                        Terminal.Color(Color.Yellow);
                    }
                    else
                    {
                        Terminal.Color(Color.White);
                    }

                    Terminal.Print(X + 2, y++, item.Name);
                }

                Terminal.Color(Color.Green);
                Terminal.Print(X + 2, Bounds.Bottom - 2, "(U)se");

                Terminal.Refresh();

                var input = InputManager.AwaitInput();

                switch (input)
                {
                    case InputAction.Quit:
                        return null;

                    case InputAction.MoveNorth:
                        selectedIndex--;
                        selectedIndex = selectedIndex.Clamp(0, inventory.Items.Length - 1);

                        currentPage = selectedIndex / itemsPerPage;
                        break;
                    case InputAction.MoveSouth:
                        selectedIndex++;
                        selectedIndex = selectedIndex.Clamp(0, inventory.Items.Length - 1);

                        currentPage = selectedIndex / itemsPerPage;
                        break;
                    case InputAction.MoveEast:
                        currentPage++;
                        currentPage = currentPage.Clamp(0, pageCount);

                        selectedIndex = itemsPerPage * currentPage;
                        break;
                    case InputAction.MoveWest:
                        currentPage--;
                        currentPage = currentPage.Clamp(0, pageCount);

                        selectedIndex = itemsPerPage * currentPage;
                        break;
                    case InputAction.UseItem:
                        var item = inventory.Items[selectedIndex];

                        if (item.GetComponent<ItemComponent>().UseFunction != null)
                        {
                            return new UseCommand(item);
                        }
                        break;
                }
            }

            return null;
        }
    }
}
