using Roguelike.Entities.Components;
using System;

namespace Roguelike.Entities.StatusEffects
{
    [Serializable]
    public class ConfusionEffect : StatusEffect
    {
        private ActorComponent previousActorComponent;

        public ConfusionEffect(int duration)
            : base(duration)
        {

        }

        public override void Apply(Entity entity)
        {
            previousActorComponent = entity.GetComponent<ActorComponent>();
            entity.RemoveComponent(previousActorComponent);

            entity.AddComponent(new ConfusedMonsterAI());
            entity.Tags.Add(Tags.Confused);
        }

        public override void Remove(Entity entity)
        {
            entity.RemoveComponent<ConfusedMonsterAI>();
            entity.Tags.Remove(Tags.Confused);

            entity.AddComponent(previousActorComponent);
        }
    }
}
