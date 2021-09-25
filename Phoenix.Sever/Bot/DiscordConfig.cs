using Newtonsoft.Json;

namespace Phoenix.Server.Bot
{
    public struct DiscordConfig
    {
        [JsonProperty("token")]
        public string Token { get; private set; }
        [JsonProperty("prefix")]
        public string Prefix { get; private set; }
    }
}
