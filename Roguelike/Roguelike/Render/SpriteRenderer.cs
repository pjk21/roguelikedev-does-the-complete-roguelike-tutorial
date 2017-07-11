using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Roguelike.World;
using BearLib;
using System.Drawing;

namespace Roguelike.Render
{
    public class SpriteRenderer : Renderer
    {
        public override void RenderEntities(IEnumerable<Entity> entities)
        {
            Terminal.Layer(EntityLayer);

            foreach (var entity in entities)
            {
                if (entity.SpriteIndex.HasValue)
                {
                    Terminal.Color(entity.SpriteTint);
                    Terminal.Put(entity.X, entity.Y, entity.SpriteIndex.Value);
                }
                else
                {
                    Terminal.Color(entity.Colour);
                    Terminal.Put(entity.X, entity.Y, entity.Character);
                }
            }
        }

        public override void RenderMap(Map map)
        {
            Terminal.Layer(MapLayer);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.IsWalkable(x, y))
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, TileSprites.Floor);
                    }
                    else
                    {
                        Terminal.Color(Color.White);
                        Terminal.Put(x, y, TileSprites.Wall);
                    }
                }
            }
        }
    }
}
