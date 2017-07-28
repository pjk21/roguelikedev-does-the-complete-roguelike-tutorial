using Roguelike.Entities.Commands;
using Roguelike.Input;
using Roguelike.States;
using Roguelike.UI;
using RogueSharp;
using System.Linq;

namespace Roguelike.Entities.Components
{
    public class PlayerInputComponent : ActorComponent
    {
        private Path currentPath;

        private readonly InventoryDialog inventoryDialog = new InventoryDialog(1, 1, Program.MapDisplayWidth - 2, Program.MapDisplayHeight - 2);

        public override Command GetCommand()
        {
            if (currentPath != null)
            {
                if (Program.Entities.Any(e => (e != Entity && e.HasComponent<ActorComponent>() && Program.Map.IsInFov(e.X, e.Y))) || (InputManager.CheckAction(InputAction.MouseMove) || InputManager.AnyKeyPress()))
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
                if (InputManager.CheckAction(InputAction.MoveEast))
                {
                    return new MoveCommand(1, 0);
                }
                else if (InputManager.CheckAction(InputAction.MoveWest))
                {
                    return new MoveCommand(-1, 0);
                }
                else if (InputManager.CheckAction(InputAction.MoveNorth))
                {
                    return new MoveCommand(0, -1);
                }
                else if (InputManager.CheckAction(InputAction.MoveSouth))
                {
                    return new MoveCommand(0, 1);
                }
                else if (InputManager.CheckAction(InputAction.MoveNorthEast))
                {
                    return new MoveCommand(1, -1);
                }
                else if (InputManager.CheckAction(InputAction.MoveNorthWest))
                {
                    return new MoveCommand(-1, -1);
                }
                else if (InputManager.CheckAction(InputAction.MoveSouthEast))
                {
                    return new MoveCommand(1, 1);
                }
                else if (InputManager.CheckAction(InputAction.MoveSouthWest))
                {
                    return new MoveCommand(-1, 1);
                }
                else if (InputManager.CheckAction(InputAction.Rest))
                {
                    return new RestCommand();
                }
                else if (InputManager.CheckAction(InputAction.Take))
                {
                    return new TakeCommand();
                }
                else if (InputManager.CheckAction(InputAction.ShowInventory))
                {
                    return inventoryDialog.Show();
                }
                else if (InputManager.CheckAction(InputAction.MouseMove))
                {
                    return DoMouseMovement();
                }

                return null;
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
