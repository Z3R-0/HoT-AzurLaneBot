using AzurApiLibrary;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Test {
    public class TestCommands(IDatabaseService dbService, IAzurClient azurClient, IImageService imageService) : BotInteraction<SocketSlashCommand> {
        private readonly IDatabaseService _dbService = dbService;
        private readonly IAzurClient _azurClient = azurClient;
        private readonly IImageService _imageService = imageService;

        [SlashCommand("get-info", "get info on a ship/skin")]
        public async Task HandleTestSlash(string shipName) {
            try {
                await DeferAsync();

                var infoEntry = _dbService.GetBBPShip(shipName) ?? throw new ArgumentException($"Couldn't find an entry named: {shipName}, make sure it is present in the Name column of the database");

                var embed = DiscordUtilityMethods.GetEmbedBuilder("Database test result:");

                var isSkinOf = "";
                if (infoEntry.IsSkinOf != null) {
                    isSkinOf = $"\nIsSkinOf: {infoEntry.IsSkinOf}";
                }

                embed.AddField("Data", $"Retrieved stats from: {infoEntry.Name}\n\nRarity: {infoEntry.Rarity}" + isSkinOf + $"\nCup size: {infoEntry.CupSize}\n" +
                                   $"Coverage type: {infoEntry.CoverageType}\nShape: {infoEntry.Shape}");

                if (string.IsNullOrEmpty(infoEntry.IsSkinOf)) {
                    var image = await _azurClient.GetShipAsync(shipName);

                    if (image != null)
                        embed.WithImageUrl(image.Thumbnail);
                } else {
                    var shipSkin = (await _azurClient.GetShipAsync(infoEntry.IsSkinOf)).Skins.Where(skin => skin.Name == shipName).FirstOrDefault();
                    if (shipSkin != null)
                        embed.WithImageUrl(shipSkin.Image);
                }

                await FollowupAsync(embed: embed.Build());
            } catch (Exception e) {
                Logger.Log(e);
                await FollowupAsync($"Something went wrong while retrieving data from database: {e.Message}", ephemeral: true);
            }
        }

        [SlashCommand("api-test", "Test if the 3rd party API can be accessed")]
        public async Task HandleApiTestSlash(string shipName) {
            await DeferAsync();
            var testShip = await _azurClient.GetShipAsync(shipName);
            var embed = DiscordUtilityMethods.GetEmbedBuilder("API test result:");

            embed.AddField("Data", $"Retrieved stats from: {testShip.Names.en}\n\nRarity: {testShip.Rarity}\n" +
                               $"Nationality: {testShip.Nationality}\nClass: {testShip.Class}");
            embed.WithImageUrl(testShip.Thumbnail);

            await FollowupAsync(embed: embed.Build());
        }

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
}
