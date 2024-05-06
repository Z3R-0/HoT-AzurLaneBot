using Discord.Interactions;

namespace AzurLaneBBot.Modules.Commands.Modals {
    public class UpdateShipModal : IModal {

        public string Title => "Update existing ship";

        [InputLabel("ship_name")]
        [ModalTextInput("Name", placeholder: "Name of the ship to update", maxLength: 255)]
        [RequiredInput(true)]
        public required string Name { get; set; }

        [InputLabel("ship_rarity")]
        [ModalTextInput("Rarity", placeholder: "Ship rarity (leave empty for skins)", maxLength: 2)]
        [RequiredInput(false)]
        public required string Rarity { get; set; }

        [InputLabel("ship_cupsize")]
        [ModalTextInput("Cupsize", placeholder: "Ship cupsize", maxLength: 3)]
        [RequiredInput(true)]
        public required string Cupsize { get; set; }

        [InputLabel("ship_coverage")]
        [ModalTextInput("Coverage Type", placeholder: "Sideboob, boob window, underboob, etc...", maxLength: 255)]
        [RequiredInput(true)]
        public required string CoverageType { get; set; }

        [InputLabel("ship_shape")]
        [ModalTextInput("Shape", placeholder: "Round, side set, tear drop, etc...", maxLength: 255)]
        [RequiredInput(true)]
        public required string Shape { get; set; }
    }
}
