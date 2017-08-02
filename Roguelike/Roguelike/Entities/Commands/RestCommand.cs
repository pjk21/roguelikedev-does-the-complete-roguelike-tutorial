using Roguelike.Entities.Components;
using Roguelike.UI;
using System.Drawing;

namespace Roguelike.Entities.Commands
{
    public class RestCommand : Command
    {
        public const float RestHealPercent = 0.1f;

        public override CommandResult Execute(Entity entity)
        {
            if (entity == Program.Game.Player)
            {
                var healed = entity.GetComponent<FighterComponent>()?.HealPercent(RestHealPercent);
                MessageLog.Add($"You rest for a turn and regain {healed} HP.", Color.LightGreen);
            }

            return CommandResult.Success;
        }
    }
}
