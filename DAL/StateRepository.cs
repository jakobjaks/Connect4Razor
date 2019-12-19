using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class StateRepository : IStateRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<GameState> _gameStates;
        private readonly DbSet<GameSettings> _gameSettings;

        public StateRepository(AppDbContext context)
        {
            _context = context;
            _gameStates = _context.Set<GameState>();
            _gameSettings = _context.Set<GameSettings>();
        }


        public GameState GetGameState(int id)
        {
            var gameState = _context.GameStates
                .SingleOrDefaultAsync(item => item.GameStateId == id).Result;
            return gameState;
        }

        public async Task<List<GameState>> GetAllSavedGameStates()
        {
            var gameStates = await _context.GameStates.Where(item => item.GameStateName != null).ToListAsync();
            return gameStates;
        }
        
        public async Task<int> SaveGameState(GameState gameState)
        {
            await _context.GameStates.AddAsync(gameState);
            await SaveChangesAsync();
            return gameState.GameStateId;
        }
        

        public async Task<int> UpdateGameState(GameState gameState)
        {
            _gameStates.Update(gameState);
            return await SaveChangesAsync();
        }

        public async Task<GameSettings> GetGameSettings()
        {
            GameSettings settings;
            if ((await _gameSettings.AnyAsync()) == false)
            {
                settings = new GameSettings()
                {
                    BoardHeight = 8,
                    BoardWidth = 8,
                    PlayerOneName = "Player One",
                    PlayerTwoName = "Player Two"
                };
                settings.GameSettingsId = await SaveGameSettings(settings);
            }
            else
            {
                settings = await _gameSettings.FirstOrDefaultAsync();
            }
            return settings;
        }

        public async Task<int> SaveGameSettings(GameSettings gameSettings)
        {
            await _gameSettings.AddAsync(gameSettings);
            return gameSettings.GameSettingsId;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}