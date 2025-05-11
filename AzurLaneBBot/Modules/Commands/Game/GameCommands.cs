using Application.Interfaces;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ReshDiscordNetLibrary;

namespace AzurLaneBBot.Modules.Commands.Game;
public class GameCommands(IGameApplicationService gameApplicationService) : BotInteraction<SocketSlashCommand> {
    private readonly IGameApplicationService _gameApplicationService = gameApplicationService;

    public const string GuessShipButtonId = "guess_ship_submit:";
    public const string GuessShipModalId = "guess_ship_modal:";

    [SlashCommand("guess-ship", "Guess a ship (or skin)")]
    public async Task HandleGuessShipSlash(
        [Choice("Yes", "Yes")]
        [Choice("No", "No")]
        string allowSkins) {
        await DeferAsync();

        var result = await _gameApplicationService.StartGuessShipGameAsync(allowSkins == "Yes");
        if (result == null) {
            await FollowupAsync("No eligible ship was found...", ephemeral: true);
            return;
        }

        var embed = DiscordUtilityMethods.GetEmbedBuilder(result.Prompt)
            .WithImageUrl(result.ImageUrl)
            .AddField("Caution!", "Only click the button once you are ready to answer", inline: true);

        var components = new ComponentBuilder()
            .WithButton("Submit answer", GuessShipModalId + result.Ship.Name);

        await FollowupWithFileAsync(result.ImageFilePath, embed: embed.Build(), components: components.Build());
    }
}
