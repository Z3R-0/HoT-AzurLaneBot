using Discord.Commands;
using Discord.Interactions;
using System.Reflection;

namespace AzurLaneBBot.Core.InteractionHandling {
    public class InteractionHandler(CommandService commandService, IServiceProvider serviceProvider, InteractionService interactionService) {
        private readonly CommandService _commandService = commandService;
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        private readonly InteractionService _interactionService = interactionService;

        public async Task InitializeAsync() {
            await _interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            await _commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
        }
    }
}
