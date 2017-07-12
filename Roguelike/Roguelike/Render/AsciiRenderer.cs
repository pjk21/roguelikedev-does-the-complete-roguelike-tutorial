using BearLib;
using Roguelike.World;
using System.Collections.Generic;
using System.Drawing;

namespace Roguelike.Render
{
    public class AsciiRenderer : Renderer
    {
        public override void RenderEntities(IEnumerable<Entity> entities)
        {
            Terminal.Layer(EntityLayer);

            foreach (var entity in entities)
            {
                if (Program.Map.IsInFov(entity.X, entity.Y))
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
                    if (map.IsInFov(x, y))
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

                        map.SetCellProperties(x, y, map.IsTransparent(x, y), map.IsWalkable(x, y), true);
                    }
                    else
                    {
                        if (map.IsExplored(x, y))
                        {
                            if (map.IsWalkable(x, y))
                            {
                                Terminal.BkColor(Colours.FloorDark);
                                Terminal.Put(x, y, 0x0020);
                            }
                            else
                            {
                                Terminal.Color(Color.FromArgb(Color.DimGray.R / 2, Color.DimGray.G / 2, Color.DimGray.B / 2));
                                Terminal.BkColor(Colours.WallDark);
                                Terminal.Put(x, y, 0x2591);
                            }
                        }
                    }
                }
            }
        }
    }
}
