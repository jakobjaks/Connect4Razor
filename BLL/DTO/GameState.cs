using System.Collections;
using System.Collections.Generic;
using Domain;

namespace BLL.DTO
{
    public class GameState
    {
        public int StateId { get; set; }
        
        public bool PlayerOneTurn { get; set; }

        public int[,] Board { get; set; }

        public ICollection<Move> Moves = new List<Move>(); 
        
        public int Height { get; set; }
        public int Width { get; set; }
    }
}