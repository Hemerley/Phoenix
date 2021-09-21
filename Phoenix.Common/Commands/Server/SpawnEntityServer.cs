using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Server
{
    public class SpawnNPCServer : Command
    {
		#region -- Properties --

		public string NPCName { get; set; } = "\0";
		public string CharacterName { get; set; } = "\0";

        #endregion

        public SpawnNPCServer()
		{
			this.CommandType = CommandType.SpawnNPC;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.NPCName,
					this.CharacterName
				}
			};
		}
	}
}
