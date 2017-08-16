using Roguelike.Entities.Commands;
using RogueSharp;
using System;

namespace Roguelike.Entities.Components
{
    [Serializable]
    public class BasicMonsterComponent : ActorComponent
    {
        public override Command GetCommand()
        {
            var map = Program.Game.Map;

            if (map.FovMap.IsInFov(Entity.X, Entity.Y))
            {
                var player = Program.Game.Player;

                if (Entity.DistanceTo(player) >= 2)
                {
                    map.FovMap.SetCellProperties(Entity.X, Entity.Y, true, true);
                    map.FovMap.SetCellProperties(player.X, player.Y, true, true);

                    var pathFinder = new PathFinder(map.FovMap);
                    Path path = null;

                    try
                    {
                        path = pathFinder.ShortestPath(map.FovMap.GetCell(Entity.X, Entity.Y), map.FovMap.GetCell(player.X, player.Y));
                    }
                    catch (PathNotFoundException) { }

                    map.FovMap.SetCellProperties(Entity.X, Entity.Y, true, false);
                    map.FovMap.SetCellProperties(player.X, player.Y, true, false);

                    if (path != null)
                    {
                        var nextPosition = path.CurrentStep;
                        return new MoveCommand(nextPosition.X - Entity.X, nextPosition.Y - Entity.Y);
                    }
                }
                else if (Entity.HasComponent<FighterComponent>() && player.GetComponent<FighterComponent>()?.Health.Value > 0)
                {
                    return new AttackCommand(player);
                }
            }

            return new RestCommand();
        }
    }
}
