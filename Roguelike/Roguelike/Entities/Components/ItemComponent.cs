using Roguelike.UI;
using System;
using System.Drawing;

namespace Roguelike.Entities.Components
{
    public class ItemComponent : Component
    {
        public const int HealAmount = 5;

        public Func<Entity, bool> UseFunction { get; set; }

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
    }
}
