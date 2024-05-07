using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game {
    public class GameModalHandlers(IDatabaseService dbService) : BotInteraction<SocketModal> {
        private readonly IDatabaseService _dbService = dbService;

        [ModalInteraction(GameCommands.GuessShipModalId + "*,*")]
        public async Task HandleGuessShipModal(GuessShipGameModal modal, string correctShip) {
            if (modal.Guess.Trim().Equals(correctShip, StringComparison.CurrentCultureIgnoreCase)) {
                await FollowupAsync($"User: {Context.User.Mention} has guessed correctly!");
            } else {
                await FollowupAsync($"User: {Context.User.Mention} has guessed wrong!");
            }
        }
    }
}
