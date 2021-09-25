using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Server
{
    public class TickTimerServer : Command
    {
        public TickTimerServer()
        {
            this.CommandType = CommandType.TickTimer;
        }
    }
}