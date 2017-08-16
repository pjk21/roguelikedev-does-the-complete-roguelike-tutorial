using System;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class EquipmentComponent : Component
    {
        public EquipmentSlot Slot { get; }

        public int MaximumHealth { get; set; }
        public int Power { get; set; }
        public int Defence { get; set; }

        public EquipmentComponent(EquipmentSlot slot)
        {
            Slot = slot;
        }
    }
}
