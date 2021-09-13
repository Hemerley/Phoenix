using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Request
{
    public class GetCharacterListCommand : Command
    {
        public GetCharacterListCommand()
        {
            this.CommandType = CommandType.CharacterList;
        }
    }
}
