using BearLib;
using Roguelike.Entities;
using Roguelike.World;
using System.Drawing;

namespace Roguelike.Render
{
    public class SpriteRenderer : Renderer
    {
        public Color FogColour { get; } = Color.Gray;

        protected override void RenderEntity(Entity entity, Camera camera)
        {
            Terminal.Layer(entity.RenderLayer);

            if (entity.SpriteIndex.HasValue)
            {
                Terminal.Color(entity.SpriteTint);
                Terminal.Put(entity.X - camera.X, entity.Y - camera.Y, entity.SpriteIndex.Value);
            }
            else
            {
                Terminal.Color(entity.Colour);
                Terminal.Put(entity.X - camera.X, entity.Y - camera.Y, entity.Character);
            }

            if (entity.Flags.HasFlag(EntityFlags.Confused))
            {
                Terminal.Put(entity.X - camera.X, entity.Y - camera.Y - 1, EntitySprites.Confusion);
            }
        }

        protected override void RenderTile(Map map, int x, int y, Camera camera)
        {
            var tile = map.IsWalkable(x, y) ? Tile.Floor : Tile.Wall;

            if (map.FovMap.IsInFov(x, y) || Program.Game.IsDebugModeEnabled)
            {
                var spriteIndex = tile.GetSpriteIndex(map, x, y);

                Terminal.Color(tile.SpriteTint);
                Terminal.Put(x - camera.X, y - camera.Y, spriteIndex);

                if (map.FovMap.IsInFov(x, y))
                {
                    map.SetExplored(x, y, true);
                }
            }
            else
            {
                if (map.IsExplored(x, y))
                {
                    int spriteIndex = tile.GetSpriteIndex(map, x, y);

                    Terminal.Color(FogColour);
                    Terminal.Put(x - camera.X, y - camera.Y, spriteIndex);
                }
            }
        }
    }
}
