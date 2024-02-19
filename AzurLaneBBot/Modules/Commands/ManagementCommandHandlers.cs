using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    public class ManagementCommandHandlers : BotInteraction<SocketModal> {
        protected IDatabaseService _dbService;

        public ManagementCommandHandlers(AzurlanedbContext azurlanedbContext) {
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
        }

        [ModalInteraction(ManagementCommands.AddShipModalCustomId)]
        public async Task AddShipModalResponse(AddShipModal modal) {
            await DeferAsync();

            if (!VerifyInputStrings(new List<string> { modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape })) {
                await FollowupAsync("Input was not correctly formatted, could not add new ship to the database", ephemeral: true);
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
                    await FollowupAsync($"Failed to add ship '{modal.Name}'. Please see logs or contact administrator for help and try again later.", ephemeral: true);
                }
            } catch (Exception ex) {
                await FollowupAsync($"Encountered an error while trying to add the ship to the database, error: {ex.Message}", ephemeral: true);
            }
        }

        [ModalInteraction(ManagementCommands.UpdateShipModalCustomId)]
        public async Task UpdateShipModalResponse(UpdateShipModal modal) {
            await DeferAsync();

            if (!VerifyInputStrings(new List<string> { modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape })) {
                await FollowupAsync("Input was not correctly formatted, could not add new ship to the database", ephemeral: true);
                return;
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
