using Roguelike.Entities.Commands;
using RogueSharp;

namespace Roguelike.Entities.Components
{
    public class BasicMonsterComponent : ActorComponent
    {
        public override Command GetCommand()
        {
            if (Program.Map.IsInFov(Entity.X, Entity.Y))
            {
                if (Entity.DistanceTo(Program.Player) >= 2)
                {
                    var path = new PathFinder(Program.Map).ShortestPath(Program.Map.GetCell(Entity.X, Entity.Y), Program.Map.GetCell(Program.Player.X, Program.Player.Y));

                    if (path != null)
                    {
                        var nextPosition = path.CurrentStep;
                        return new MoveCommand(nextPosition.X - Entity.X, nextPosition.Y - Entity.Y);
                    }
                }
                else if (Entity.HasComponent<FighterComponent>() && Program.Player.GetComponent<FighterComponent>()?.CurrentHealth > 0)
                {
                    return new AttackCommand(Program.Player);
                }
            }

            return new RestCommand();
        }
    }
}
