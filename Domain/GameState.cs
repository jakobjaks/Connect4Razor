using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameState
    {
        [Display(Name="Save ID")]
        public int GameStateId { get; set; }
        
        [Display(Name="Save Name")]
        public string GameStateName { get; set; }
        
        [Required]
        public bool Turn { get; set; }

        [Display(Name="Board Height")]
        public int BoardHeight { get; set; }
        
        [Display(Name="Board Width")]
        public int BoardWidth { get; set; }

        public string BoardJson { get; set; } = default!;

        [Display(Name="Game Mode")]
        public GameMode GameMode { get; set; } = default!;


    }
}