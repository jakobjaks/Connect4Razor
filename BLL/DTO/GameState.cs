using System.Collections;
using System.Collections.Generic;
using Domain;

namespace BLL.DTO
{
    public class GameState
    {
        public int StateId { get; set; }
        
        public string GameName { get; set; }
        
        public bool PlayerOneTurn { get; set; }

        public int[,] Board { get; set; }
        
        public int Height { get; set; }
        public int Width { get; set; }

        public GameMode GameMode { get; set; } = default!;
        public Win Winner { get; set; } = default!;

        public enum Win
        {
            NO_WINNER,
            PLAYER_ONE,
            PLAYER_TWO,
            DRAW
        }
    }
}