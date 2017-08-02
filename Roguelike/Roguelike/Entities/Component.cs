using System;

namespace Roguelike.Entities
{
    [Serializable]
    public abstract class Component
    {
        public Entity Entity { get; set; }
    }
}
