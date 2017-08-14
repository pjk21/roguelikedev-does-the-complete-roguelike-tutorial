using Roguelike.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class FighterComponent : Component
    {
        public const int LevelUpBase = 50;
        public const int LevelUpFactor = 50;

        private readonly Dictionary<ElementType, float> resistances = new Dictionary<ElementType, float>();
        private readonly List<StatusEffect> statusEffects = new List<StatusEffect>();

        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }

        public int Level { get; private set; } = 1;
        public int XP { get; set; }
        public int LevelUpRequirement => LevelUpBase + ((Level - 1) * LevelUpFactor);

        public ElementType AttackElement { get; set; }

        public Action<Entity> DeathFunction { get; set; }

        public FighterComponent()
        {
            foreach (var element in Enum.GetValues(typeof(ElementType)))
            {
                resistances[(ElementType)element] = 0f;
            }
        }

        public override void OnAdded()
        {
            Entity.TurnEnd += Entity_TurnEnd;
        }

        public override void OnRemoved()
        {
            Entity.TurnEnd -= Entity_TurnEnd;
        }

        public void SetResistance(ElementType element, float value)
        {
            resistances[element] = value.Clamp(-1f, 1f);
        }

        public float GetResistance(ElementType element)
        {
            return resistances[element];
        }

        public void ApplyStatus(StatusEffect effect)
        {
            statusEffects.Add(effect);
            effect.Apply(Entity);
        }

        public void RemoveStatus(StatusEffect effect)
        {
            effect.Remove(Entity);
            statusEffects.Remove(effect);
        }

        public void Damage(int amount, Entity source, ElementType element)
        {
            amount -= (int)(amount * GetResistance(element));

            CurrentHealth -= amount;

            if (source != null)
            {
                MessageLog.Add($"{source.Name} attacks {Entity.Name} for {amount} HP.", Color.Red);
            }

            if (CurrentHealth <= 0)
            {
                DeathFunction?.Invoke(Entity);

                if (source == Program.Game.Player)
                {
                    source.GetComponent<FighterComponent>().AwardXP(XP);
                }
            }
        }

        public void AwardXP(int amount)
        {
            XP += amount;

            if (XP >= LevelUpRequirement)
            {
                Level++;
                XP = 0;
                MessageLog.Add($"You grow stronger! You have reached level {Level}!", Color.Yellow);

                new LevelUpDialog().Show();

                CurrentHealth = MaximumHealth;
            }
        }

        public int Heal(int amount)
        {
            amount = amount.Clamp(0, MaximumHealth - CurrentHealth);

            CurrentHealth += amount;

            return amount;
        }

        public int HealPercent(float percent)
        {
            var amount = (int)Math.Round(MaximumHealth * percent);
            return Heal(amount);
        }

        private void Entity_TurnEnd()
        {
            var expiringEffects = new List<StatusEffect>();

            foreach (var effect in statusEffects)
            {
                effect.OnEndTurn(Entity);

                effect.Duration--;

                if (effect.Duration <= 0)
                {
                    expiringEffects.Add(effect);
                }
            }

            foreach (var effect in expiringEffects)
            {
                RemoveStatus(effect);
            }
        }
    }
}
