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
                    Program.Map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, true);
                    Program.Map.PathfindingMap.SetCellProperties(Program.Player.X, Program.Player.Y, true, true);

                    var pathFinder = new PathFinder(Program.Map.PathfindingMap);
                    Path path = null;

                    try
                    {
                        path = pathFinder.ShortestPath(Program.Map.PathfindingMap.GetCell(Entity.X, Entity.Y), Program.Map.PathfindingMap.GetCell(Program.Player.X, Program.Player.Y));
                    }
                    catch (PathNotFoundException) { }

                    Program.Map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, false);
                    Program.Map.PathfindingMap.SetCellProperties(Program.Player.X, Program.Player.Y, true, false);

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
