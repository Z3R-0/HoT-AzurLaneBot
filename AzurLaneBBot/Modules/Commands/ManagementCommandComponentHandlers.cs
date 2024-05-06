using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.Models;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    public class ManagementCommandComponentHandlers(IDatabaseService dbService) : BotInteraction<SocketMessageComponent> {
        private readonly IDatabaseService _dbService = dbService;

        [ComponentInteraction(ManagementCommands.NextButtonCustomId + "*")]
        public async Task HandleNextButton(int current) {
            await DeferAsync();
            current++;
            
            await Context.Interaction.Message.DeleteAsync();

            var entries = _dbService.GetAllBBPShips();
            var pagination = ManagementCommands.DisplayPage(entries!, current, (entries.Count() + ManagementCommands.EntriesPerPage - 1) / ManagementCommands.EntriesPerPage, ManagementCommands.EntriesPerPage);

            await FollowupAsync(embed: pagination.EmbedBuilder.Build(), components: pagination.ComponentBuilder.Build());
        }

        [ComponentInteraction(ManagementCommands.PreviousButtonCustomId + "*")]
        public async Task HandlePreviousButton(int current) {
            await DeferAsync();
            current--;

            await Context.Interaction.Message.DeleteAsync();

            var entries = _dbService.GetAllBBPShips();
            var pagination = ManagementCommands.DisplayPage(entries!, current, (entries.Count() + ManagementCommands.EntriesPerPage - 1) / ManagementCommands.EntriesPerPage, ManagementCommands.EntriesPerPage);

            await FollowupAsync(embed: pagination.EmbedBuilder.Build(), components: pagination.ComponentBuilder.Build());
        }
    }
}
