using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using System.Drawing;

namespace Roguelike.UI
{
    public class PauseDialog : Dialog<bool>
    {
        private readonly MenuItem<bool>[] menuOptions;

        private int selectedIndex = 0;

        public PauseDialog()
        {
            menuOptions = new MenuItem<bool>[]
            {
                new MenuItem<bool> { Text = "Resume", OnSelected = () => { return true; } },
                new MenuItem<bool> { Text = "Quit", OnSelected = () => { return false; } }
            };
        }

        protected override void Draw()
        {
            var bounds = new Rectangle(1, 1, Program.MapDisplayWidth - 2, Program.MapDisplayHeight - 2);
            RenderUtils.DrawBox(bounds, Color.FromArgb(128, Color.Black), Color.White);

            string title = "Paused";
            Terminal.Color(Color.Gold);
            Terminal.Print(bounds.X + bounds.Width / 2 - title.Length / 2, 3, title);

            Terminal.Color(Color.White);

            int y = 5;

            for (int i = 0; i < menuOptions.Length; i++)
            {
                var menuItem = menuOptions[i];
                menuItem.Draw(bounds.X + bounds.Width / 2 - menuItem.Text.Length / 2, y++, selectedIndex == i);
            }
        }

        protected override bool Update(out bool result)
        {
            if (InputManager.CheckAction(InputAction.MenuCancel))
            {
                result = true;
                return true;
            }

            if (InputManager.CheckAction(InputAction.MenuUp))
            {
                for (int i = selectedIndex - 1; i >= 0; i--)
                {
                    if (menuOptions[i].IsEnabled)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }

            if (InputManager.CheckAction(InputAction.MenuDown))
            {
                for (int i = selectedIndex + 1; i < menuOptions.Length; i++)
                {
                    if (menuOptions[i].IsEnabled)
                    {
                        selectedIndex = i;
                        break;
                    }
                }
            }

            if (InputManager.CheckAction(InputAction.MenuSelect))
            {
                result = menuOptions[selectedIndex].Select();
                return true;
            }

            result = false;
            return false;
        }
    }
}
