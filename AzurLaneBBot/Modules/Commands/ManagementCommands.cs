using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;

namespace AzurLaneBBot.Modules.Commands {
    public class ManagementCommands : InteractionModuleBase<SocketInteractionContext> {
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
                // Check if the user has the requried role
                if (user.Roles.Any(r => r.Name == "Booba Connoisseur")) {
                    await Context.Interaction.RespondWithModalAsync<AddShipModal>(AddShipModalCustomId);
                } else {
                    await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                }
            }
        }

        [ModalInteraction(AddShipModalCustomId)]
        public async Task AddShipModalResponse(AddShipModal modal) {
            await DeferAsync();

            if (!VerifyInputStrings(new List<string> { modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape })) {
                await FollowupAsync("Input was not correctly formatted, could not add new ship to the database");
                return;
            }

            try {
                // Add ship to database
                var result = _dbService.AddBBShip(new BoobaBotProject {
                    Name = modal.Name,
                    Rarity = modal.Rarity,
                    CupSize = modal.Cupsize,
                    CoverageType = modal.CoverageType,
                    Shape = modal.Shape,
                    ImageUrl = ""
                });

                if (result != null) {
                    await FollowupAsync($"Ship '{modal.Name}' added successfully.");
                } else {
                    await FollowupAsync($"Failed to add ship '{modal.Name}'. Please see logs or contact administrator for help and try again later.");
                }
            } catch (Exception ex) {
                await FollowupAsync($"Encountered an error while trying to add the ship to the database, error: {ex.Message}");
            }
        }

        private bool VerifyInputStrings(List<string> inputStrings) {
            foreach (string inputString in inputStrings) {
                if (string.IsNullOrWhiteSpace(inputString))
                    return false;
            }
            return true;
        }
    }
}
