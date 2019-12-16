using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface IStateRepository
    {
        GameState GetGameStateWithMoves(int id);
        Task<List<GameState>> GetAllSavedGameStates();
        Task<int> SaveGameStateWithMoves(GameState gameState);
        Task<int> SaveGameState(GameState gameState);
        Task<int> SaveMoves(ICollection<Move> moves);
        Task<GameSettings> GetGameSettings();
        Task<int> SaveGameSettings(GameSettings gameSettings);

    }
}