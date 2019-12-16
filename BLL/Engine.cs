using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace BLL
{
    public class Engine
    {
        private readonly IStateRepository _stateRepository;

        public Engine(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }
        

        public BLL.DTO.GameState GetSavedState(int id)
        {
            var gameState = _stateRepository.GetGameStateWithMoves(id);
            var outerGameState = new BLL.DTO.GameState()
            {
                StateId = gameState.GameStateId,
                Height = gameState.BoardHeight,
                Width = gameState.BoardWidth,
                Moves = gameState.Moves,
                Board  = MakeBoard(gameState),
            };
            return outerGameState;
        }
        
        public int[,] MakeBoard(Domain.GameState gameState)
        {
            var board = new int[gameState.BoardHeight, gameState.BoardWidth];
            foreach (var move in gameState.Moves)
            {
                var turn = 1;
                if (move.PlayerOneTurn) turn = 0;
                board[move.YCoordinate, move.XCoordinate] = turn;
            }
            return board;
        }

        public Task<int> SaveGameState(BLL.DTO.GameState gameState)
        {
            var innerGameState = new GameState()
            {
                GameStateId = gameState.StateId,
                BoardHeight = gameState.Height,
                BoardWidth = gameState.Width,
                Moves = gameState.Moves,
                GameMode = "test"
            };
            return _stateRepository.SaveGameStateWithMoves(innerGameState);
        }

        public Task<List<GameState>> GetAllSavedGameStates()
        {
            return _stateRepository.GetAllSavedGameStates();
        }

        public async Task<BLL.DTO.GameState> CreateNewGameState()
        {
            var gameSettings = await GetGameSettings();
            var gameState = new BLL.DTO.GameState()
            {
                Height = gameSettings.BoardHeight,
                Width = gameSettings.BoardWidth,
                Board = new int[gameSettings.BoardHeight, gameSettings.BoardWidth],
                Moves = new List<Move>(),
            };
            FillArray(gameState.Board);
            return gameState;
        }

        public async Task<GameSettings> GetGameSettings()
        {
            return await _stateRepository.GetGameSettings();
        }
        
        public static void FillArray(int[,] array)
        {
            Random rnd = new Random();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = 0;
                }
            }
        }
    }
}