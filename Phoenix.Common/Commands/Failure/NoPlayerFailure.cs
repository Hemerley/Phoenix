using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Failure
{
    public class NoPlayerFailure : Command
    {
        public string Message { get; set; } = "&tilda&cCould not find player!";

        public NoPlayerFailure()
        {
            this.CommandType = CommandType.NoPlayer;
        }
    }
}
