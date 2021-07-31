using Discord;
using Discord.WebSocket;
using Mixlr.ApiClient.Models.Broadcasts;
using Mixlr.ApiClient.Models.Users;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MixlrBot.Discord
{
    public class DiscordWrapper
    {
        public DateTime LastUpdate { get; set; }

        private readonly DiscordSocketClient _discordClient;
        private readonly string _token;
        private readonly ulong _guildId;
        private readonly ulong _channelId;
        private bool _connected;

        private List<string> _guildIds;
        private List<string> _roomIds;

        #region Constructor

        public DiscordWrapper(string token, string guildId, string channelId)
        {
            this._token = token;
            this._guildId = Convert.ToUInt64(guildId);
            this._channelId = Convert.ToUInt64(channelId);

            _guildIds = new List<string>();
            _roomIds = new List<string>();

            _discordClient = new DiscordSocketClient();

            LastUpdate = DateTime.MinValue;
        }

        #endregion

        #region Public Methods

        public async Task Login()
        {
            await _discordClient.LoginAsync(TokenType.Bot, _token);

            _discordClient.Connected += DiscordConnected;
            _discordClient.Disconnected += DiscordDisconnected;
            await _discordClient.StartAsync();

            //foreach (var guild in _discordClient.Guilds)
            //{
            //    _guildIds.Add(guild.Id.ToString());
            //}
        }

        public async Task Logout()
        {
            await _discordClient.StopAsync();
            await _discordClient.LogoutAsync();
        }

        public async Task WritePost(User user, Broadcast broadcast)
        {
            if (_discordClient.LoginState != LoginState.LoggedIn)
                await Login();

            var retries = 0;
            var maxRetries = 3;

            while (!_connected && retries < maxRetries)
            {
                retries++;
                Thread.Sleep(2000);
            }

            var guild = _discordClient.GetGuild(_guildId);
            var channel = guild.GetTextChannel(_channelId);

            var embedAuthorBuilder = new EmbedAuthorBuilder()
            {
                IconUrl = user.ProfileImageUrl,
                Name = user.Username,
                Url = user.Url
            };

            var embedFieldsBuilders = new List<EmbedFieldBuilder>
            {
                new EmbedFieldBuilder()
                {
                    IsInline = false,
                    Name = "Show Title",
                    Value = broadcast.Title
                }
            };

            var embedBuilder = new EmbedBuilder
            {
                Author = embedAuthorBuilder,
                Color = Color.Red,
                Description = user.AboutMe,
                Fields = embedFieldsBuilders,
                Footer = null,
                ThumbnailUrl = user.ProfileImageUrl,
                Timestamp = broadcast.StartedAt,
            };

            var embed = embedBuilder.Build();

            await channel.SendMessageAsync($"@everyone {user.Username} is live!", false, embed, null, AllowedMentions.All);
        }

        #endregion

        #region Private Methods

        private Task DiscordDisconnected(Exception arg)
        {
            _connected = false;
            return Task.CompletedTask;
        }

        private Task DiscordConnected()
        {
            _connected = true;
            return Task.CompletedTask;
        }

        #endregion
    }
}
