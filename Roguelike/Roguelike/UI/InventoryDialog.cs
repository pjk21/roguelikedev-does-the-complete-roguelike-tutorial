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

        private readonly InventoryComponent inventory;
        private readonly int itemsPerPage;
        private readonly int pageCount;
        private int selectedIndex = 0;
        private int currentPage = 0;

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

            inventory = Program.Game.Player.GetComponent<InventoryComponent>();

            itemsPerPage = Height - 8;
            pageCount = (inventory.Items.Length - 1) / itemsPerPage;
        }

        protected override void Draw()
        {
            RenderUtils.DrawBox(Bounds, Color.FromArgb(128, Color.Black), Color.White);

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
        }

        protected override bool Update(out Command result)
        {
            if (InputManager.CheckAction(InputAction.MenuCancel))
            {
                result = null;
                return true;
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
                    result = UseItem(inventory, selectedIndex);
                    return true;
                }
            }
            else if (InputManager.CheckAction(InputAction.UseItem))
            {
                result = UseItem(inventory, selectedIndex);
                return true;
            }
            else if (InputManager.CheckAction(InputAction.DropItem))
            {
                if (inventory.Items.Length > 0)
                {
                    inventory.Remove(inventory.Items[selectedIndex], true);
                }
            }

            result = null;
            return false;
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
