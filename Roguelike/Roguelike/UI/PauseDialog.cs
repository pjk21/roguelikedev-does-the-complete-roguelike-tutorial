using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using System.Drawing;

namespace Roguelike.UI
{
    public class PauseDialog : Dialog<bool>
    {
        private MenuItem[] menuOptions;

        private bool result = true;

        public override bool Show()
        {
            bool show = true;

            menuOptions = new MenuItem[]
            {
                new MenuItem { Text = "Resume", OnSelected = () => { result = true; show = false; } },
                new MenuItem { Text = "Quit", OnSelected = () => { result = false; show = false; } }
            };

            var bounds = new Rectangle(1, 1, Program.MapDisplayWidth - 2, Program.MapDisplayHeight - 2);

            Terminal.Layer(Renderer.DialogLayer);
            RenderUtils.DrawBox(bounds, Color.FromArgb(128, Color.Black), Color.White);

            Terminal.Layer(Renderer.DialogLayer + 1);

            int selectedIndex = 0;

            while (show)
            {
                Terminal.Color(Color.White);
                Terminal.BkColor(Color.Black);
                Terminal.ClearArea(1, 1, Program.MapDisplayWidth - 2, Program.MapDisplayHeight - 2);

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

                Terminal.Refresh();

                InputManager.Update(true);

                if (InputManager.CheckAction(InputAction.MenuCancel))
                {
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
                    menuOptions[selectedIndex].Select();
                }
            }

            return result;
        }
    }
}
