using BearLib;
using Roguelike.Input;
using Roguelike.Render;
using Roguelike.UI;
using System;
using System.Drawing;
using System.Linq;

namespace Roguelike.States
{
    public class MainMenuState : IState
    {
        private string title = "RogueLike: The Tutorial";
        private MenuItem[] menuOptions; //= new string[] { "New Game", "Load Game", "Exit Game" };
        private int selectedIndex = 0;

        private bool quitRequested = false;

        public void Initialize()
        {
            menuOptions = new MenuItem[]
            {
                new MenuItem { Text = "New Game", OnSelected = NewGame },
                new MenuItem {Text = "Load Game", OnSelected = LoadGame, IsEnabled = false },
                new MenuItem {Text = "Exit Game", OnSelected = ExitGame }
            };
        }

        public bool Update()
        {
            if (InputManager.CheckAction(InputAction.MenuCancel) || quitRequested)
            {
                return false;
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
                menuOptions[selectedIndex].OnSelected();
            }

            return true;
        }

        public void Draw()
        {
            int centerX = Program.ScreenWidth / 2;
            int centerY = Program.ScreenHeight / 2;

            RenderUtils.DrawBox(centerX - title.Length / 2 - 4, centerY - 8, title.Length + 8, 11, Color.CornflowerBlue, Color.White);

            Terminal.Color(Color.Gold);
            Terminal.Print(centerX - title.Length / 2, centerY - 5, title);
            int y = centerY - 3;
            int x = centerX - (menuOptions.Aggregate("", (max, cur) => max.Length > cur.Text.Length ? max : cur.Text).Length / 2);

            for (int i = 0; i < menuOptions.Length; i++)
            {
                var option = menuOptions[i];
                option.Draw(x, y++, i == selectedIndex);
            }
        }

        private void NewGame()
        {
            Program.ChangeState(new GameState());
        }

        private void LoadGame()
        {
            Console.WriteLine("load game");
        }

        private void ExitGame()
        {
            quitRequested = true;
        }
    }
}
