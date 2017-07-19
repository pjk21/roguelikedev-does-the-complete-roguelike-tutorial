using Roguelike.Entities;
using Roguelike.World;
using System.Collections.Generic;

namespace Roguelike.Render
{
    public interface IRenderer
    {
        void RenderMap(Map map, Camera camera);
        void RenderEntities(IEnumerable<Entity> entities, Camera camera);
        void RenderUI(Camera camera);
    }
}
