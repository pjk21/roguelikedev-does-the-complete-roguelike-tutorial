using Roguelike.Entities.Components;
using System.Linq;

namespace Roguelike.Entities.Commands
{
    public class MoveCommand : Command
    {
        public int X { get; }
        public int Y { get; }

        public MoveCommand(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override CommandResult Execute(Entity entity)
        {
            if (X == 0 && Y == 0)
            {
                return new CommandResult(new RestCommand());
            }

            var target = Program.Game.Entities.FirstOrDefault(e => e.HasComponent<FighterComponent>() && e.X == entity.X + X && e.Y == entity.Y + Y);

            if (target != null)
            {
                return new CommandResult(new AttackCommand(target));
            }

            if (!Program.Game.Map.CanEnter(entity.X + X, entity.Y + Y))
            {
                return CommandResult.Failure;
            }

            Program.Game.Map.FovMap.SetCellProperties(entity.X, entity.Y, true, true);

            entity.X += X;
            entity.Y += Y;

            Program.Game.Map.FovMap.SetCellProperties(entity.X, entity.Y, true, false);

            return CommandResult.Success;
        }
    }
}
