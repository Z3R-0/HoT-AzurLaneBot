using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game {
    public class GameButtonHandlers : BotInteraction<SocketMessageComponent> {
        [ComponentInteraction(GameCommands.GuessShipButtonId + "*")]
        public async Task HandleGuessShipButton(string correctShip) {
            await Context.Interaction.RespondWithModalAsync<GuessShipGameModal>(GameCommands.GuessShipModalId + correctShip);
        }
    }
}
