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
        NewCharacterResponse = 6,
        CharacterList = 7,
        CharacterListResponse = 8,
        CharacterLogin = 9,
        CharacterLoginResponse = 10,

        #endregion

        #region -- Messages (100-199) --

        MessageRoom = 100,
        MessageDirect = 101,
        MessageParty = 103,
        MessageGuild = 104,
        MessageWorld = 105,
        MessageBroadcast = 106,
        SlashCommand = 107,

        #endregion

        #region -- Client Updates  (200 - 299) --

        ClientConnect = 200,
        ClientConnectResponse = 201,
        ClientRoom = 202,
        ClientRoomResponse = 203,
        MapRequest = 204,
        MapResponse = 205,
        RoomCharacterUpdate = 206,
        RoomNPCUpdate = 207,
        CharacterMoveRequest = 208,
        CharacterStatUpdate = 209,
        RoomItemUpdate = 210,
        ItemLootRequest = 211,

        #endregion

        #region -- Server (300-499) --

        SpawnNPC = 300,
        RespawnCharacter = 301,
        TickTimer = 302,
        SecondTimer = 303,
        MinuteTimer = 304,

        #endregion

        #region -- Staff (500 - 599) --

        SummonPlayer = 500,

        #endregion

        #region -- Failure (900-999) --
        NoCommand = 900,
        NoPlayer = 901

        #endregion

    }
}
