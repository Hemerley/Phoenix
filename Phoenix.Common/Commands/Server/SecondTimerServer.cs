using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Server
{
    public class SecondTimerServer : Command
    {
        public SecondTimerServer()
        {
            this.CommandType = CommandType.SecondTimer;
        }
    }
}