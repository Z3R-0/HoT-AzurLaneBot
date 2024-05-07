using AzurLaneBBot.Database.DatabaseServices;
using AzurLaneBBot.Database.ImageServices;
using AzurLaneBBot.Database.Models;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game {
    public class GameCommands(IDatabaseService dbService, IImageService imageService) : BotInteraction<SocketSlashCommand> {
        private readonly IDatabaseService _dbService = dbService;
        private readonly IImageService _imageService = imageService;

        public const string GuessShipButtonId = "guess_ship_submit:";
        public const string GuessShipModalId = "guess_ship_modal:";

        [SlashCommand("guess-ship", "Guess a ship (or skin)")]
        public async Task HandleGuessShipSlash(
            [Choice("Yes", "Yes")]
            [Choice("No", "No")]
            string allowSkins) {
            await DeferAsync();

            var randShip = GetRandomShip(allowSkins == "Yes");

            if (randShip == null) {
                await FollowupAsync("No eligible ship was found...", ephemeral: true);
                return;
            }

            var conditionalText = allowSkins == "Yes" ? " or skin " : "";
            var embedBuilder = DiscordUtilityMethods.GetEmbedBuilder($"What is the name of this ship{conditionalText}?");

            var randShipImage = _imageService.GetImage(randShip.Name!);

            embedBuilder.WithImageUrl(randShipImage!.ImageUrl);
            embedBuilder.AddField("Caution!", "Only click the button once you are ready to answer", inline: true);

            var componentBuilder = new ComponentBuilder()
                .WithButton("Submit answer", $"{GuessShipButtonId + Context.User.Id}");

            await Context.Channel.SendFileAsync(randShipImage.FilePath, null, false, embed: embedBuilder.Build(), components: componentBuilder.Build());
        }

        private BoobaBotProject? GetRandomShip(bool allowSkins) {
            var possibleShips = _dbService.GetAllBBPShips().Where(
                            ship => !string.IsNullOrEmpty(ship!.ImageUrl)
                            && !allowSkins && string.IsNullOrEmpty(ship.IsSkinOf));

            var randomIndex = Random.Shared.Next(0, possibleShips.Count());

            if (randomIndex == 0)
                return null;

            var randShip = possibleShips.ElementAt(randomIndex);
            return randShip;
        }
    }
}
