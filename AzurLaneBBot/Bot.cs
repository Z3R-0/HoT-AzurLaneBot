using AzurLaneBBot.Modules.Events;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot;
public class Bot {
    private readonly DiscordSocketClient _client;
    private readonly Ready _ready;
    private readonly InteractionCreated _interactionCreated;
    private readonly IConfiguration _configuration;

    public static DateTime BotStarted;

    public Bot(DiscordSocketClient client, Ready ready, InteractionCreated interactionCreated, IConfiguration configuration) {
        _client = client;
        _ready = ready;
        _interactionCreated = interactionCreated;
        _configuration = configuration;
    }

    public async Task RunAsync() {
        var token = _configuration["DiscordSettings:Token"];
        if (string.IsNullOrEmpty(token)) {
            throw new InvalidOperationException("Bot token is not configured.");
        }

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.SetGameAsync("v2 B) (still gathering waifu data...)");
        await _client.StartAsync();

        _client.Log += Logger.Log;

        _client.Ready += _ready.HandleEventAsync;
        _client.InteractionCreated += _interactionCreated.HandleEventAsync;

        await Task.Delay(-1); // waits indefinitely
    }
}
