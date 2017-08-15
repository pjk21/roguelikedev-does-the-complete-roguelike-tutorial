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
                Program.Game.DescendStairs();
                MessageLog.Add($"You descend to level {Program.Game.DungeonLevel} of the dungeon.", Color.LightBlue);

                return CommandResult.Success;
            }

            return CommandResult.Failure;
        }
    }
}
