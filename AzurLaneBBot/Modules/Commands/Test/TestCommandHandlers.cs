using Discord.Interactions;
using Discord.WebSocket;

namespace AzurLaneBBot.Modules.Commands.Test {
    public class TestCommandHandlers : ReshDiscordNetLibrary.BotInteraction<SocketMessageComponent> {
        [ComponentInteraction("click_button")]
        public async Task HandleButton() {
            await Context.Interaction.UpdateAsync(m => {
                m.Content = "I have been clicked!";
                m.Components = null;
            });
            await FollowupAsync($"{Context.User.Username} has clicked the button");
        }
    }
}
