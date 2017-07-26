using Roguelike.Entities.Commands;
using Roguelike.Input;
using Roguelike.States;
using RogueSharp;
using System.Linq;

namespace Roguelike.Entities.Components
{
    public class PlayerInputComponent : ActorComponent
    {
        private Path currentPath;

        public override Command GetCommand()
        {
            if (currentPath != null)
            {
                if (Program.Entities.Any(e => e != Entity && Program.Map.IsInFov(e.X, e.Y)) || InputManager.LastCommand == InputAction.ClickMove)
                {
                    currentPath = null;
                    return null;
                }

                var destination = currentPath.StepForward();

                if (!Program.Map.CanEnter(destination.X, destination.Y))
                {
                    currentPath = null;
                    return null;
                }

                var x = destination.X - Entity.X;
                var y = destination.Y - Entity.Y;

                if (destination == currentPath.End)
                {
                    currentPath = null;
                }

                return new MoveCommand(x, y);
            }
            else
            {
                switch (InputManager.LastCommand)
                {
                    case InputAction.MoveEast:
                        return new MoveCommand(1, 0);
                    case InputAction.MoveWest:
                        return new MoveCommand(-1, 0);
                    case InputAction.MoveNorth:
                        return new MoveCommand(0, -1);
                    case InputAction.MoveSouth:
                        return new MoveCommand(0, 1);
                    case InputAction.MoveNorthEast:
                        return new MoveCommand(1, -1);
                    case InputAction.MoveNorthWest:
                        return new MoveCommand(-1, -1);
                    case InputAction.MoveSouthEast:
                        return new MoveCommand(1, 1);
                    case InputAction.MoveSouthWest:
                        return new MoveCommand(-1, 1);

                    case InputAction.Rest:
                        return new RestCommand();
                    case InputAction.Take:
                        return new TakeCommand();

                    case InputAction.ClickMove:
                        return DoMouseMovement();

                    default:
                        return null;
                }
            }
        }

        private Command DoMouseMovement()
        {
            var destination = InputManager.GetMouseWorldPosition(GameState.Camera);

            if (Entity.X == destination.X && Entity.Y == destination.Y)
            {
                return null;
            }

            if ((Program.Map.IsInFov(destination.X, destination.Y) || Program.Map.IsExplored(destination.X, destination.Y)) && Program.Map.CanEnter(destination.X, destination.Y))
            {
                Program.Map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, true);

                var pathFinder = new PathFinder(Program.Map.PathfindingMap);
                currentPath = null;

                try
                {
                    currentPath = pathFinder.ShortestPath(Program.Map.PathfindingMap.GetCell(Entity.X, Entity.Y), Program.Map.PathfindingMap.GetCell(destination.X, destination.Y));
                    Program.Map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, true);
                }
                catch (PathNotFoundException)
                {
                    Program.Map.PathfindingMap.SetCellProperties(Entity.X, Entity.Y, true, false);
                    return null;
                }

                var x = currentPath.CurrentStep.X - Entity.X;
                var y = currentPath.CurrentStep.Y - Entity.Y;

                if (currentPath.Length == 1)
                {
                    currentPath = null;
                }

                return new MoveCommand(x, y);
            }

            return null;
        }
    }
}
