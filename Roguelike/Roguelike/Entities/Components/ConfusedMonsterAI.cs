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
            var direction = Program.Game.Random.GetPoint(-1, 2, -1, 2);

            if (Program.Game.Map.CanEnter(Entity.X + direction.X, Entity.Y + direction.Y))
            {

                return new MoveCommand(direction.X, direction.Y);
            }

            return new RestCommand();
        }
    }
}
