using Roguelike.Entities.Commands;
using System;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public abstract class ActorComponent : Component
    {
        public abstract Command GetCommand();
    }
}
