

using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameSettings
    {
        public int GameSettingsId { get; set; }

        [Display(Name="Board Height")]
        public int BoardHeight { get; set; }
        
        [Display(Name="Board Width")]
        public int BoardWidth { get; set; }

        [Display(Name="Player one name")]
        public string PlayerOneName { get; set; }
        
        [Display(Name="Player two name")]
        public string PlayerTwoName { get; set; }

        public GameMode GameMode { get; set; } = default!;

    }
}