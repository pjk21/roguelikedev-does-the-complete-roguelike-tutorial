using BearLib;
using Roguelike.Entities;
using Roguelike.World;
using System.Collections.Generic;

namespace Roguelike.Render
{
    public abstract class Renderer : IRenderer
    {
        public const int MapLayer = 0;
        public const int EntityLayer = 10;

        public abstract void RenderEntities(IEnumerable<Entity> entities, Camera camera);

        public void RenderMap(Map map, Camera camera)
        {
            Terminal.Layer(MapLayer);

            for (int x = camera.Left; x < camera.Right; x++)
            {
                for (int y = camera.Top; y < camera.Bottom; y++)
                {
                    RenderTile(map, x, y, camera);
                }
            }
        }

        protected abstract void RenderTile(Map map, int x, int y, Camera camera);
    }
}
