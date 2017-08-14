using System;

namespace Roguelike.Entities
{
    [Serializable]
    public abstract class StatusEffect
    {
        public int Duration { get; set; }

        public StatusEffect(int duration)
        {
            Duration = duration;
        }

        public virtual void Apply(Entity entity)
        {

        }

        public virtual void OnEndTurn(Entity entity)
        {

        }

        public virtual void Remove(Entity entity)
        {

        }
    }
}
