using BearLib;
using Roguelike.Entities.Components;
using Roguelike.Input;
using Roguelike.Render;
using System.Drawing;

namespace Roguelike.UI
{
    public class LevelUpDialog : Dialog
    {
        private readonly MenuItem[] menuOptions;

        private int selectedIndex = 0;

        public LevelUpDialog()
        {
            var fighter = Program.Game.Player.GetComponent<FighterComponent>();

            menuOptions = new MenuItem[]
            {
                new MenuItem { Text = $"HP  {fighter.Health.Base} \u2192 {fighter.Health.Base + 5}", OnSelected = () => { fighter.Health.Base += 5; } },
                new MenuItem { Text = $"POW {fighter.Power.Value} \u2192 {fighter.Power.Value + 1}", OnSelected = () => {fighter.Power.Base++; } },
                new MenuItem { Text = $"DEF {fighter.Defense.Value} \u2192 {fighter.Defense.Value + 1}", OnSelected = () => {fighter.Defense.Base++; } }
            };
        }

        protected override void Draw()
        {
            var bounds = new Rectangle(1, 1, Program.MapDisplayWidth - 2, Program.MapDisplayHeight - 2);
            RenderUtils.DrawBox(bounds, Color.FromArgb(192, Color.CornflowerBlue), Color.White);

            string title = "Level Up";
            Terminal.Color(Color.Gold);
            Terminal.Print(bounds.X + bounds.Width / 2 - title.Length / 2, 3, title);

            Terminal.Color(Color.White);
            Terminal.Print(bounds.X + bounds.Width / 2 - "Select an attribute to increase".Length / 2, 4, "Select an attribute to increase");

            Terminal.Color(Color.White);

            int y = 6;

            for (int i = 0; i < menuOptions.Length; i++)
            {
                var menuItem = menuOptions[i];
                menuItem.Draw(bounds.X + bounds.Width / 2 - menuItem.Text.Length / 2, y++, selectedIndex == i);
            }
        }

        protected override bool Update()
        {
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
                return true;
            }

            return false;
        }
    }
}
