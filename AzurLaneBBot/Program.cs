using Application;
using Application.Interfaces;
using AzurLaneBBot.Core.InteractionHandling;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Modules.Events;
using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReshDiscordNetLibrary;
using System.Configuration;

namespace AzurLaneBBot;
public static class Program {
    private static async Task Main() {
        Bot.BotStarted = DateTime.Now;

        var serviceDescriptors = new ServiceCollection();

        ConfigureServices(serviceDescriptors);

        serviceDescriptors.RegisterBotEvents();

        var serviceProvider = serviceDescriptors.BuildServiceProvider();

        ConfigureRequiredServices(serviceProvider);

        // Log unhandled exceptions
        AppDomain.CurrentDomain.UnhandledException += Domain_UnhandledException;

        await serviceProvider.GetRequiredService<InteractionHandler>().InitializeAsync();
        await serviceProvider.GetRequiredService<Bot>().RunAsync();
    }

    private static void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
        var exc = (Exception)e.ExceptionObject;

        Logger.Log($"Caught unhandled exception: {exc.Message}");
    }

    private static DiscordSocketConfig BuildDiscordSocketConfig() {
        return new DiscordSocketConfig {
            GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages,
            UseInteractionSnowflakeDate = false
        };
    }

    private static void ConfigureServices(IServiceCollection serviceCollection) {
        // Boilerplate for Discord.NET
        serviceCollection.AddSingleton<Ready>();
        serviceCollection.AddSingleton<InteractionCreated>();
        serviceCollection.AddSingleton<Bot>();
        serviceCollection.AddSingleton<InteractionService>();
        serviceCollection.AddSingleton<CommandService>();
        serviceCollection.AddSingleton<InteractionHandler>();
        serviceCollection.AddSingleton(new DiscordSocketClient(BuildDiscordSocketConfig()));

        // Custom Modules/database
        serviceCollection.AddDbContext<IApplicationDbContext, AzurLaneBBotDbContext>(options => {
            options.UseSqlite($"Data Source={AppDomain.CurrentDomain.BaseDirectory}{ConfigurationManager.AppSettings["dbv2RelativeLocation"]}");
        });
        serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();

        // Old stuff
        serviceCollection.AddSingleton<IDatabaseService, AzurDbContextDatabaseService>();
        serviceCollection.AddSingleton<IImageService, ImageService>();

        // TODO: New stuff, remove old stuff later
        // Application services
        serviceCollection.AddScoped<ISkinApplicationService, SkinApplicationService>();
        serviceCollection.AddScoped<IShipApplicationService, ShipApplicationService>();
        serviceCollection.AddScoped<IGameApplicationService, GameApplicationService>();

        // Aux service
        serviceCollection.AddScoped<IImageStorageService, ImageStorageService>();

        // Repositories
        serviceCollection.AddScoped<ISkinRepository, EfSkinRepository>();
        serviceCollection.AddScoped<IShipRepository, EfShipRepository>();
    }

    private static void ConfigureRequiredServices(IServiceProvider serviceProvider) {
        serviceProvider.GetRequiredService<DiscordSocketClient>();
        serviceProvider.GetRequiredService<InteractionService>();
        serviceProvider.GetRequiredService<IServiceProvider>();
    }
}
