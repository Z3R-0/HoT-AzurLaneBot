using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game {
    public class GameModalHandlers() : BotInteraction<SocketModal> {
        [ModalInteraction(GameCommands.GuessShipModalId + "*")]
        public async Task HandleGuessShipModal(string correctShip, GuessShipGameModal modal) {
            await DeferAsync();

            if (modal.Guess.Trim().Equals(correctShip, StringComparison.CurrentCultureIgnoreCase)) {
                await FollowupAsync($"{Context.User.Mention} has guessed correctly!");
            } else {
                await FollowupAsync($"{Context.User.Mention} has guessed wrong!");
            }
        }
    }
}
