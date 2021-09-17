namespace Phoenix.Common.Commands.Factory
{
	public enum CommandType
	{

		#region -- Account & Creation (0-99) --

		Unknown = 0,
		Authenticate = 1,
		AuthenticateResponse = 2,
		NewAccount = 3,
		NewAccountResponse = 4,
		NewCharacter = 5,
		NewChracterResponse = 6,
		CharacterList = 7,
		CharacterListResponse = 8,
		CharacterLogin = 9,
		CharacterLoginResponse = 10,

        #endregion

        #region -- Messages (100-199) --

        MessageRoom = 100,
		MessagePlayer = 101,
		MessageParty = 103,
		MessageGuild = 104,

		#endregion

		#region -- Client Updates  (200 - 299) --

		ClientConnect = 200,
		ClientConnectResponse = 201,
		ClientRoom = 202,
		ClientRoomResponse = 203,
		MapRequest = 204,
		MapResponse = 205,
		RoomPlayerUpdate = 206,
		RoomEntityUpdate = 207

        #endregion

        #region -- Server (300-499) --

        #endregion

    }
}
