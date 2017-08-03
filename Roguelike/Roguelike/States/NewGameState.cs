using BearLib;
using System.Drawing;

namespace Roguelike.States
{
    public class NewGameState : IState
    {
        private string name = "Player";

        public void Initialize()
        {

        }

        public bool Update()
        {
            return true;
        }

        public void Draw()
        {
            string header = $"Enter Your Name (Max 20 Characters)";

            Terminal.Color(Color.Gold);
            Terminal.Print(Program.ScreenWidth / 2 - header.Length / 2, 2, header);

            Terminal.Color(Color.White);
            int result = Terminal.ReadStr(Program.ScreenWidth / 2 - header.Length / 2 + 2, 3, ref name, 20);

            if (result == Terminal.TK_INPUT_CANCELLED)
            {
                Program.ChangeState(new MainMenuState());
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    Program.Game = new Game();
                    Program.Game.Initialize(true);

                    Program.Game.Player.Name = name;

                    Program.ChangeState(new GameState());
                }
            }
        }
    }
}
