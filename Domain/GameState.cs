using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameState
    {
        public int GameStateId { get; set; }
        
        public string GameStateName { get; set; }
        
        [Required]
        public bool Turn { get; set; }

        public int BoardHeight { get; set; }
        public int BoardWidth { get; set; }

        public string BoardJson { get; set; } = default!;

        public GameMode GameMode { get; set; } = default!;


    }
}