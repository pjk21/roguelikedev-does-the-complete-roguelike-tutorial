using Roguelike.Entities.Components;

namespace Roguelike.Entities.Commands
{
    public class UseCommand : Command
    {
        public Entity Item { get; }

        public UseCommand(Entity item)
        {
            Item = item;
        }

        public override CommandResult Execute(Entity entity)
        {
            if (Item != null)
            {
                if (Item.GetComponent<ItemComponent>().UseFunction(entity))
                {
                    entity.GetComponent<InventoryComponent>().Remove(Item, false);
                }
            }

            return CommandResult.Failure;
        }
    }
}
