using Discord.WebSocket;
using Discord;
using AzurLaneBBot.Modules.Events;
using ReshDiscordNetLibrary;
using System.Configuration;

namespace AzurLaneBBot {
    public class Bot {
        private readonly DiscordSocketClient _client;

        private readonly Ready _ready;
        private readonly MessageReceived _messageReceived;
        private readonly InteractionCreated _interactionCreated;

        public static DateTime BotStarted;

        public Bot(DiscordSocketClient client, Ready ready, MessageReceived messageReceived, InteractionCreated interactionCreated) {
            _client = client;
            _ready = ready;
            _messageReceived = messageReceived;
            _interactionCreated = interactionCreated;
        }

        public async Task RunAsync() {
            await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["token"]);
            await _client.SetGameAsync("gathering waifu data...");
            await _client.StartAsync();


            _client.Log += Logger.Log;

            _client.Ready += _ready.HandleEventAsync;
            _client.InteractionCreated += _interactionCreated.HandleEventAsync;
            _client.MessageReceived += _messageReceived.HandleEventAsync;

            await Task.Delay(-1); // waits indefinitely
        }
    }
}
