using BearLib;
using Roguelike.World;
using System.Collections.Generic;

namespace Roguelike.Render
{
    public class AsciiRenderer : Renderer
    {
        public override void RenderEntities(IEnumerable<Entity> entities)
        {
            Terminal.Layer(EntityLayer);

            foreach (var entity in entities)
            {
                Terminal.Color(entity.Colour);
                Terminal.Put(entity.X, entity.Y, entity.Character);
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
                        Terminal.BkColor(Colours.FloorDark);
                    }
                    else
                    {
                        Terminal.BkColor(Colours.WallDark);
                    }

                    Terminal.Put(x, y, 0x0020);
                }
            }
        }
    }
}
