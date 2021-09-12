using System.Collections.Generic;

namespace Phoenix.Common
{
    public class CharacterListResponseCommand : Command
    {
		#region -- Properties --

		public string characters { get; set; }

		#endregion

		public CharacterListResponseCommand()
		{
			this.CommandType = CommandType.CharacterListResponse;
		}

		public override IEnumerable<string> GetCommandParts()
		{
			return new List<string> { characters };
		}
	}
}
