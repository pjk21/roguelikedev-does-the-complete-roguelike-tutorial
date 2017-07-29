using System;

namespace Roguelike.Entities.Components
{
    public class ItemComponent : Component
    {
        public string Description { get; set; }

        public Func<Entity, bool> UseFunction { get; set; }
    }
}
