using System;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class ItemComponent : Component
    {
        public string Description { get; set; }

        public Func<Entity, bool> UseFunction { get; set; }
    }
}
