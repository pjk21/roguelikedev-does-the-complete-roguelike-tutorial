using Roguelike.UI;
using System.Drawing;
using System.Linq;

namespace Roguelike.Entities.Commands
{
    public class DescendStairsCommand : Command
    {
        public override CommandResult Execute(Entity entity)
        {
            if (Program.Game.Entities.Any(e => e.Tags.Contains(Tags.Stairs) && e.X == entity.X && e.Y == entity.Y))
            {
                MessageLog.Add("You descend deeper into the dungeon.", Color.LightBlue);
                Program.Game.DescendStairs();

                return CommandResult.Success;
            }

            return CommandResult.Failure;
        }
    }
}
