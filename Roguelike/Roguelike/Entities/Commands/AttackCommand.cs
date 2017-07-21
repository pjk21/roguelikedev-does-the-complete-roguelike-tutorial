using Roguelike.Entities.Components;
using Roguelike.UI;

namespace Roguelike.Entities.Commands
{
    public class AttackCommand : Command
    {
        public const float CriticalHitChance = 0.05f;

        public Entity Target { get; }

        public AttackCommand(Entity target)
        {
            Target = target;
        }

        public override CommandResult Execute(Entity entity)
        {
            if (Target == null || !entity.HasComponent<FighterComponent>() || !Target.HasComponent<FighterComponent>())
            {
                return CommandResult.Failure;
            }

            int damage = 0;
            int power = entity.GetComponent<FighterComponent>().Power;
            int defense = Target.GetComponent<FighterComponent>().Defense;

            if (Program.Random.NextDouble() < CriticalHitChance)
            {
                damage = power;
            }
            else
            {
                damage = power - defense;
            }

            if (damage > 0)
            {
                MessageLog.Add($"{entity.Name} attacks {Target.Name} for {damage} HP.");
                Target.GetComponent<FighterComponent>().Damage(damage);
            }
            else
            {
                MessageLog.Add($"{entity.Name} attacks {Target.Name} but it has no effect!");
            }

            return CommandResult.Success;
        }
    }
}
