using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game {
    public class GameButtonHandlers : BotInteraction<SocketMessageComponent> {
        [ComponentInteraction(GameCommands.GuessShipButtonId + "*")]
        public async Task HandleGuessShipButton(ulong originalUserId) {
            var isOriginalUser = Context.User.Id == originalUserId;


        }
    }
}
