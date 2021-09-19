using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Failure
{
    public class NoCommandFailure : Command
    {
        public string Message { get; set; } = "&tilda&cCommand does not exist!";

        public NoCommandFailure()
        {
            this.CommandType = CommandType.NoCommand;
        }
    }
}
