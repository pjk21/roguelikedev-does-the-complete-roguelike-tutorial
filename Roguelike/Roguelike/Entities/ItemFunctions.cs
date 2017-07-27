using Roguelike.Entities.Components;
using Roguelike.UI;
using System.Drawing;

namespace Roguelike.Entities
{
    public static class ItemFunctions
    {
        public const int HealAmount = 5;

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
