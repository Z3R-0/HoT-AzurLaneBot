using Discord.Interactions;

namespace AzurLaneBBot.Modules.Commands.Modals {
    public class GuessShipGameModal : IModal {
        public string Title => "Guess Ship/Skin";

        [InputLabel("guess")]
        [ModalTextInput("Guess", placeholder: "Enter your guess here (only letters, spaces and numbers)", maxLength: 255)]
        public required string Guess { get; set; }
    }
}
