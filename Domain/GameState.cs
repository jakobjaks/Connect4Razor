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
        
        [Required]
        public string GameMode { get; set; }
        
        public ICollection<Move> Moves { get; set; }
        
    }
}