using AzurLaneBBot.Modules.Events;
using Discord;
using Discord.WebSocket;
using ReshDiscordNetLibrary;
using System.Configuration;

namespace AzurLaneBBot;
public class Bot {
    private readonly DiscordSocketClient _client;

    private readonly Ready _ready;
    private readonly InteractionCreated _interactionCreated;

    public static DateTime BotStarted;

    public Bot(DiscordSocketClient client, Ready ready, InteractionCreated interactionCreated) {
        _client = client;
        _ready = ready;
        _interactionCreated = interactionCreated;
    }

    public async Task RunAsync() {
        await _client.LoginAsync(TokenType.Bot, ConfigurationManager.AppSettings["token"]);
        await _client.SetGameAsync("v2 B) (still gathering waifu data...)");
        await _client.StartAsync();

        _client.Log += Logger.Log;

        _client.Ready += _ready.HandleEventAsync;
        _client.InteractionCreated += _interactionCreated.HandleEventAsync;

        await Task.Delay(-1); // waits indefinitely
    }
}
