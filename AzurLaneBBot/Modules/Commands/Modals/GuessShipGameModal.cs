using Discord.Interactions;

namespace AzurLaneBBot.Modules.Commands.Modals {
    public class GuessShipGameModal : IModal {
        public string Title => "Guess Ship/Skin";

        [InputLabel("guess")]
        [ModalTextInput("Guess", placeholder: "Enter your guess here", maxLength: 255)]
        public required string Guess { get; set; }
    }
}
