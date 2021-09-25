using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Server
{
    public class MinuteTimerServer : Command
    {
        public MinuteTimerServer()
        {
            this.CommandType = CommandType.MinuteTimer;
        }
    }
}