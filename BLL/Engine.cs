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
                if (gameStateInner.GameMode != GameMode.HUMAN_VS_HUMAN && gameStateOuter.Winner != DTO.GameState.Win.DRAW)
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
            CheckForWin(gameState);
        }

        private int GetLowestFreeCell(int x, BLL.DTO.GameState gameState)
        {
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
            if (gameState.Width - 1 < x || x < 0) return false;
            return gameState.Board[0, x] == 0;
        }

        private void TurnSwitch(BLL.DTO.GameState gameStateOuter, GameState gameStateInner)
        {
            gameStateOuter.PlayerOneTurn = !gameStateOuter.PlayerOneTurn;
            gameStateInner.Turn = !gameStateInner.Turn;
            if (CheckIfBoardIsFull(gameStateOuter)) gameStateOuter.Winner = DTO.GameState.Win.DRAW;

        }

        private bool CheckIfBoardIsFull(DTO.GameState gameState)
        {
            for (var x = 0; x < gameState.Width; x++) {
                if (gameState.Board[0, x] == 0) return false; 
            }
            return true;
        }

        private void GenerateAIMove(BLL.DTO.GameState gameState)
        {
            Random r = new Random();
            while (true)
            {
                var rInt = r.Next(0, gameState.Width);
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

        //https://stackoverflow.com/questions/39062111/java-how-to-check-diagonal-connect-four-win-in-2d-array
        private void CheckForWin(DTO.GameState gameState)
        {
            int[,] directions = new int[,] {{1,0}, {1,-1}, {1,1}, {0,1}};
            for (var d = 0; d < 4; d++) {
                int dx = directions[d, 0];
                int dy = directions[d, 1];
                for (int x = 0; x < gameState.Width; x++) {
                    for (int y = 0; y < gameState.Height; y++) {
                        int lastx = x + 3*dx;
                        int lasty = y + 3*dy;
                        if (0 <= lastx && lastx < gameState.Width && 0 <= lasty && lasty < gameState.Height) {
                            int w = gameState.Board[x,y];
                            if (w != 0 && w == gameState.Board[x+dx, y+dy] 
                                         && w == gameState.Board[x+2*dx, y+2*dy] 
                                         && w == gameState.Board[lastx, lasty]) {
                                if (w == 1)
                                {
                                    gameState.Winner = DTO.GameState.Win.PLAYER_ONE;
                                }
                                else
                                {
                                    gameState.Winner = DTO.GameState.Win.PLAYER_TWO;
                                }
                            }
                        }
                    }
                }
            }
        }

        public Task<int> DeleteGameState(int gameId)
        {
            return _stateRepository.DeleteGameState(gameId);
        }

        public async Task<string> GetWinnerName(DTO.GameState.Win Winner)
        {
            var settings = await _stateRepository.GetGameSettings();
            return Winner == DTO.GameState.Win.PLAYER_ONE ? settings.PlayerOneName : settings.PlayerTwoName;
        }

        public async Task UpdatePlayerName(bool playerOne, string name)
        {
            var settings = await _stateRepository.GetGameSettings();
            if (playerOne)
            {
                settings.PlayerOneName = name;
            }
            else
            {
                settings.PlayerTwoName = name;
            }
            await _stateRepository.SaveChangesAsync();
        }

        public async Task UpdateSettingsBoardSize(GameSettings gameSettings)
        {
            var settings = await _stateRepository.GetGameSettings();
            settings.BoardHeight = gameSettings.BoardHeight;
            settings.BoardWidth = gameSettings.BoardWidth;
            await _stateRepository.SaveChangesAsync();
        }
    }
}