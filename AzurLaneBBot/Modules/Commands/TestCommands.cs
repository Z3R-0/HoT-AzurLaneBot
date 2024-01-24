using AzurApiLibrary;
using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Jan0660.AzurAPINet.Ships;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands {
    public class TestCommands : BotInteraction<SocketSlashCommand> {
        protected IDatabaseService _dbService;
        protected IAzurClient _azurClient;
        protected IImageService _imageService;

        public TestCommands(AzurClient azurClient, AzurlanedbContext azurlanedbContext, ImageService imageService) {
            _azurClient = azurClient;
            _dbService = new AzurDbContextDatabaseService(azurlanedbContext);
            _imageService = imageService;
        }

        [SlashCommand("test", "Test if the database can be accessed")]
        public async Task HandleTestSlash(string shipName) {
            try {
                await DeferAsync();
                var testEntry = _dbService.GetBBPShip(shipName);

                if (testEntry == null) {
                    throw new ArgumentException($"Couldn't find an entry named: {shipName}, make sure it is present in the Name column of the database");
                }

                var embed = DiscordUtilityMethods.GetEmbedBuilder("Database test result:");

                embed.AddField("Data", $"Retrieved stats from: {testEntry.Name}\n\nRarity: {testEntry.Rarity}\nIsSkinOf: {testEntry.IsSkinOf ?? "false"}\nCup size: {testEntry.CupSize}\n" +
                                   $"Coverage type: {testEntry.CoverageType}\nShape: {testEntry.Shape}");

                if (string.IsNullOrEmpty(testEntry.IsSkinOf)) {
                    embed.WithImageUrl((await _azurClient.GetShipAsync(shipName)).Thumbnail);
                } else {
                    var shipSkin = (await _azurClient.GetShipAsync(testEntry.IsSkinOf)).Skins.Where(skin => skin.Name == shipName).FirstOrDefault();
                    embed.WithImageUrl(shipSkin.Image);
                }

                await FollowupAsync(embed: embed.Build());
            } catch (Exception e) {
                Logger.Log(e);
                await FollowupAsync($"Something went wrong while retrieving data from database: {e.Message}");
            }
        }

        [SlashCommand("api-test", "Test if the 3rd party API can be accessed")]
        public async Task HandleApiTestSlash(string shipName) {
            await DeferAsync();
            Ship testShip = await _azurClient.GetShipAsync(shipName);
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

            var imageResult = _imageService.GetImage(shipName)?.ImageURL;

            if (imageResult != null) {
                embed.WithImageUrl(imageResult);
            } else {
                embed.AddField("Failed to retrieve image", $"No image was found in relation to ship: {shipName}");
            }

            await FollowupAsync(embed: embed.Build());
        }

        [SlashCommand("button", "button command")]
        public async Task HandleButton() {
            var btn = new ButtonBuilder {
                Label = "Click me",
                CustomId = "click_button",
                Style = ButtonStyle.Primary,
            };

            await RespondAsync("Cool button", components: new ComponentBuilder().WithButton(btn).Build());
        }
    }
}
