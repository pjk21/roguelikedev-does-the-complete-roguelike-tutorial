using Roguelike.UI;
using System;

namespace Roguelike.Entities.Components
{
    public class FighterComponent : Component
    {
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

        public int HealPercent(float percent)
        {
            var amount = (int)Math.Round(MaximumHealth * percent);
            amount = amount.Clamp(0, MaximumHealth - CurrentHealth);

            CurrentHealth += amount;

            return (int)amount;
        }
    }
}
