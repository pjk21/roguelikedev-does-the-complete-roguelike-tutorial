using Roguelike.Entities.Components;
using Roguelike.Render;

namespace Roguelike.Entities
{
    public static class MonsterFactory
    {
        public static Entity CreateHound(int x, int y, int dungeonLevel)
        {
            var hound = new Entity("Hound", x, y, 'h', Colours.Hound, true)
            {
                SpriteIndex = EntitySprites.Hound,
                RenderLayer = Renderer.ActorLayer
            };

            var health = 8 + 8 * (dungeonLevel / 3);
            var power = 3 + 3 * (dungeonLevel / 3);
            var defence = 1 + 1 * (dungeonLevel / 3);
            var xp = 20 + 20 * (dungeonLevel / 3);

            hound.AddComponent(new FighterComponent
            {
                MaximumHealth = health,
                CurrentHealth = health,
                Power = power,
                Defense = defence,
                XP = xp,
                DeathFunction = DeathFunctions.MonsterDeath
            });

            hound.AddComponent(new BasicMonsterComponent());

            return hound;
        }

        public static Entity CreateRat(int x, int y, int dungeonLevel)
        {
            var rat = new Entity("Rat", x, y, 'r', Colours.Rat, true)
            {
                SpriteIndex = EntitySprites.Rat,
                RenderLayer = Renderer.ActorLayer
            };

            var health = 3 + 3 * (dungeonLevel / 3);
            var power = 1 + 1 * (dungeonLevel / 3);
            var defence = 0 + 1 * (dungeonLevel / 3);
            var xp = 5 + 5 * (dungeonLevel / 3);

            rat.AddComponent(new FighterComponent
            {
                MaximumHealth = health,
                CurrentHealth = health,
                Power = power,
                Defense = defence,
                XP = xp,
                AttackElement = ElementType.Poison,
                DeathFunction = DeathFunctions.MonsterDeath
            });

            rat.AddComponent(new BasicMonsterComponent());

            return rat;
        }
    }
}
