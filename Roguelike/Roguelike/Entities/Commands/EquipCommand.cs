using Roguelike.Entities.Components;
using Roguelike.UI;
using System;
using System.Drawing;

namespace Roguelike.Entities.Commands
{
    public class EquipCommand : Command
    {
        public Entity Item { get; }

        public EquipCommand(Entity item)
        {
            Item = item;
        }

        public override CommandResult Execute(Entity entity)
        {
            if (Item == null || !Item.HasComponent<EquipmentComponent>() || !entity.HasComponent<InventoryComponent>())
            {
                return CommandResult.Failure;
            }

            var equipment = Item.GetComponent<EquipmentComponent>();
            var inventory = entity.GetComponent<InventoryComponent>();

            if (inventory.GetEquipment(equipment.Slot) == Item)
            {
                inventory.Unequip(Item);
                MessageLog.Add($"{entity.Name} unequipped the {Item.Name} from {equipment.Slot}.", Color.LightBlue);
                return CommandResult.Success;
            }

            entity.GetComponent<InventoryComponent>().Equip(Item);
            MessageLog.Add($"{entity.Name} equipped the {Item.Name} on {equipment.Slot}.", Color.LightBlue);
            return CommandResult.Success;
        }
    }
}
