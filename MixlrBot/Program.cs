using Microsoft.Extensions.Configuration;
using Mixlr.ApiClient.Models.Users;
using MixlrBot.Discord;
using MixlrBot.Mixlr;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MixlrBot
{
    internal class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        private static DiscordWrapper _discordWrapper;
        private static MixlrWrapper _mixlrWrapper;

        private static AutoResetEvent _closing;

        private static bool _reset;
        private static bool _isRunning;

        private static async Task Main(string[] args)
        {
            _closing = new AutoResetEvent(false);

            Console.CancelKeyPress += OnExit;

            Configuration = BuildConfig();

            _reset = true;

            while (_reset)
            {
                await MainLoop();
            }
            _closing.WaitOne();
        }

        private static async Task MainLoop()
        {
            _reset = false;
            _isRunning = true;

            //Set up client wrappers
            _discordWrapper = new DiscordWrapper(Configuration["DiscordBotToken"], Configuration["DiscordGuildId"], 
                Configuration["DiscordRoomId"]);

            var userList = new List<string>();
            var mixlrUsers = Configuration.GetSection("MixlrUsers").GetChildren().ToList();
            foreach (var mxUser in mixlrUsers)
            {
                userList.Add(mxUser.Value);
            }

            _mixlrWrapper = new MixlrWrapper(Configuration["MixlrApiUrl"], userList);

            //Start discord connection
            await _discordWrapper.Login();

            //Set up worker thread
            Console.WriteLine("Checking mixlr users...");

            await Task.Factory.StartNew(async () =>
            {
                while (!_reset && _isRunning)
                {
                    //If we checked less than 5 minutes ago, skip checking again
                    if ((DateTime.Now - _mixlrWrapper.LastUpdate).TotalMinutes <= 1)
                        continue;

                    await CheckMixlrUsers();

                    await CheckLiveMixlrUsers();

                    Thread.Sleep(500);
                }
            });
        }

        private static async Task CheckMixlrUsers()
        {
            foreach (var user in _mixlrWrapper.MixlrUsers)
            {
                var userInfo = await _mixlrWrapper.GetUser(user);

                if (userInfo.IsLive.HasValue && userInfo.IsLive.Value)
                {
                    //Check for broadcast id
                    if (userInfo.BroadcastIds == null || userInfo.BroadcastIds.Count < 1)
                        continue;

                    //Check if already was live
                    if (_mixlrWrapper.LiveMixlrUsers.Any(x => x.Username == userInfo.Username))
                        continue;

                    //Add user to Live list
                    _mixlrWrapper.LiveMixlrUsers.Add(userInfo);

                    //Get broadcast info
                    var broadcastInfo = await _mixlrWrapper.GetBroadcast(userInfo.BroadcastIds[0]);

                    //Send notification
                    Console.WriteLine($"{userInfo.Username} is live. Sending notification...");
                    await _discordWrapper.WritePost(userInfo, broadcastInfo);
                }

                Thread.Sleep(250);
            }
        }

        public static async Task CheckLiveMixlrUsers()
        {
            var noLongerLive = new List<User>();
            foreach (var user in _mixlrWrapper.LiveMixlrUsers)
            {
                var userInfo = await _mixlrWrapper.GetUser(user.Id);

                if (userInfo.IsLive.HasValue && userInfo.IsLive.Value)
                    continue;

                Console.WriteLine($"{user.Username} is no longer live.");
                noLongerLive.Add(user);

                Thread.Sleep(250);
            }

            foreach (var user in noLongerLive)
            {
                Console.WriteLine($"Removing {user.Username} from Live list...");
                _mixlrWrapper.LiveMixlrUsers.Remove(user);
            }
        }

        private static async void OnExit(object sender, ConsoleCancelEventArgs e)
        {
            _isRunning = false;
            await _discordWrapper.Logout();
        }

        private static IConfigurationRoot BuildConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddUserSecrets<Program>();
            return builder.Build();
        }
    }
}
