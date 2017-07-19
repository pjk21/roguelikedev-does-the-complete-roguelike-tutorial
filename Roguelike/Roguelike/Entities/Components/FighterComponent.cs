using Roguelike.UI;
using System;

namespace Roguelike.Entities.Components
{
    public class FighterComponent : Component
    {
        public const float CriticalHitChance = 0.05f;

        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }

        public Action<Entity> DeathFunction { get; set; }

        public void Damage(int amount)
        {
            CurrentHealth -= amount;

            if (CurrentHealth <= 0)
            {
                DeathFunction?.Invoke(Entity);
            }
        }

        public void Attack(Entity target)
        {
            if (target.GetComponent<FighterComponent>() == null)
            {
                return;
            }

            int damage = 0;

            if (Program.Random.NextDouble() < CriticalHitChance)
            {
                damage = Power;
            }
            else
            {
                damage = Power - target.GetComponent<FighterComponent>().Defense;
            }

            if (damage > 0)
            {
                MessageLog.Add($"{Entity.Name} attacks {target.Name} for {damage} HP.");
                target.GetComponent<FighterComponent>().Damage(damage);
            }
            else
            {
                MessageLog.Add($"{Entity.Name} attacks {target.Name} but it has no effect!");
            }
        }

        public int HealPercent(float percent)
        {
            var amount = (int)Math.Round(MaximumHealth * percent);
            amount = amount.Clamp(0, MaximumHealth - CurrentHealth);

            CurrentHealth += amount;

            return (int)amount;
        }
    }
}
