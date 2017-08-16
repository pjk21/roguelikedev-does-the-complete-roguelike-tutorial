using System;
using System.Collections.Generic;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class InventoryComponent : Component
    {
        private readonly List<Entity> items = new List<Entity>();
        private readonly Dictionary<EquipmentSlot, Entity> equipment = new Dictionary<EquipmentSlot, Entity>();

        public Entity[] Items => items.ToArray();

        public void Add(Entity item)
        {
            items.Add(item);

            Program.Game.Entities.Remove(item);

            if (item.HasComponent<EquipmentComponent>() && (!equipment.ContainsKey(item.GetComponent<EquipmentComponent>().Slot) || equipment[item.GetComponent<EquipmentComponent>().Slot] == null))
            {
                Equip(item);
            }
        }

        public void Remove(Entity item, bool drop)
        {
            if (item.HasComponent<EquipmentComponent>() && equipment.ContainsValue(item))
            {
                Unequip(item);
            }

            items.Remove(item);

            if (drop)
            {
                item.X = Entity.X;
                item.Y = Entity.Y;
                Program.Game.Entities.Add(item);
            }
        }

        public void Equip(Entity item)
        {
            if (item == null || !item.HasComponent<EquipmentComponent>())
            {
                return;
            }

            var itemEquipment = item.GetComponent<EquipmentComponent>();
            equipment[itemEquipment.Slot] = item;

            if (Entity.HasComponent<FighterComponent>())
            {
                var fighter = Entity.GetComponent<FighterComponent>();
                fighter.Health.Base += itemEquipment.MaximumHealth;
                fighter.Power.Modifier += itemEquipment.Power;
                fighter.Defense.Modifier += itemEquipment.Defence;
            }
        }

        public void Unequip(Entity item)
        {
            if (item == null || !item.HasComponent<EquipmentComponent>())
            {
                return;
            }

            equipment[item.GetComponent<EquipmentComponent>().Slot] = null;

            if (Entity.HasComponent<FighterComponent>())
            {
                var fighter = Entity.GetComponent<FighterComponent>();
                var itemEquipment = item.GetComponent<EquipmentComponent>();

                fighter.Health.Base -= itemEquipment.MaximumHealth;
                fighter.Power.Modifier -= itemEquipment.Power;
                fighter.Defense.Modifier -= itemEquipment.Defence;
            }
        }

        public Entity GetEquipment(EquipmentSlot slot)
        {
            if (!equipment.ContainsKey(slot))
            {
                return null;
            }

            return equipment[slot];
        }
    }
}
