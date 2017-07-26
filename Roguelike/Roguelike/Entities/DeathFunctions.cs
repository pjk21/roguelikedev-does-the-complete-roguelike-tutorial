using Roguelike.Entities.Components;
using Roguelike.Render;
using Roguelike.UI;
using System.Drawing;

namespace Roguelike.Entities
{
    public static class DeathFunctions
    {
        public static void PlayerDeath(Entity entity)
        {
            MessageLog.Add("You died!");

            entity.Character = '%';
            entity.Colour = Color.Red;
            entity.SpriteIndex = EntitySprites.TombStone;
            entity.RenderLayer = Renderer.CorpseLayer;
        }

        public static void MonsterDeath(Entity entity)
        {
            MessageLog.Add($"The {entity.Name} died!");

            entity.Character = '%';
            entity.Colour = Color.Red;
            entity.SpriteIndex = EntitySprites.TombStone;
            entity.RenderLayer = Renderer.CorpseLayer;

            entity.IsSolid = false;

            entity.RemoveComponent<FighterComponent>();
            entity.RemoveComponent<BasicMonsterComponent>();

            entity.Name = $"Corpse of {entity.Name}";
        }
    }
}
