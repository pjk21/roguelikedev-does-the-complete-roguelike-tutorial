using BearLib;
using Roguelike.World;
using System.Collections.Generic;
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
                if (Program.Map.IsInFov(entity.X, entity.Y))
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
        }

        public override void RenderMap(Map map)
        {
            Terminal.Layer(MapLayer);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.IsInFov(x, y))
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

                        map.SetCellProperties(x, y, map.IsTransparent(x, y), map.IsWalkable(x, y), true);
                    }
                    else
                    {
                        if (map.IsExplored(x, y))
                        {
                            if (map.IsWalkable(x, y))
                            {
                                Terminal.Color(Color.Gray);
                                Terminal.Put(x, y, TileSprites.Floor);
                            }
                            else
                            {
                                Terminal.Color(Color.Gray);
                                Terminal.Put(x, y, TileSprites.Wall);
                            }
                        }
                    }
                }
            }
        }
    }
}
