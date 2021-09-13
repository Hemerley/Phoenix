using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common
{
    public class CharacterListResponseCommand : Command
    {
		#region -- Properties --

		public List<Character> Characters { get; set; } = new();

		#endregion

		public CharacterListResponseCommand()
		{
			this.CommandType = CommandType.CharacterListResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			var characters = new List<List<string>>();
            foreach (var character in this.Characters)
            {
				characters.Add(new List<string>
				{
					character.Name,
					character.Caste,
					character.Philosophy
				});
            }
			return characters;
		}
	}
}
