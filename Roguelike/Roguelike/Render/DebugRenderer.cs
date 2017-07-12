using BearLib;
using Roguelike.World;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike.Render
{
    public class DebugRenderer : Renderer
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
                        Terminal.BkColor(Colours.FloorLight);
                        Terminal.Put(x, y, 0x0020);
                    }
                    else
                    {
                        Terminal.Color(Color.DimGray);
                        Terminal.BkColor(Colours.WallLight);
                        Terminal.Put(x, y, 0x2591);
                    }
                }
            }
        }
    }
}
