using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text;
using System.Text.RegularExpressions;

namespace ReshDiscordNetLibrary;
public class DiscordUtilityMethods : BotInteraction<SocketSlashCommand> {
    public async Task CountdownAsync(int counts) {
        for (var i = counts; i > 0; i--) {
            await Task.Delay(TimeSpan.FromSeconds(1));
            await RespondAsync($"{i}");
        }
    }

    /// <summary>
    /// Returns a SocketGuildUser object formatted to allow mentioning that user
    /// </summary>
    /// <param name="message">Message to parse</param>
    public static async Task<SocketGuildUser?> GetUserFromMessage(string message, InteractionContext context) {
        try {
            SocketGuildUser user;

            user = (SocketGuildUser)await context.Guild.GetUserAsync(ulong.Parse(Regex.Match(message, "([0-9])\\w+").Value));

            return user;
        } catch (Exception e) {
            Logger.Log(e.ToString());
            return null;
        }
    }

    /// <summary>
    /// Returns ID of the user mentioned in this message as a string
    /// </summary>
    /// <param name="message">Message to parse</param>
    public static string GetUserIdFromMessage(string message) {
        return Regex.Match(message, "([0-9])\\w+").Value;
    }

    /// <summary>
    /// Returns a pre-formatted embed with a custom title, the color set to Teal and the Footer to include the creator's name (Spade) and the link to the guild's server
    /// </summary>
    /// <returns>A pre-formatted embed with the color set to Teal and the Footer to include the creator's name (Spade) and the link to the guild's server</returns>
    public static EmbedBuilder GetEmbedBuilder(string title) {
        var embedBuilder = new EmbedBuilder();

        embedBuilder.WithTitle(title);
        embedBuilder.WithColor(Color.Teal);
        embedBuilder.Footer = new EmbedFooterBuilder().WithText("AzurLaneBoobaBot - Created by Home Of Thighs Guild\nhttps://discord.gg/tnFAXshaha");

        return embedBuilder;
    }

    /// <summary>
    /// Normalizes a name by replacing special characters with their ASCII equivalents and removing extra spaces.
    /// </summary>
    /// <param name="name">Name to normalize</param>
    /// <returns>Normalized name</returns>
    public static string NormalizeName(string name) {
        // Replace special characters (e.g., umlauts) with their ASCII equivalents
        name = name.Normalize(NormalizationForm.FormD);
        var filteredName = new string(name
            .Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
            .ToArray());

        // Remove extra spaces and return the normalized name
        return filteredName.Trim();
    }
}
