using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Test;
public class TestCommands : BotInteraction<SocketSlashCommand> {

    [SlashCommand("ping", "Use to subtly check if the bot is online without sending messages into a channel")]
    public async Task HandlePingAsync() {
        await RespondAsync("Pong!", ephemeral: true);
    }
}
