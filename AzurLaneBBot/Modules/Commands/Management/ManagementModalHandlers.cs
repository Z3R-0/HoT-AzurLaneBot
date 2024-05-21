using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;
using System.Text.RegularExpressions;

namespace AzurLaneBBot.Modules.Commands.Management {
    public class ManagementModalHandlers(IDatabaseService dbService) : BotInteraction<SocketModal> {
        private readonly IDatabaseService _dbService = dbService;

        [ModalInteraction(ManagementCommands.AddShipModalCustomId)]
        public async Task AddShipModalResponse(AddShipModal modal) {
            await DeferAsync();

            modal.Name = Regex.Replace(modal.Name, @"[^a-zA-Z0-9äöüÄÖÜẞ ]", string.Empty);

            if (!VerifyInput([modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape], isAdd: true, out var message)) {
                await FollowupAsync(message, ephemeral: true);
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

        [ModalInteraction(ManagementCommands.AddSkinModalCustomId)]
        public async Task AddSkinModalResponse(AddSkinModal modal) {
            await DeferAsync();

            modal.Name = Regex.Replace(modal.Name, @"[^a-zA-Z0-9äöüÄÖÜẞ ]", string.Empty);

            if (!VerifyInput([modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape], isAdd: true, out var message)) {
                await FollowupAsync(message, ephemeral: true);
                return;
            }

            try {
                // Add skin to database
                var result = _dbService.AddBBShip(new BoobaBotProject {
                    Name = modal.Name,
                    IsSkinOf = modal.IsSkinOf,
                    CupSize = modal.Cupsize,
                    CoverageType = modal.CoverageType,
                    Shape = modal.Shape,
                    ImageUrl = ""
                });

                if (result != null) {
                    await FollowupAsync($"Skin '{modal.Name}' added successfully.");
                } else {
                    await FollowupAsync($"Failed to add skin '{modal.Name}'. Please see logs or contact administrator for help and try again later.", ephemeral: true);
                }
            } catch (Exception ex) {
                await FollowupAsync($"Encountered an error while trying to add the skin to the database, error: {ex.Message}", ephemeral: true);
            }
        }

        [ModalInteraction(ManagementCommands.UpdateShipModalCustomId + "*")]
        public async Task UpdateShipModalResponse(string originalName, UpdateShipModal modal) {
            await DeferAsync();

            modal.Name = Regex.Replace(modal.Name, @"[^a-zA-Z0-9äöüÄÖÜẞ ]", string.Empty);

            if (!VerifyInput([modal.Name, modal.Cupsize, modal.CoverageType, modal.Shape], isAdd: false, out var message)) {
                await FollowupAsync(message, ephemeral: true);
                return;
            }

            try {
                var result = _dbService.UpdateBBShip(new BoobaBotProject {
                    Name = modal.Name,
                    Rarity = modal.Rarity,
                    CupSize = modal.Cupsize,
                    CoverageType = modal.CoverageType,
                    Shape = modal.Shape,
                }, originalName);

                await FollowupAsync($"Succesfully updated {modal.Name}");
            } catch (Exception ex) {
                await FollowupAsync($"Encountered an error while trying to add the ship to the database, error: {ex.Message}", ephemeral: true);
            }
        }

        private bool VerifyInput(List<string> inputStrings, bool isAdd, out string message) {
            foreach (var inputString in inputStrings) {
                if (string.IsNullOrWhiteSpace(inputString)) {
                    message = "Input was not correctly formatted, could not add new ship to the database...";
                    return false;
                }
            }

            if (isAdd) {
                var check = _dbService.GetBBPShip(inputStrings[0]);

                if (check != null) {
                    message = "Ship/Skin already exists in the database.";
                    return false;
                }
            }

            message = "No issues found with input";
            return true;
        }
    }
}
