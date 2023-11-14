using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.EntityFrameworkCore;
using AzurLaneBBot.Database.Models;
using ReshDiscordNetLibrary;
using Jan0660.AzurAPINet;
using Jan0660.AzurAPINet.Ships;
using AzurApiLibrary;
using System.Diagnostics;

namespace AzurLaneBBot.Modules.Commands {
    public class TestCommands : ReshDiscordNetLibrary.BotInteraction<SocketSlashCommand> {

        private AzurlanedbContext _dbContext = new AzurlanedbContext();
        private AzurClient _azurClient;

        public TestCommands(AzurClient azurClient) {
            _azurClient = azurClient;
        }

        [SlashCommand("test", "Test if the database can be accessed")]
        public async Task HandleTestSlash(string shipName) {
            try {
                await DeferAsync();
                var testEntry = _dbContext.BoobaBotProjects.Where(b => b.Name.ToLower() == shipName.ToLower().Trim()).FirstOrDefault();

                if(testEntry == null) {
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

        [SlashCommand("api-test","Test if the 3rd party API can be accessed")]
        public async Task HandleApiTestSlash(string ShipName) {
            await DeferAsync();
            Ship testShip = await _azurClient.GetShipAsync(ShipName);
            var embed = DiscordUtilityMethods.GetEmbedBuilder("API test result:");

            embed.AddField("Data", $"Retrieved stats from: {testShip.Names.en}\n\nRarity: {testShip.Rarity}\n" +
                               $"Nationality: {testShip.Nationality}\nClass: {testShip.Class}");
            embed.WithImageUrl(testShip.Thumbnail);

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

        [SlashCommand("menu", "menu command")]
        public async Task HandleMenu() {
            var selectMenu = new SelectMenuBuilder {
                CustomId = "menu_selection",
                Placeholder = "Select an option",
                MinValues = 1,
                MaxValues = 3,
            };

            selectMenu.AddOption("Hey", "hey", "This is an cool description");
            selectMenu.AddOption("Hoi", "hoi", "This is an cool description");
            selectMenu.AddOption("Hallo", "hallo", "This is an cool description");
            selectMenu.AddOption("Haiii", "haiii", "This is an cool description");

            await RespondAsync("Select an option", components: new ComponentBuilder().WithSelectMenu(selectMenu).Build());
        }
    }
}
