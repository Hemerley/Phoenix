using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Phoenix.Server.Program;

namespace Phoenix.Server.Bot.Commands
{
    public class UptimeBot : BaseCommandModule
    {
        [Command("uptime")]
        [Description("Returns current server statistics.")]
        public async Task ReturnUptime(CommandContext ctx)
        {
            var totalUpTime = TimeSpan.FromSeconds(DateTimeOffset.Now.ToUnixTimeSeconds() - Program.uptime);
            await ctx.Channel.SendMessageAsync($"The server has been online for: {string.Format("{0:D2} Hours, {1:D2} Minutes, {2:D2} Seconds", totalUpTime.Hours, totalUpTime.Minutes, totalUpTime.Seconds)}. There are currently {game.connectedAccounts.Values.Where(x => x.Account.Character.Name != "").ToList().Count} players online with a total of {game.totalConnections} today, with a maximum of {game.maximumPlayers} since the last reboot.").ConfigureAwait(false);
        }
    }
}
