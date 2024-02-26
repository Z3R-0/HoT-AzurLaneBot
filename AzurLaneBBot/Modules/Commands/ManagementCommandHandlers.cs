using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "It is unncessary to run the base constructor when initializing Discord.NET components")]
    public class ManagementCommandHandlers : BotInteraction<SocketModal> {
        protected IDatabaseService _dbService;

        public ManagementCommandHandlers(AzurlanedbContext azurlanedbContext) {
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
        }

        [ModalInteraction(ManagementCommands.AddShipModalCustomId)]
        public async Task AddShipModalResponse(AddShipModal modal) {
            await DeferAsync();

            if (!VerifyInput([modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape], isAdd: true)) {
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

            if (!VerifyInput([modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape], isAdd: false)) {
                await FollowupAsync("Input was not correctly formatted, could not add new ship to the database", ephemeral: true);
                return;
            }

            try {
                var result = _dbService.UpdateBBShip(new BoobaBotProject {
                    Name = modal.Name,
                    Rarity = modal.Rarity,
                    CupSize = modal.Cupsize,
                    CoverageType = modal.CoverageType,
                    Shape = modal.Shape,
                    ImageUrl = ""
                });
            } catch (Exception ex) {
                await FollowupAsync($"Encountered an error while trying to add the ship to the database, error: {ex.Message}", ephemeral: true);
            }
        }

        private bool VerifyInput(List<string> inputStrings, bool isAdd) {
            foreach (string inputString in inputStrings) {
                if (string.IsNullOrWhiteSpace(inputString))
                    return false;
            }

            if (isAdd) {
                var check = _dbService.GetBBPShip(inputStrings[0]);

                if (check == null)
                    return false;
            }

            return true;
        }
    }
}
