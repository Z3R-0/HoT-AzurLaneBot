using Discord.Interactions;

namespace AzurLaneBBot.Modules.Commands.Modals {
    public class AddSkinModal : IModal {

        public string Title => "Add New skin";

        [InputLabel("skin_name")]
        [ModalTextInput("Name", placeholder: "skin name", maxLength: 255)]
        [RequiredInput(true)]
        public required string Name { get; set; }

        [InputLabel("skin_isSkinOf")]
        [ModalTextInput("Rarity", placeholder: "base ship this skin belongs to", maxLength: 255)]
        [RequiredInput(false)]
        public required string IsSkinOf { get; set; }

        [InputLabel("skin_cupsize")]
        [ModalTextInput("Cupsize", placeholder: "skin cupsize", maxLength: 3)]
        [RequiredInput(true)]
        public required string Cupsize { get; set; }

        [InputLabel("skin_coverage")]
        [ModalTextInput("Coverage Type", placeholder: "Sideboob, boob window, underboob, etc...", maxLength: 255)]
        [RequiredInput(true)]
        public required string CoverageType { get; set; }

        [InputLabel("skin_shape")]
        [ModalTextInput("Shape", placeholder: "Round, side set, tear drop, etc...", maxLength: 255)]
        [RequiredInput(true)]
        public required string Shape { get; set; }
    }
}
