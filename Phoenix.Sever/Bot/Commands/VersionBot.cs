using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Phoenix.Common.Data;
using System.Threading.Tasks;

namespace Phoenix.Server.Bot.Commands
{
    public class VersionBot : BaseCommandModule
    {
        [Command("version")]
        [Description("Returns current server version.")]
        public async Task ReturnVersion(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"The current server version is: V{Constants.GAME_VERSION}").ConfigureAwait(false);
        }
    }
}
