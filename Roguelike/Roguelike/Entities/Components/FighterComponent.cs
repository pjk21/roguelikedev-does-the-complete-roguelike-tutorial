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

        public void Attack(Entity target)
        {
            if (target.GetComponent<FighterComponent>() == null)
            {
                return;
            }

            var damage = Power - target.GetComponent<FighterComponent>().Defense;

            if (damage > 0)
            {
                Console.WriteLine($"{Entity.Name} attacks {target.Name} for {damage} HP. {target.GetComponent<FighterComponent>().CurrentHealth}");
                target.GetComponent<FighterComponent>().Damage(damage);
            }
            else
            {
                Console.WriteLine($"{Entity.Name} attacks {target.Name} but it has no effect!");
            }
        }
    }
}
