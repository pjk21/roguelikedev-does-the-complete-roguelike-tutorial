using Roguelike.Entities.Commands;
using System;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class ConfusedMonsterAI : ActorComponent
    {
        public int TurnsRemaining { get; set; }
        public ActorComponent PreviousAI { get; set; }

        public override Command GetCommand()
        {
            if (TurnsRemaining > 0)
            {
                TurnsRemaining--;
                var direction = Program.Game.Random.GetPoint(-1, 2, -1, 2);

                if (Program.Game.Map.CanEnter(Entity.X + direction.X, Entity.Y + direction.Y))
                {

                    return new MoveCommand(direction.X, direction.Y);
                }

                return new RestCommand();
            }
            else
            {
                Entity.AddComponent(PreviousAI);
                Entity.RemoveComponent<ConfusedMonsterAI>();

                Entity.Flags &= ~EntityFlags.Confused;

                return new RestCommand();
            }
        }
    }
}
