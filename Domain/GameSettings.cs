

namespace Domain
{
    public class GameSettings
    {
        public int GameSettingsId { get; set; }

        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }

        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }

    }
}