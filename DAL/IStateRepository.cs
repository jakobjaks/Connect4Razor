using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace DAL
{
    public interface IStateRepository
    {
        GameState GetGameState(int id);
        Task<List<GameState>> GetAllSavedGameStates();
        Task<int> SaveGameState(GameState gameState);
        Task<GameSettings> GetGameSettings();
        Task<int> SaveGameSettings(GameSettings gameSettings);
        Task<int> SaveChangesAsync();
        Task<int> UpdateGameState(GameState gameState);

    }
}