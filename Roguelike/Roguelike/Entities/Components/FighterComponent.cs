﻿using Roguelike.UI;
using System;
using System.Collections.Generic;

namespace Roguelike.Entities.Components
{
    public class FighterComponent : Component
    {
        private readonly Dictionary<ElementType, float> resistances = new Dictionary<ElementType, float>();

        public int MaximumHealth { get; set; }
        public int CurrentHealth { get; set; }
        public int Power { get; set; }
        public int Defense { get; set; }

        public ElementType AttackElement { get; set; }

        public Action<Entity> DeathFunction { get; set; }

        public FighterComponent()
        {
            foreach (var element in Enum.GetValues(typeof(ElementType)))
            {
                resistances[(ElementType)element] = 0f;
            }
        }

        public void SetResistance(ElementType element, float value)
        {
            resistances[element] = value.Clamp(-1f, 1f);
        }

        public float GetResistance(ElementType element)
        {
            return resistances[element];
        }

        public void Damage(int amount, Entity source, ElementType element)
        {
            amount -= (int)(amount * GetResistance(element));

            CurrentHealth -= amount;

            if (source != null)
            {
                MessageLog.Add($"{source.Name} attacks {Entity.Name} for {amount} HP.");
            }
            else
            {
                MessageLog.Add($"{Entity.Name} is damaged for {amount} HP.");
            }

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
