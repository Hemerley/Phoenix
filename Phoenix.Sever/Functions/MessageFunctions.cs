using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using Phoenix.Server.Connections;
using Phoenix.Server.Data;
using static Phoenix.Server.Program;
using System;
using Phoenix.Server.Logs;
using System.Collections.Generic;

namespace Phoenix.Server.Functions
{
    class MessageFunctions
    {

        public static void MessageRoom(bool mode, string message, ConnectedAccount account = null)
        {
			if (mode)
            {
				foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
				{
					if (connectedAccount.Account.Character != null)
					{
						if (connectedAccount.Account.Character.RoomID == account.Account.Character.RoomID)
						{
							game.SendCommandToClient(connectedAccount.Client, new MessageRoomServer
							{
								Character = account.Account.Character,
								Message = message
							});
						}
					}
				}
			}
		}

		public static void MessageDirect (bool mode, string message, string sending = "", string receiving= "", ConnectedAccount account = null)
        {
			if (mode)
            {
				bool foundPlayer = false;
				foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
				{
					if (connectedAccount.Account.Character != null)
					{
						if (connectedAccount.Account.Character.Name.ToLower() == receiving)
						{
							game.SendCommandToClient(connectedAccount.Client, new MessageDirectServer
							{
								SendingName = sending.FirstCharToUpper(),
								ReceivingName = receiving.FirstCharToUpper(),
								Message = message
							});
							game.SendCommandToClient(account.Client, new MessageDirectServer
							{
								SendingName = sending.FirstCharToUpper(),
								ReceivingName = receiving.FirstCharToUpper(),
								Message = message
							});
							foundPlayer = true;
						}
					}
				}
				if (!foundPlayer)
					game.SendCommandToClient(account.Client, new NoPlayerFailure());
			}
		}
	
		public static void MessageParty()
        {

        }

		public static void MessageGuild()
        {

        }

		public static void MessageWorld(bool mode, string message, int ID, ConnectedAccount account)
        {
			if (true)
            {
				if (account.Account.Character.Id == ID)
				{
					if (account.Account.Character.TypeID > 2)
					{
						foreach (ConnectedAccount connectedAccount in game.connectedAccounts)
						{
							if (connectedAccount.Account.Character != null)
							{
								game.SendCommandToClient(connectedAccount.Client, new MessageWorldServer
								{
									Message = message
								});
							}
						}
					}
					else
					{
						game.SendCommandToClient(account.Client, new NoCommandFailure());
					}
				}
			}
		}

	}
}
