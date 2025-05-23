﻿using Application;
using Application.Interfaces;
using AzurLaneBBot.Core.InteractionHandling;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot;
public static class Program {
    private static IConfiguration Configuration;

    private static async Task Main() {
        // Build configuration
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddUserSecrets(typeof(Program).Assembly)
            .Build();

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

        // Configuration
        serviceCollection.AddSingleton(Configuration);

        // Database
        serviceCollection.AddDbContext<IApplicationDbContext, AzurLaneBBotDbContext>(options => {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString)) {
                throw new InvalidOperationException("DefaultConnection string is not configured.");
            }
            options.UseSqlServer(connectionString);
        });
        serviceCollection.AddScoped<IUnitOfWork, EfUnitOfWork>();

        // Application services
        serviceCollection.AddScoped<ISkinApplicationService, SkinApplicationService>();
        serviceCollection.AddScoped<IShipApplicationService, ShipApplicationService>();
        serviceCollection.AddScoped<IGameApplicationService, GameApplicationService>();

        // Aux services
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
