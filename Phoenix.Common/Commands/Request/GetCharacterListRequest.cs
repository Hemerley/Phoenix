using Phoenix.Common.Commands.Factory;

namespace Phoenix.Common.Commands.Request
{
    public class GetCharacterListRequest : Command
    {
        public GetCharacterListRequest()
        {
            this.CommandType = CommandType.CharacterList;
        }
    }
}
