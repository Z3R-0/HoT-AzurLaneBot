using AzurApiLibrary;
using AzurLaneBBot.Core.InteractionHandling;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using AzurLaneBBot.Modules.Events;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReshDiscordNetLibrary;
using System.Configuration;

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
            await serviceProvider.GetRequiredService<Bot>().RunAsync();
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
            serviceCollection.AddSingleton<AzurClient>();
            serviceCollection.AddDbContext<AzurlanedbContext>(options => {
                options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}{ConfigurationManager.AppSettings["dbRelativeLocation"]}");
            });
            serviceCollection.AddSingleton<AzurDbContextDatabaseService>();
            serviceCollection.AddSingleton<ImageService>();
        }

        private static void ConfigureRequiredServices(IServiceProvider serviceProvider) {
            serviceProvider.GetRequiredService<DiscordSocketClient>();
            serviceProvider.GetRequiredService<InteractionService>();
            serviceProvider.GetRequiredService<IServiceProvider>();
        }
    }
}
