using AzurLaneBBot.Database.ImageServices;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Test;
public class TestCommands(IImageService imageService) : BotInteraction<SocketSlashCommand> {
    private readonly IImageService _imageService = imageService;

    //[SlashCommand("get-info", "get info on a ship/skin")]
    //public async Task HandleTestSlash(string shipName) {
    //    try {
    //        await DeferAsync();

    //        shipName = Regex.Replace(shipName, @"[^a-zA-Z0-9äöüÄÖÜẞ ]", string.Empty);

    //        var infoEntry = _dbService.GetBBPShip(shipName) ?? throw new ArgumentException($"Couldn't find an entry named: {shipName}, make sure it is present in the Name column of the database");

    //        var embed = DiscordUtilityMethods.GetEmbedBuilder("Database test result:");

    //        var isSkinOf = "";
    //        if (!string.IsNullOrEmpty(infoEntry.IsSkinOf)) {
    //            isSkinOf = $"**Is skin of**: {infoEntry.IsSkinOf}\n";
    //        }

    //        embed.Title = $"{infoEntry.Name}'s stats";
    //        embed.Description = $"{(isSkinOf == null ? "**Rarity**:" + infoEntry.Rarity + "\n" : "")}"
    //                            + isSkinOf +
    //                            $"**Cup size**: {infoEntry.CupSize}\n" +
    //                            $"**Coverage type**: {infoEntry.CoverageType}\n" +
    //                            $"**Shape**: {infoEntry.Shape}";


    //        if (string.IsNullOrEmpty(infoEntry.ImageUrl)) {
    //            await GetShipImageFromApi(shipName, infoEntry, embed);

    //            await FollowupAsync(embed: embed.Build());
    //        } else {
    //            var shipImage = _imageService.GetImage(infoEntry.Name!);
    //            if (shipImage == null)
    //                await FollowupAsync(embed: embed.Build());
    //            else
    //                await FollowupWithFileAsync(shipImage.FilePath, embed: embed.Build());
    //        }
    //    } catch (Exception e) {
    //        Logger.Log(e);
    //        await FollowupAsync($"Something went wrong while retrieving data from database: {e.Message}", ephemeral: true);
    //    }

    //    async Task GetShipImageFromApi(string shipName, Database.Models.BoobaBotProject infoEntry, Discord.EmbedBuilder embed) {
    //        if (string.IsNullOrEmpty(infoEntry.IsSkinOf)) {
    //            var image = await _azurClient.GetShipAsync(shipName);

    //            if (image != null)
    //                embed.WithImageUrl(image.Thumbnail);
    //        } else {
    //            var shipSkin = (await _azurClient.GetShipAsync(infoEntry.IsSkinOf)).Skins.Where(skin => Regex.Replace(skin.Name, @"[^a-zA-Z0-9äöüÄÖÜẞ ]", string.Empty) == shipName)?.FirstOrDefault();
    //            if (shipSkin != null)
    //                embed.WithImageUrl(shipSkin.Image);
    //        }
    //    }
    //}

    [SlashCommand("custom-image-test", "Test if the custom images are viewable")]
    public async Task HandleCustomImageTest(string shipName) {
        await DeferAsync();

        var embed = DiscordUtilityMethods.GetEmbedBuilder("Custom image test result:");

        var shipImage = _imageService.GetImage(shipName);

        if (shipImage != null) {
            embed.WithImageUrl(shipImage.ImageUrl);

            await FollowupWithFileAsync(shipImage.FilePath, embed: embed.Build());
        } else {
            embed.AddField("Failed to retrieve image", $"No image was found in relation to ship: {shipName}");

            await FollowupAsync(embed: embed.Build());
        }
    }

    [SlashCommand("ping", "Use to check if the bot is online without sending messages into a channel")]
    public async Task HandlePingAsync() {
        await RespondAsync("Pong!", ephemeral: true);
    }
}
