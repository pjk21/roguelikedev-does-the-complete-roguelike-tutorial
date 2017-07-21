namespace Roguelike.Entities.Commands
{
    public class CommandResult
    {
        public static CommandResult Success { get; } = new CommandResult(true);
        public static CommandResult Failure { get; } = new CommandResult(false);

        public bool Result { get; }
        public Command Alternative { get; }

        private CommandResult(bool result)
        {
            Result = result;
        }

        public CommandResult(Command alternative)
        {
            Result = false;
            Alternative = alternative;
        }
    }
}
