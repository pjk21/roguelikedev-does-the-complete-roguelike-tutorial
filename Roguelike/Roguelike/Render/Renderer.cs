using Roguelike.World;
using System.Collections.Generic;

namespace Roguelike.Render
{
    public abstract class Renderer : IRenderer
    {
        public const int MapLayer = 0;
        public const int EntityLayer = 1;

        public abstract void RenderEntities(IEnumerable<Entity> entities, Camera camera);
        public abstract void RenderMap(Map map, Camera camera);
    }
}
