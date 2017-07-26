using Roguelike.Entities.Components;
using Roguelike.UI;
using System.Drawing;
using System.Linq;

namespace Roguelike.Entities.Commands
{
    public class TakeCommand : Command
    {
        public override CommandResult Execute(Entity entity)
        {
            if (entity.HasComponent<InventoryComponent>())
            {
                var item = Program.Entities.FirstOrDefault(e => e.HasComponent<ItemComponent>() && e.X == entity.X && e.Y == entity.Y);

                if (item != null)
                {
                    entity.GetComponent<InventoryComponent>().Add(item);
                    MessageLog.Add($"{entity.Name} picks up a {item.Name}.", Color.LightBlue);
                    return CommandResult.Success;
                }
                else
                {
                    MessageLog.Add("No items to pickup.", Color.Red);
                }
            }

            return CommandResult.Failure;
        }
    }
}
