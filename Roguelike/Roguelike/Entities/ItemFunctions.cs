﻿using Roguelike.Entities.Components;
using Roguelike.UI;
using System.Drawing;
using System.Linq;

namespace Roguelike.Entities
{
    public static class ItemFunctions
    {
        public const int HealAmount = 5;

        public const int LightningRange = 5;
        public const int LightningDamage = 10;

        public const int ConfuseRange = 8;
        public const int ConfuseTurns = 5;

        public static bool PotionFunction(Entity target)
        {
            if (target.HasComponent<FighterComponent>())
            {
                var healAmount = target.GetComponent<FighterComponent>().Heal(HealAmount);

                if (healAmount > 0)
                {
                    MessageLog.Add($"{target.Name} is healed for {healAmount} HP!", Color.LightGreen);
                    return true;
                }
                else
                {
                    MessageLog.Add($"{target.Name} is already at full health.", Color.Red.Lerp(Color.White));
                    return false;
                }
            }

            return false;
        }

        public static bool LightningScroll(Entity target)
        {
            var closestMonster = Program.Entities
                .Where(e => e.HasComponent<FighterComponent>() && e != Program.Player && e.DistanceTo(Program.Player) <= LightningRange)
                .OrderBy(e => e.DistanceTo(Program.Player))
                .FirstOrDefault();

            if (closestMonster != null)
            {
                MessageLog.Add($"A lightning bolt strikes {closestMonster.Name} for {LightningDamage} HP!", Color.Red);
                closestMonster.GetComponent<FighterComponent>().Damage(LightningDamage, null, ElementType.Electric);
                return true;
            }

            MessageLog.Add($"Lightning scroll can not hit any monsters.", Color.Orange);
            return false;
        }

        public static bool ConfuseScroll(Entity target)
        {
            var closestMonster = Program.Entities
                .Where(e => e.HasComponent<BasicMonsterComponent>() && e != Program.Player && e.DistanceTo(Program.Player) <= ConfuseRange)
                .OrderBy(e => e.DistanceTo(Program.Player))
                .FirstOrDefault();

            if (closestMonster != null)
            {
                MessageLog.Add($"{closestMonster.Name} is confused.", Color.LightBlue);

                var oldAI = closestMonster.GetComponent<BasicMonsterComponent>();
                var newAI = new ConfusedMonsterAI { TurnsRemaining = ConfuseTurns, PreviousAI = oldAI };

                closestMonster.RemoveComponent<BasicMonsterComponent>();
                closestMonster.AddComponent(newAI);
                return true;
            }

            MessageLog.Add($"Confuse scroll can not hit any monsters.", Color.Orange);
            return false;
        }
    }
}
