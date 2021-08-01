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
        //private readonly ulong _guildId;
        //private readonly ulong _channelId;
        private List<GuildAccessOptions> _guilds;
        private bool _connected;

        #region Constructor

        public DiscordWrapper(string token, string guildId, string channelId)
        {
            this._token = token;
            //this._guildId = Convert.ToUInt64(guildId);
            //this._channelId = Convert.ToUInt64(channelId);

            this._guilds = new List<GuildAccessOptions>();
            this._guilds.Add(new GuildAccessOptions
            {
                GuildId = Convert.ToUInt64(guildId),
                ChannelId = Convert.ToUInt64(channelId)
            });

            _discordClient = new DiscordSocketClient();

            LastUpdate = DateTime.MinValue;
        }

        public DiscordWrapper(string token, List<GuildAccessOptions> guildAccess)
        {
            this._token = token;
            this._guilds = guildAccess;

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

            foreach (var guild in _guilds)
            {
                var guildInfo = _discordClient.GetGuild(guild.GuildId);
                var channel = guildInfo.GetTextChannel(guild.ChannelId);

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
                    },
                    new EmbedFieldBuilder()
                    {
                        IsInline = false,
                        Name = "Hearts",
                        Value = broadcast.HeartCount ?? 0
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
