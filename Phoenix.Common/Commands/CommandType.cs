namespace Phoenix.Common
{
	public enum CommandType
	{
		#region ---Account & Creation (0-99)---

		Unknown = 0,
		Authenticate = 1,
		AuthenticateResponse = 2,
		NewAccount = 3,
		NewCharacter = 4,
		Connected = 5,

		#endregion

		#region ---Messages (100-199)---

		MessageRoom = 100,
		MessagePlayer = 101,
		MessageParty = 103,
		MessageGuild = 104
        
		#endregion
    }
}
