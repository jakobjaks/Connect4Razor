using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Move
    {

        public int MoveId { get; set; }

        public bool PlayerOneTurn { get; set; }
        
        public int GameStateId { get; set; }
        public GameState GameState { get; set; }

        public int YCoordinate { get; set; }
        public int XCoordinate { get; set; }

        [NotMapped]
        public (int, int) Position => (YCoordinate, XCoordinate);
    }
}