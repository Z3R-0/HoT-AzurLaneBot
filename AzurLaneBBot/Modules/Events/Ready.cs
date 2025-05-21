using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Events;
public class Ready(DiscordSocketClient client, InteractionService interactionService, IConfiguration configuration) : IEvent {
    private readonly DiscordSocketClient _client = client;
    private readonly InteractionService _interactionService = interactionService;
    private readonly IConfiguration _configuration = configuration;

    public async Task HandleEventAsync() {
        Logger.Log($"{_client.CurrentUser.Username} was ready in {(DateTime.Now - Bot.BotStarted).Milliseconds}ms");

        _interactionService.Log += Logger.Log;

        RegisterCommand();

        await Task.CompletedTask;
    }

    private async void RegisterCommand() {
#if DEBUG
        try {
            var debugGuildId = _configuration["DiscordSettings:DebugGuildId"];
            if (string.IsNullOrEmpty(debugGuildId)) {
                throw new InvalidOperationException("Debug guild ID is not configured.");
            }

            Logger.Log("Initializing in DEBUG mode");
            await _interactionService.RegisterCommandsToGuildAsync(ulong.Parse(debugGuildId));
        } catch (Exception e) {
            Logger.Log("Tried to parse a debug guildId from configuration, but failed \n\n" + e.ToString());
        }
#else
        Logger.Log("Initializing in RELEASE mode");
        await _interactionService.RegisterCommandsGloballyAsync();
#endif
    }
}
