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

        }
    }
}
