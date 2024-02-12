using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    public class ManagementCommands : BotInteraction<SocketSlashCommand> {
        protected IDatabaseService _dbService;
        protected IImageService _imageService;

        public const string AddShipModalCustomId = "add_ship_modal";

        public ManagementCommands(AzurlanedbContext azurlanedbContext, ImageService imageService) {
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
            _imageService = imageService;
        }

        [SlashCommand("add-ship", "Add a new ship to the database")]
        public async Task HandleAddShipSlash() {
            if (Context.User is SocketGuildUser user) {
                // Check if the user has the required role
                if (user.Roles.Any(r => r.Name == "Booba Connoisseur")) {
                    await Context.Interaction.RespondWithModalAsync<AddShipModal>(AddShipModalCustomId);
                } else {
                    await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                }
            }
        }
    }
}
