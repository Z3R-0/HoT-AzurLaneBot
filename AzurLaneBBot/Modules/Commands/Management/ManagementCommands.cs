using Application.DTO;
using Application.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Domain.ShipAggregate.Enums;
using Domain.SkinAggregate.Enums;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Management;
public class ManagementCommands(
    ISkinApplicationService skinApplicationService,
    IShipApplicationService shipApplicationService) : BotInteraction<SocketSlashCommand> {
    private readonly ISkinApplicationService _skinApplicationService = skinApplicationService;
    private readonly IShipApplicationService _shipApplicationService = shipApplicationService;

    [SlashCommand("get-info", "Get information about a skin and its related ship")]
    [RequireRole("Booba Connoisseur")]
    public async Task GetInfoAsync(string skinName) {
        await DeferAsync();

        try {
            // Fetch the skin image details
            var skin = await _skinApplicationService.GetByNameAsync(skinName);
            if (skin == null) {
                await FollowupAsync($"[Input Error]: No skin found with the name '{skinName}'", ephemeral: true);
                return;
            }

            var image = await _skinApplicationService.GetImageAsync(skinName);

            // Fetch the related ship details
            var ship = await _shipApplicationService.GetByIdAsync(skin.ShipId);
            if (ship == null) {
                await FollowupAsync($"[Input Error]: No ship found related to the skin '{skinName}'", ephemeral: true);
                return;
            }

            // Format and display the information
            var embed = DiscordUtilityMethods.GetEmbedBuilder($"Skin Information: {skin.Name}")
                .AddField("Coverage type", skin.CoverageType, true)
                .AddField("Cup size", skin.CupSize, true)
                .AddField("Shape", skin.Shape, false)
                .AddField("Ship Name", ship.Name, true)
                .AddField("Ship Rarity", ship.Rarity.ToString(), true)
                .Build();

            await FollowupWithFileAsync(AppDomain.CurrentDomain.BaseDirectory + skin.ImageUrl, embed: embed);
        } catch (Exception ex) {
            await FollowupAsync("[System Error] An unexpected error occurred while processing your request.\n" +
                                $"```{ex.InnerException?.Message ?? ex.Message}```");
        }
    }

    [SlashCommand("delete-ship", "Remove a ship from the database")]
    [RequireRole("Booba Connoisseur")]
    public async Task HandleDeleteShipSlash(string shipName) {
        await DeferAsync();

        try {
            var (success, message) = await _shipApplicationService.DeleteShipAsync(shipName);

            if (success)
                await FollowupAsync($"[Success]: Successfully deleted ship '{shipName}'");
            else
                await FollowupAsync($"{message}", ephemeral: true);
        } catch (Exception ex) {
            await FollowupAsync("[System Error] An unexpected error occurred while processing your request.\n" +
                                $"```{ex.InnerException?.Message ?? ex.Message}```");
        }
    }

    [SlashCommand("delete-skin", "Remove a skin from the database")]
    [RequireRole("Booba Connoisseur")]
    public async Task HandleDeleteSkinSlash(string skinName) {
        await DeferAsync();

        try {
            var (success, message) = await _skinApplicationService.DeleteSkinAsync(skinName);

            if (success)
                await FollowupAsync($"[Success]: Successfully deleted skin '{skinName}'");
            else
                await FollowupAsync($"{message}", ephemeral: true);
        } catch (Exception ex) {
            await FollowupAsync("[System Error] An unexpected error occurred while processing your request.\n" +
                                $"```{ex.InnerException?.Message ?? ex.Message}```");
        }
    }

    [SlashCommand("register-ship", "Register a new ship in the database")]
    [RequireRole("Booba Connoisseur")]
    public async Task RegisterShipAsync(
    string shipName,
    Rarity rarity) {
        await DeferAsync();

        try {
            var dto = new RegisterShip {
                ShipName = shipName,
                Rarity = rarity
            };

            var (success, message) = await _shipApplicationService.RegisterShipAsync(dto);

            if (success)
                await FollowupAsync($"[Success]: Successfully registered ship '{shipName}'");
            else
                await FollowupAsync($"{message}", ephemeral: true);
        } catch (Exception ex) {
            await FollowupAsync("[System Error] An unexpected error occurred while processing your request.\n" +
                                $"```{ex.InnerException?.Message ?? ex.Message}```");
        }
    }

    [SlashCommand("register-skin", "Register a new skin for a ship")]
    [RequireRole("Booba Connoisseur")]
    public async Task RegisterSkinAsync(
    string shipName,
    CoverageType coverageType,
    CupSize cupSize,
    Shape shape,
    IAttachment image,
    string? skinName = null) {
        if (!Path.GetExtension(image.Url).Contains("png")) {
            await RespondAsync("[Input Error]: Only PNG format images are allowed", ephemeral: true);
            return;
        }

        await DeferAsync();

        try {
            using var client = new HttpClient();
            var imageData = await client.GetByteArrayAsync(image.Url);

            var dto = new RegisterSkin {
                ShipName = shipName,
                SkinName = skinName,
                CoverageType = coverageType,
                CupSize = cupSize,
                Shape = shape,
                ImageData = imageData
            };

            var (success, message) = await _skinApplicationService.RegisterSkinAsync(dto);

            if (success)
                await FollowupAsync($"[Success]: Successfully uploaded and registered skin");
            else
                await FollowupAsync($"{message}", ephemeral: true);
        } catch (Exception ex) {
            await FollowupAsync("[System Error] An unexpected error occurred while processing your request.\n" +
                                $"```{ex.InnerException?.Message ?? ex.Message}```");
        }
    }
}
