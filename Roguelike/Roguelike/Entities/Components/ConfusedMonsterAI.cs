using Roguelike.Entities.Commands;

namespace Roguelike.Entities.Components
{
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

                return new RestCommand();
            }
        }
    }
}
