using System.Collections.Generic;

namespace Phoenix.Common
{
    public class NewAccountResponseCommand : Command
    {
		#region -- Properties --

		/// <summary>
		/// 0 - Fail
		/// 1 - Success
		/// </summary>
		public int Success { get; set; }

		#endregion

		public NewAccountResponseCommand()
		{
			this.CommandType = CommandType.NewAccountResponse;
		}

		public override IEnumerable<string> GetCommandParts()
		{
			return new List<string> { this.Success.ToString() };
		}
	}
}
