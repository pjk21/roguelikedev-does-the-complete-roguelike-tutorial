using BearLib;
using Roguelike.World;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike.Render
{
    public class SpriteRenderer : Renderer
    {
        public override void RenderEntities(IEnumerable<Entity> entities, Camera camera)
        {
            Terminal.Layer(EntityLayer);

            foreach (var entity in entities)
            {
                if (camera.Contains(entity.X, entity.Y) && Program.Map.IsInFov(entity.X, entity.Y))
                {
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
                }
            }
        }

        protected override void RenderTile(Map map, int x, int y, Camera camera)
        {
            if (map.IsInFov(x, y))
            {
                if (map.IsWalkable(x, y))
                {
                    Terminal.Color(Color.White);
                    Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Floor);
                }
                else
                {
                    Terminal.Color(Color.White);
                    Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Wall);
                }

                map.SetCellProperties(x, y, map.IsTransparent(x, y), map.IsWalkable(x, y), true);
            }
            else
            {
                if (map.IsExplored(x, y))
                {
                    if (map.IsWalkable(x, y))
                    {
                        Terminal.Color(Color.Gray);
                        Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Floor);
                    }
                    else
                    {
                        Terminal.Color(Color.Gray);
                        Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Wall);
                    }
                }
            }
        }
    }
}
