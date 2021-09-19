using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class SpawnEntityServer : Command
    {
		#region -- Properties --

		public string EntityName { get; set; } = "\0";
		public string CharacterName { get; set; } = "\0";

        #endregion

        public SpawnEntityServer()
		{
			this.CommandType = CommandType.SpawnEntity;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.EntityName,
					this.CharacterName
				}
			};
		}
	}
}
