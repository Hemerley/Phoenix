using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Response
{
    public class NewCharacterResponse : Command
    {
		#region -- Properties --

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public bool Success { get; set; }
		public string Message { get; set; } = "\0";

        #endregion

        public NewCharacterResponse()
		{
			this.CommandType = CommandType.NewCharacterResponse;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{
			return new List<List<string>>
			{
				new List<string>
				{
					this.Success.ToString(),
					this.Message
				}
			};
		}
	}
}
