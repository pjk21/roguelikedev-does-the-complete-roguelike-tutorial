using System;

namespace Roguelike.Entities.Components
{
    public class ItemComponent : Component
    {
        public Func<Entity, bool> UseFunction { get; set; }
    }
}
