using System.Threading.Tasks;

namespace Phoenix.Server.Bot.Commands
{
    /// <summary>
    /// 882937683616350258
    /// </summary>
    class MessageChannelBot
    {
        public static async Task MessageChannel(ulong channel, string message)
        {
            var socket = await Discord.Client.GetChannelAsync(channel);
            await Discord.Client.SendMessageAsync(socket, message);
        }
    }
}
