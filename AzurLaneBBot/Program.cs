using AzurLaneBBot.Core.InteractionHandling;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using ReshDiscordNetLibrary;
using AzurLaneBBot.Modules.Events;

namespace AzurLaneBBot {
    internal static class Program {
        private static async Task Main() {
            Bot.BotStarted = DateTime.Now;

            var serviceDescriptors = new ServiceCollection();

            ConfigureServices(serviceDescriptors);

            serviceDescriptors.RegisterBotEvents();

            var serviceProvider = serviceDescriptors.BuildServiceProvider();

            ConfigureRequiredServices(serviceProvider);

            await serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
            serviceProvider.GetRequiredService<Bot>().RunAsync().GetAwaiter().GetResult();
        }

        private static DiscordSocketConfig BuildDiscordSocketConfig() {
            return new DiscordSocketConfig {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages,
                UseInteractionSnowflakeDate = false
            };
        }

        private static void ConfigureServices(IServiceCollection serviceCollection) {
            serviceCollection.AddSingleton<Ready>();
            serviceCollection.AddSingleton<MessageReceived>();
            serviceCollection.AddSingleton<InteractionCreated>();
            serviceCollection.AddSingleton<Bot>();
            serviceCollection.AddSingleton<InteractionService>();
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<InteractionHandler>();
            serviceCollection.AddSingleton(new DiscordSocketClient(BuildDiscordSocketConfig()));
        }

        private static void ConfigureRequiredServices(IServiceProvider serviceProvider) {
            serviceProvider.GetRequiredService<DiscordSocketClient>();
            serviceProvider.GetRequiredService<InteractionService>();
            serviceProvider.GetRequiredService<IServiceProvider>();
        }
    }
}
