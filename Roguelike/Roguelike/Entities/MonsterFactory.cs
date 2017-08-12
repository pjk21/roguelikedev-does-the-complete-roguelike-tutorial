using Roguelike.Entities.Components;
using Roguelike.Render;

namespace Roguelike.Entities
{
    public static class MonsterFactory
    {
        public static Entity CreateHound(int x, int y)
        {
            var hound = new Entity("Hound", x, y, 'h', Colours.Hound, true)
            {
                SpriteIndex = EntitySprites.Hound,
                RenderLayer = Renderer.ActorLayer
            };

            hound.AddComponent(new FighterComponent { MaximumHealth = 8, CurrentHealth = 8, Power = 3, Defense = 1, DeathFunction = DeathFunctions.MonsterDeath });
            hound.AddComponent(new BasicMonsterComponent());

            return hound;
        }

        public static Entity CreateRat(int x, int y)
        {
            var rat = new Entity("Rat", x, y, 'r', Colours.Rat, true)
            {
                SpriteIndex = EntitySprites.Rat,
                RenderLayer = Renderer.ActorLayer
            };

            rat.AddComponent(new FighterComponent { MaximumHealth = 3, CurrentHealth = 3, Power = 1, Defense = 0, AttackElement = ElementType.Poison, DeathFunction = DeathFunctions.MonsterDeath });
            rat.AddComponent(new BasicMonsterComponent());

            return rat;
        }
    }
}
