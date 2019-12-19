using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BLL
{
    public class Engine : IEngine
    {
        private readonly IStateRepository _stateRepository;

        public Engine(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }


        public BLL.DTO.GameState GetSavedState(int id)
        {
            var gameState = _stateRepository.GetGameState(id);
            return MapFromDb(gameState);
        }


        public Task<int> SaveGameState(BLL.DTO.GameState gameState)
        {
            return _stateRepository.SaveGameState(MapFromPage(gameState));
        }

        public async Task<BLL.DTO.GameState> UpdateGameState(int gameId, int col)
        {
            var gameStateInner = _stateRepository.GetGameState(gameId);
            var gameStateOuter = MapFromDb(gameStateInner);
            if (CheckIfColumnNotFull(col, gameStateOuter))
            {
                SelectCell(col, gameStateOuter);
                TurnSwitch(gameStateOuter, gameStateInner);
                if (gameStateInner.GameMode != GameMode.HUMAN_VS_HUMAN)
                {
                    GenerateAIMove(gameStateOuter);
                    TurnSwitch(gameStateOuter, gameStateInner);
                }

                gameStateInner.BoardJson = SerializeBoard(gameStateOuter.Board);
                await _stateRepository.UpdateGameState(gameStateInner);
            }
            return gameStateOuter;
        }

        private DTO.GameState MapFromDb(GameState innerGameState)
        {
            var outerGameState = new BLL.DTO.GameState()
            {
                StateId = innerGameState.GameStateId,
                Height = innerGameState.BoardHeight,
                Width = innerGameState.BoardWidth,
                Board = DeserializeBoard(innerGameState.BoardJson),
                GameMode = innerGameState.GameMode,
                PlayerOneTurn = innerGameState.Turn
            };
            return outerGameState;
        }

        private GameState MapFromPage(DTO.GameState outerGameState)
        {
            var innerGameState = new GameState()
            {
                GameStateId = outerGameState.StateId,
                BoardHeight = outerGameState.Height,
                BoardWidth = outerGameState.Width,
                BoardJson = SerializeBoard(outerGameState.Board),
                GameMode = outerGameState.GameMode,
                Turn = outerGameState.PlayerOneTurn
            };
            return innerGameState;
        }

        public Task<List<GameState>> GetAllSavedGameStates()
        {
            return _stateRepository.GetAllSavedGameStates();
        }

        public async Task<BLL.DTO.GameState> CreateGameStateFromSettings()
        {
            var gameSettings = await GetGameSettings();
            var gameState = new BLL.DTO.GameState()
            {
                Height = gameSettings.BoardHeight,
                Width = gameSettings.BoardWidth,
                Board = new int[gameSettings.BoardHeight, gameSettings.BoardWidth],
                PlayerOneTurn = false,
                GameMode = gameSettings.GameMode
            };
            return await CreateGameState(gameState);
        }

        public async Task<BLL.DTO.GameState> CreateGameStateWithGameMode(GameMode gameMode)
        {
            var gameSettings = await GetGameSettings();
            var gameState = new BLL.DTO.GameState()
            {
                Height = gameSettings.BoardHeight,
                Width = gameSettings.BoardWidth,
                Board = new int[gameSettings.BoardHeight, gameSettings.BoardWidth],
                PlayerOneTurn = false,
                GameMode = gameMode
            };
            return await CreateGameState(gameState);
        }

        private async Task<DTO.GameState> CreateGameState(BLL.DTO.GameState gameState)
        {
            FillArray(gameState.Board);
            if (gameState.GameMode == GameMode.AI_FIRST)
            {
                GenerateAIMove(gameState);
                gameState.PlayerOneTurn = true;
            }
            gameState.StateId = await SaveGameState(gameState);
            return gameState;
        }


        public async Task<GameSettings> GetGameSettings()
        {
            var gameSettings = await _stateRepository.GetGameSettings();
            await _stateRepository.SaveChangesAsync();
            return gameSettings;
        }

        private static void FillArray(int[,] array)
        {
            var rnd = new Random();
            for (var i = 0; i < array.GetLength(0); i++)
            {
                for (var j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = 0;
                }
            }
        }

        private string SerializeBoard(int[,] board)
        {
            return JsonConvert.SerializeObject(board);
        }

        private int[,] DeserializeBoard(string board)
        {
            return JsonConvert.DeserializeObject<int[,]>(board);
        }

        private void SelectCell(int x, BLL.DTO.GameState gameState)
        {
            var y = (GetLowestFreeCell(x, gameState));
            gameState.Board[y, x] = gameState.PlayerOneTurn ? 1 : 2;
            CheckForWin();
        }

        private int GetLowestFreeCell(int x, BLL.DTO.GameState gameState)
        {
            if (gameState.Board[0,x] != 0);
            for (int y = gameState.Height - 1; y >= 0; y--)
            {
                if (gameState.Board[y, x] == 0)
                {
                    return y;
                }
            }

            return 0;
        }

        private bool CheckIfColumnNotFull(int x, DTO.GameState gameState)
        {
            return gameState.Board[0, x] == 0;
        }

        private void TurnSwitch(BLL.DTO.GameState gameStateOuter, GameState gameStateInner)
        {
            gameStateOuter.PlayerOneTurn = !gameStateOuter.PlayerOneTurn;
            gameStateInner.Turn = !gameStateInner.Turn;
        }

        private void GenerateAIMove(BLL.DTO.GameState gameState)
        {
            Random r = new Random();
            int rInt = r.Next(0, gameState.Width);
            while (true)
            {
                if (gameState.Board[0, rInt] != 0) continue;
                SelectCell(rInt, gameState);
                break;
            }
        }

        public Task<int> SaveGameStateWithName(DTO.GameState gameStateOuter)
        {
            var gameStateInner = _stateRepository.GetGameState(gameStateOuter.StateId);
            var namedSaveState = gameStateInner;
            namedSaveState.GameStateName = gameStateOuter.GameName;
            namedSaveState.GameStateId = 0;
            return _stateRepository.SaveGameState(namedSaveState);
        }

        private void CheckForWin()
        {
            
        }
    }
}