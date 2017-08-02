using BearLib;
using Roguelike.Entities;
using Roguelike.World;
using System.Drawing;

namespace Roguelike.Render
{
    public class AsciiRenderer : Renderer
    {
        protected override void RenderEntity(Entity entity, Camera camera)
        {
            Terminal.Layer(entity.RenderLayer);

            Terminal.Color(entity.Colour);
            Terminal.Put(entity.X - camera.X, entity.Y - camera.Y, entity.Character);
        }

        protected override void RenderTile(Map map, int x, int y, Camera camera)
        {
            if (map.IsInFov(x, y) || Program.Game.IsDebugModeEnabled)
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
                        Terminal.BkColor(Colours.FloorDark);
                        Terminal.Put(x - camera.X, y - camera.Y, 0x0020);
                    }
                    else
                    {
                        Terminal.Color(Color.DimGray.Lerp(Color.Black));
                        Terminal.BkColor(Colours.WallDark);
                        Terminal.Put(x - camera.X, y - camera.Y, 0x2591);
                    }
                }
            }
        }
    }
}
