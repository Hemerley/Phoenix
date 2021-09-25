using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using Phoenix.Server.Bot.Commands;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Server.Bot
{
    public class Discord
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (FileStream fs = File.OpenRead("config.json"))
            using (StreamReader sr = new(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<DiscordConfig>(json);

            var discordConfig = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            var commandConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true
            };

            this.Client = new DiscordClient(discordConfig);
            this.Client.Ready += OnClientReady;

            this.Commands = this.Client.UseCommandsNext(commandConfig);

            this.Commands.RegisterCommands<UptimeBot>();
            this.Commands.RegisterCommands<VersionBot>();

            await this.Client.ConnectAsync();
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
