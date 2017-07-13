using BearLib;
using Roguelike.World;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike.Render
{
    public class DebugRenderer : Renderer
    {
        public override void RenderEntities(IEnumerable<Entity> entities, Camera camera)
        {
            Terminal.Layer(EntityLayer);

            foreach (var entity in entities)
            {
                if (camera.Contains(entity.X, entity.Y))
                {
                    Terminal.Color(entity.Colour);
                    Terminal.Put(entity.X - camera.X, entity.Y - camera.Y, entity.Character);
                }
            }
        }

        public override void RenderMap(Map map, Camera camera)
        {
            Terminal.Layer(MapLayer);

            for (int x = camera.Left; x < camera.Right; x++)
            {
                for (int y = camera.Top; y < camera.Bottom; y++)
                {
                    if (map.IsWalkable(x, y))
                    {
                        Terminal.BkColor(Colours.FloorLight);
                        Terminal.Put(x - camera.X, y - camera.Y, 0x0020);
                    }
                    else
                    {
                        Terminal.Color(Color.DimGray);
                        Terminal.BkColor(Colours.WallLight);
                        Terminal.Put(x - camera.X, y - camera.Y, 0x2591);
                    }
                }
            }
        }
    }
}
