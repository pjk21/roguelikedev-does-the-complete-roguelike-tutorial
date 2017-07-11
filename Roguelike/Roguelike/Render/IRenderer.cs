using Roguelike.World;
using System.Collections.Generic;

namespace Roguelike.Render
{
    public interface IRenderer
    {
        void RenderMap(Map map);
        void RenderEntities(IEnumerable<Entity> entities);
    }
}
