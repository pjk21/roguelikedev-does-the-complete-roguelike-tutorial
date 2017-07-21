using Roguelike.Entities.Commands;
using Roguelike.Input;

namespace Roguelike.Entities.Components
{
    public class PlayerInputComponent : ActorComponent
    {
        public override Commands.Command GetCommand()
        {
            switch (InputManager.LastCommand)
            {
                case InputAction.MoveEast:
                    return new MoveCommand(-1, 0);
                case InputAction.MoveWest:
                    return new MoveCommand(1, 0);
                case InputAction.MoveNorth:
                    return new MoveCommand(0, -1);
                case InputAction.MoveSouth:
                    return new MoveCommand(0, 1);
                case InputAction.Rest:
                    return new RestCommand();

                default:
                    return null;
            }
        }
    }
}
