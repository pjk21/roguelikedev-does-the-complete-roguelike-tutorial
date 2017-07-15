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
                if (camera.Contains(entity.X, entity.Y) && Program.Map.IsInFov(entity.X, entity.Y) || Program.IsDebugModeEnabled)
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
            if (map.IsInFov(x, y) || Program.IsDebugModeEnabled)
            {
                if (map.IsWalkable(x, y))
                {
                    Terminal.Color(Color.White);
                    Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Floor);
                }
                else
                {
                    int offset = GetWallSpriteOffset(map, x, y);

                    Terminal.Color(Color.White);
                    Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Wall + offset);
                }

                if (map.IsInFov(x, y))
                {
                    map.SetCellProperties(x, y, map.IsTransparent(x, y), map.IsWalkable(x, y), true);
                }
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
                        int offset = GetWallSpriteOffset(map, x, y);

                        Terminal.Color(Color.Gray);
                        Terminal.Put(x - camera.X, y - camera.Y, TileSprites.Wall + offset);
                    }
                }
            }
        }

        private int GetWallSpriteOffset(Map map, int x, int y)
        {
            int index = 0;

            if (y > 0 && !map.IsWalkable(x, y - 1))
            {
                index += 1;
            }

            if (x < map.Width - 1 && !map.IsWalkable(x + 1, y))
            {
                index += 2;
            }

            if (y < map.Height - 1 && !map.IsWalkable(x, y + 1))
            {
                index += 4;
            }

            if (x > 0 && !map.IsWalkable(x - 1, y))
            {
                index += 8;
            }

            return index;
        }
    }
}
