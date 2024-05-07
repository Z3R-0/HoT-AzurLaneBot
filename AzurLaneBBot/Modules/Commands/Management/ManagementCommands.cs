using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Commands.Modals;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Management {
    public class ManagementCommands(IDatabaseService dbService) : BotInteraction<SocketSlashCommand> {
        private readonly IDatabaseService _dbService = dbService;

        // Consts
        public const int EntriesPerPage = 10;
        // Modal Ids
        public const string AddShipModalCustomId = "add_ship_modal";
        public const string AddSkinModalCustomId = "add_skin_modal";
        public const string UpdateShipModalCustomId = "update_ship_modal";
        // Button Ids
        public const string PreviousButtonCustomId = "prev_button:";
        public const string NextButtonCustomId = "next_button:";

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

        [SlashCommand("delete-ship", "Remove a ship/skin from the database")]
        public async Task HandleDeleteShipSlash(string shipName) {
            if (!(Context.User as SocketGuildUser)!.Roles.Any(r => r.Name != "Booba Connoisseur")) {
                await FollowupAsync("Sorry, you don't have permission to do that.", ephemeral: true);
                return;
            }

            await DeferAsync();

            if (_dbService.GetBBPShip(shipName) != null) {
                _dbService.DeleteBBShip(shipName);

                await FollowupAsync($"'{shipName}' has been removed from the database", ephemeral: true);
            } else {
                await FollowupAsync($"'{shipName}' was not found in the databse", ephemeral: true);
            }
        }

        [SlashCommand("list-db", "List all database entries")]
        public async Task HandleListDbSlash() {
            await DeferAsync();

            var entries = _dbService.GetAllBBPShips();
            var totalEntries = entries.Count();
            var totalPages = (totalEntries + EntriesPerPage - 1) / EntriesPerPage;

            var pagination = DisplayPage(entries!, 1, totalPages, EntriesPerPage);

            await FollowupAsync(embed: pagination.EmbedBuilder.Build(), components: pagination.ComponentBuilder.Build());
        }

        public static PaginationResult DisplayPage(IEnumerable<BoobaBotProject> ships, int currentPage, int totalPages, int entriesPerPage) {
            var embedBuilder = DiscordUtilityMethods.GetEmbedBuilder("Database entries");

            var shipsList = ships
                .Skip((currentPage - 1) * entriesPerPage)
                .Take(entriesPerPage)
                .ToList();

            foreach (var ship in shipsList) {
                if (string.IsNullOrEmpty(ship.IsSkinOf))
                    embedBuilder.AddField(ship.Name, $"Rarity: {ship.Rarity} -- Cup Size: {ship.CupSize} -- Shape: {ship.Shape}");
                else
                    embedBuilder.AddField(ship.Name, $"Is skin of: {ship.IsSkinOf} -- Cup Size: {ship.CupSize} -- Shape: {ship.Shape}");
            }

            var footerText = $"Page {currentPage}/{totalPages}";
            embedBuilder.WithFooter(footer => footer.Text = footerText);

            var buttons = new ComponentBuilder();
            if (currentPage > 1) {
                buttons.WithButton(new ButtonBuilder("Previous", PreviousButtonCustomId + currentPage, ButtonStyle.Primary));
            }
            if (currentPage < totalPages) {
                buttons.WithButton(new ButtonBuilder("Next", NextButtonCustomId + currentPage, ButtonStyle.Primary));
            }

            return new PaginationResult { EmbedBuilder = embedBuilder, ComponentBuilder = buttons };
        }
    }

    public class PaginationResult {
        public required EmbedBuilder EmbedBuilder { get; set; }
        public required ComponentBuilder ComponentBuilder { get; set; }
    }
}
