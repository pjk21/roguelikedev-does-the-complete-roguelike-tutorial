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
            RenderUtils.DrawBox(Bounds, Color.FromArgb(128, Color.Black), Color.White);

            Terminal.Layer(Renderer.DialogLayer + 1);

            int selectedIndex = 0;
            var inventory = Program.Player.GetComponent<InventoryComponent>();

            int itemsPerPage = Height - 8;
            int pageCount = (inventory.Items.Length - 1) / itemsPerPage;
            int currentPage = 0;

            while (show)
            {
                Terminal.Color(Color.White);
                Terminal.BkColor(Color.Black);
                Terminal.ClearArea(X, Y, Width, Height);

                string title = $"Inventory ({currentPage}/{pageCount})";
                Terminal.Print(X + Width / 2 - title.Length / 2, Y + 1, title);

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

                if (inventory.Items.Length > 0)
                {
                    Terminal.Color(Color.LightBlue);
                    Terminal.Print(X + 2, Bounds.Bottom - 4, $"{inventory.Items[selectedIndex].GetComponent<ItemComponent>().Description}");
                }

                Terminal.Color(Color.Green);
                Terminal.Print(X + 2, Bounds.Bottom - 2, "(U)se - (D)rop");

                Terminal.Refresh();

                InputManager.Update(true);

                if (InputManager.CheckAction(InputAction.MenuCancel))
                {
                    return null;
                }
                else if (InputManager.CheckAction(InputAction.MenuUp) || InputManager.MouseScroll < 0)
                {
                    selectedIndex--;
                    selectedIndex = selectedIndex.Clamp(0, inventory.Items.Length - 1);

                    currentPage = selectedIndex / itemsPerPage;
                }
                else if (InputManager.CheckAction(InputAction.MenuDown) || InputManager.MouseScroll > 0)
                {
                    selectedIndex++;
                    selectedIndex = selectedIndex.Clamp(0, inventory.Items.Length - 1);

                    currentPage = selectedIndex / itemsPerPage;
                }
                else if (InputManager.CheckAction(InputAction.MenuRight))
                {
                    currentPage++;
                    currentPage = currentPage.Clamp(0, pageCount);

                    selectedIndex = itemsPerPage * currentPage;
                }
                else if (InputManager.CheckAction(InputAction.MenuLeft))
                {
                    currentPage--;
                    currentPage = currentPage.Clamp(0, pageCount);

                    selectedIndex = itemsPerPage * currentPage;
                }
                else if (InputManager.CheckAction(InputAction.LeftClick))
                {
                    int clickCount = Terminal.State(Terminal.TK_MOUSE_CLICKS);

                    if (clickCount == 1)
                    {
                        var mouseY = InputManager.MousePosition.Y;
                        selectedIndex = (itemsPerPage * currentPage) + (mouseY - Y - 3);
                        selectedIndex = selectedIndex.Clamp(0, inventory.Items.Length - 1);
                    }
                    else if (clickCount == 2)
                    {
                        return UseItem(inventory, selectedIndex);
                    }
                }
                else if (InputManager.CheckAction(InputAction.UseItem))
                {
                    return UseItem(inventory, selectedIndex);
                }
                else if (InputManager.CheckAction(InputAction.DropItem))
                {
                    if (inventory.Items.Length > 0)
                    {
                        inventory.Remove(inventory.Items[selectedIndex], true);
                    }
                }
            }

            return null;
        }

        private Command UseItem(InventoryComponent inventory, int selectedIndex)
        {
            if (inventory.Items.Length > 0)
            {
                var item = inventory.Items[selectedIndex];

                if (item.GetComponent<ItemComponent>().UseFunction != null)
                {
                    return new UseCommand(item);
                }
            }

            return null;
        }
    }
}
