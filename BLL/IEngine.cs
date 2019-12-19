using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace BLL
{
    public interface IEngine
    {
        BLL.DTO.GameState GetSavedState(int id);
        Task<int> SaveGameState(BLL.DTO.GameState gameState);
        Task<List<GameState>> GetAllSavedGameStates();
        Task<BLL.DTO.GameState> CreateGameStateFromSettings();
        Task<BLL.DTO.GameState> CreateGameStateWithGameMode(GameMode gameMode);

        Task<GameSettings> GetGameSettings();
        Task<BLL.DTO.GameState> UpdateGameState(int gameId, int col);
        Task<int> SaveGameStateWithName(DTO.GameState gameStateOuter);
        Task UpdateSettingsBoardSize(GameSettings gameSettings);
        Task<int> DeleteGameState(int gameId);
        Task UpdatePlayerName(bool playerOne, string name);
        Task<string> GetWinnerName(DTO.GameState.Win Winner);
    }
}