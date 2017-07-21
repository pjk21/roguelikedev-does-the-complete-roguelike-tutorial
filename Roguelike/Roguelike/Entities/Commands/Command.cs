namespace Roguelike.Entities.Commands
{
    public abstract class Command
    {
        public abstract CommandResult Execute(Entity entity);
    }
}
