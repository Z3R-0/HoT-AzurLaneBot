using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "It is unncessary to run the base constructor when initializing Discord.NET components")]
    public class ManagementCommands : BotInteraction<SocketSlashCommand> {
        protected IDatabaseService _dbService;
        protected IImageService _imageService;

        public const string AddShipModalCustomId = "add_ship_modal";
        public const string AddSkinModalCustomId = "add_skin_modal";
        public const string UpdateShipModalCustomId = "update_ship_modal";

        public ManagementCommands(AzurlanedbContext azurlanedbContext, ImageService imageService) {
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
            _imageService = imageService;
        }

        [SlashCommand("add-ship", "Add a new ship to the database")]
        public async Task HandleAddShipSlash() {
            if (!(Context.User as SocketGuildUser)!.Roles.Any(r => r.Name != "Booba Connoisseur")) {
                await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                return;
            }

            await Context.Interaction.RespondWithModalAsync<AddShipModal>(AddShipModalCustomId);
        }

        [SlashCommand("add-skin", "Add a new skin to the database")]
        public async Task HandleAddSkinSlash() {
            if (!(Context.User as SocketGuildUser)!.Roles.Any(r => r.Name != "Booba Connoisseur")) {
                await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                return;
            }

            await Context.Interaction.RespondWithModalAsync<AddSkinModal>(AddSkinModalCustomId);
        }

        [SlashCommand("update-ship", "Update a ship from the database")]
        public async Task HandleUpdateShipSlash() {
            if (!(Context.User as SocketGuildUser)!.Roles.Any(r => r.Name != "Booba Connoisseur")) {
                await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                return;
            }

            await Context.Interaction.RespondWithModalAsync<UpdateShipModal>(UpdateShipModalCustomId);
        }
    }
}
