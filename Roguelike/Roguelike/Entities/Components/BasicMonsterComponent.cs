using Roguelike.Entities.Commands;
using RogueSharp;

namespace Roguelike.Entities.Components
{
    public class BasicMonsterComponent : ActorComponent
    {
        public override Command GetCommand()
        {
            var map = Program.Game.Map;

            if (map.IsInFov(Entity.X, Entity.Y))
            {
                var player = Program.Game.Player;

                if (Entity.DistanceTo(player) >= 2)
                {
                    map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, true);
                    map.PathfindingMap.SetCellProperties(player.X, player.Y, true, true);

                    var pathFinder = new PathFinder(map.PathfindingMap);
                    Path path = null;

                    try
                    {
                        path = pathFinder.ShortestPath(map.PathfindingMap.GetCell(Entity.X, Entity.Y), map.PathfindingMap.GetCell(player.X, player.Y));
                    }
                    catch (PathNotFoundException) { }

                    map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, false);
                    map.PathfindingMap.SetCellProperties(player.X, player.Y, true, false);

                    if (path != null)
                    {
                        var nextPosition = path.CurrentStep;
                        return new MoveCommand(nextPosition.X - Entity.X, nextPosition.Y - Entity.Y);
                    }
                }
                else if (Entity.HasComponent<FighterComponent>() && player.GetComponent<FighterComponent>()?.CurrentHealth > 0)
                {
                    return new AttackCommand(player);
                }
            }

            return new RestCommand();
        }
    }
}
