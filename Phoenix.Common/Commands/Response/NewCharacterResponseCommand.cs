using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class NewCharacterResponseCommand : Command
    {
		#region -- Properties --

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public bool Success { get; set; }

		#endregion

		public NewCharacterResponseCommand()
		{
			this.CommandType = CommandType.NewChracterResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Success.ToString()
				}
			};
		}
	}
}
