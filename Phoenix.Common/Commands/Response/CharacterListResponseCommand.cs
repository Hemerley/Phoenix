using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class CharacterListResponseCommand : Command
    {
        #region -- Properties --

        public bool Success { get; set; }

        public List<Character> Characters { get; set; } = new();

		#endregion

		public CharacterListResponseCommand()
		{
			this.CommandType = CommandType.CharacterListResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			var response = new List<string>();
			var characters = new List<List<string>>();
			response.Add(this.Success.ToString());
			characters.Add(response);
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
