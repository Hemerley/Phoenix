using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;

namespace Phoenix.Common.Commands.Request
{
    public class ClientConnectRequest : Command
    {
        #region -- Properties --

        public int Id { get; set; } = new();

        #endregion

        public ClientConnectRequest()
		{
			this.CommandType = CommandType.ClientConnect;
		}

		public override IEnumerable<IEnumerable<string>> GetCommandParts()
		{

			return new List<List<string>>
			{
				new List<string>
				{
					this.Id.ToString()
                }
			};
		}
	}
}
