using Roguelike.Entities.Commands;

namespace Roguelike.Entities.Components
{
    public abstract class ActorComponent : Component
    {
        public abstract Command GetCommand();
    }
}
