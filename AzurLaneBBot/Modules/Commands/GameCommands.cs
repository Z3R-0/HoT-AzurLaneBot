using AzurLaneBBot.Database.DatabaseServices;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    public class GameCommands(IDatabaseService dbService) : BotInteraction<SocketSlashCommand> {
        private readonly IDatabaseService _dbService = dbService;

        [SlashCommand("guess-ship", "Guess a ship (or skin)")]
        public async Task HandleGuessShipSlash(
            [Choice("Yes", "Yes")]
            [Choice("No", "No")]
            string allowSkins) {
            var legalShipChoices = _dbService.GetAllBBPShips().Where(
                ship => !string.IsNullOrEmpty(ship!.ImageUrl)
                && (allowSkins == "No" && string.IsNullOrEmpty(ship.IsSkinOf)));

            var conditionalText = allowSkins == "Yes" ? " or skin " : "";

            var embedBuilder = DiscordUtilityMethods.GetEmbedBuilder($"What is the name of this ship{conditionalText}?");
        }
    }
}
