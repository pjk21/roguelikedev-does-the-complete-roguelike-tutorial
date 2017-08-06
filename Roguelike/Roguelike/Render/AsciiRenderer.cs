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
            var tile = map.IsWalkable(x, y) ? Tile.Floor : Tile.Wall;

            if (map.FovMap.IsInFov(x, y) || Program.Game.IsDebugModeEnabled)
            {
                if (tile.BackColour.HasValue)
                {
                    Terminal.BkColor(tile.BackColour.Value);
                }

                if (tile.Glyph.HasValue)
                {
                    Terminal.Color(tile.Colour ?? Color.White);
                    Terminal.Put(x - camera.X, y - camera.Y, tile.Glyph.Value);
                }

                if (map.FovMap.IsInFov(x, y))
                {
                    map.SetExplored(x, y, true);
                }
            }
            else
            {
                if (map.IsExplored(x, y))
                {
                    if (tile.BackColour.HasValue)
                    {
                        Terminal.BkColor(tile.BackColour.Value.Lerp(Color.Black));
                    }

                    if (tile.Glyph.HasValue)
                    {
                        Terminal.Color((tile.Colour ?? Color.White).Lerp(Color.Black));
                        Terminal.Put(x - camera.X, y - camera.Y, tile.Glyph.Value);
                    }
                }
            }
        }
    }
}
