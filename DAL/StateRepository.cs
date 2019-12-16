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
        private readonly DAL.AppDbContext _context;

        public StateRepository(AppDbContext context)
        {
            _context = context;
        }


        public GameState GetGameStateWithMoves(int id)
        {
            var gameState = _context.GameStates.Include(item => item.Moves)
                .SingleOrDefaultAsync(item => item.GameStateId == id).Result;

            return gameState;
        }

        public async Task<List<GameState>> GetAllSavedGameStates()
        {
            var gameStates = await _context.GameStates.ToListAsync();

            return gameStates;
        }

        public async Task<int> SaveGameStateWithMoves(GameState gameState)
        {
            await SaveGameState(gameState);
            return await SaveMoves(gameState.Moves);
        }

        public async Task<int> SaveGameState(GameState gameState)
        {
            await _context.GameStates.AddAsync(gameState);
            await _context.SaveChangesAsync();
            return gameState.GameStateId;
        }

        public async Task<int> SaveMoves(ICollection<Move> moves)
        {
            await _context.Move.AddRangeAsync(moves);
            return await _context.SaveChangesAsync();
        }

        public async Task<GameSettings> GetGameSettings()
        {
            GameSettings settings;
            if (_context.GameSettings.Any())
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
                settings = await _context.GameSettings.FirstOrDefaultAsync();
            }
            return settings;
        }

        public async Task<int> SaveGameSettings(GameSettings gameSettings)
        {
            await _context.GameSettings.AddAsync(gameSettings);
            await _context.SaveChangesAsync();
            return gameSettings.GameSettingsId;
        }
    }
}