using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game;
public class GameModalHandlers() : BotInteraction<SocketModal> {
    [ModalInteraction(GameCommands.GuessShipModalId + "*")]
    public async Task HandleGuessShipModal(string correctShip, GuessShipGameModal modal) {
        await DeferAsync();

        // Normalize both the correct ship name and the user's guess
        var normalizedCorrectShip = DiscordUtilityMethods.NormalizeName(correctShip);
        var normalizedGuess = DiscordUtilityMethods.NormalizeName(modal.Guess);

        if (normalizedGuess.Equals(normalizedCorrectShip, StringComparison.CurrentCultureIgnoreCase)) {
            await FollowupAsync($"{Context.User.Mention} has guessed correctly!");
        } else {
            await FollowupAsync($"{Context.User.Mention} has guessed wrong!");
        }
    }
}
