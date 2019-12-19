using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BLL;
using ConsoleUI;
using Domain;
using MenuSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;
using GameState = BLL.DTO.GameState;

namespace ConsoleApp
{
    public class ConsoleApplication
    {
        public ConsoleApplication(IEngine engine)
        {
            Engine = engine;
        }

        public IEngine Engine { get; set; }
        public BLL.DTO.GameState GameState { get; set; }


        public void Run()
        {
            Console.Clear();

            Console.WriteLine("Hello Game!");
            
            var settingsMenu = new Menu(2)
            {
                Title = "Settings",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Change player names",
                            CommandToExecute = ChangeName
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "Change map size",
                            CommandToExecute = SetBoardSize
                        }
                    }
                }
            };

            var gameMenu = new Menu(1)
            {
                Title = "Start a new game of Connect4",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "1", new MenuItem()
                        {
                            Title = "Computer starts",
                            CommandToExecute = AiFirstGame
                        }
                    },
                    {
                        "2", new MenuItem()
                        {
                            Title = "Human starts",
                            CommandToExecute = AiSecondGame
                        }
                    },
                    {
                        "3", new MenuItem()
                        {
                            Title = "Human against Human",
                            CommandToExecute = PVPGame
                        }
                    },
                    {
                        "4", new MenuItem()
                        {
                            Title = "Settings",
                            CommandToExecute = settingsMenu.Run
                        }
                    },
                    {
                        "5", new MenuItem()
                        {
                            Title = "Load game",
                            CommandToExecute = LoadGame
                        }
                    },
                }
            };


            var menu0 = new Menu(0)
            {
                Title = "Connect4 Main Menu",
                MenuItemsDictionary = new Dictionary<string, MenuItem>()
                {
                    {
                        "S", new MenuItem()
                        {
                            Title = "Start game",
                            CommandToExecute = gameMenu.Run
                        }
                    }
                }
            };


            menu0.Run();
        }



        private (int result, bool wasCanceled) GetUserIntInput(string prompt, int min, int max,
            int? cancelIntValue = null, string cancelStrValue = "")
        {
            do
            {
                Console.WriteLine(prompt);
                if (cancelIntValue.HasValue || !string.IsNullOrWhiteSpace(cancelStrValue))
                {
                    Console.WriteLine($"To cancel input enter: {cancelIntValue}" +
                                      $"{(cancelIntValue.HasValue && !string.IsNullOrWhiteSpace(cancelStrValue) ? " or " : "")}" +
                                      $"{cancelStrValue}");
                }

                Console.Write(">");
                var consoleLine = Console.ReadLine();

                if (consoleLine == cancelStrValue) return (0, true);

                if (consoleLine == "l")
                {
                    SaveGame();
                    GameUI.PrintBoard(GameState);
                    continue;
                }

                if (int.TryParse(consoleLine, out var userInt))
                {
                    if (userInt >= min && userInt <= max || userInt == 0)
                    {
                        return userInt == cancelIntValue ? (userInt, true) : (userInt, false);
                    }

                    Console.WriteLine($"'{consoleLine}' Wrong number!");
                    continue;
                }

                Console.WriteLine($"'{consoleLine}' cant be converted to int value!");
            } while (true);
        }
        
        
        
        private string ChangeName()
        {
            bool user;
            string consoleLine;
            Console.Clear();
            do
            {
                Console.WriteLine("Which player name do you want to edit (1,2) ?");
                Console.WriteLine("Press B to go back");
                consoleLine = Console.ReadLine();
                if (consoleLine != null && consoleLine.ToUpper() == "B")
                {
                    return "wrong place";
                }

                if (int.TryParse(consoleLine, out var userInt))
                {
                    if (userInt == 1) user = true;
                    else if (userInt == 2) user = false;
                    else
                    {
                        Console.WriteLine("Bad input");
                    }
                }
            } while (true);

            do
            {
                Console.WriteLine("Enter the new name:");
                consoleLine = Console.ReadLine();
            } while (true);
        }

        private string SetBoardSize()
        {
            var (item1, item2) = GetBoardSize();

            var gameSettings = new GameSettings()
            {
                BoardHeight = item1,
                BoardWidth = item2,
            };

            

            return "DONE";
        }
        
        
        private (int, int) GetBoardSize()
        {
            (int, int) tuple = (0, 0);
            Console.WriteLine("Enter heigth of the game board:");
            do
            {
                var consoleLine = Console.ReadLine();
                if (!int.TryParse(consoleLine, out var userInt)) continue;
                if (tuple.Item1 == 0)
                {
                    tuple.Item1 = userInt;
                    Console.WriteLine("Enter width of the game board:");
                }
                else
                {
                    tuple.Item2 = userInt;
                    break;
                }
            } while (true);

            return tuple;
        }
        
        private string AiFirstGame()
        {
            GameState = Engine.CreateGameStateWithGameMode(GameMode.AI_FIRST).Result;
            StartGame();
            return "Started";
        }

        private string AiSecondGame()
        {
            GameState = Engine.CreateGameStateWithGameMode(GameMode.AI_FIRST).Result;
            StartGame();
            return "Started";
        }

        private string PVPGame()
        {
            GameState = Engine.CreateGameStateWithGameMode(GameMode.AI_FIRST).Result;
            StartGame();
            return "Started";
        }

        private string LoadGame()
        {
            Console.Clear();
            
            var list = GameUI.DisplaySavedGames(Engine.GetAllSavedGameStates().Result);
            
            Console.WriteLine("Enter the id of the saved game or press M to go back");
            Console.Write(">");
            var consoleLine = Console.ReadLine();
            if (consoleLine == null)
            {
                return "M";
            }
            foreach (var save in list)
            {
                if (save.GameStateId.ToString().Equals(consoleLine))
                {
                    GameState = Engine.GetSavedState(save.GameStateId);
                    StartGame();
                } else if (consoleLine.Equals("M"))
                {
                    return "M";
                }
            }

            return "STARTED";
        }

        private void SaveGame()
        {
            Console.WriteLine("Enter a name for your new save or press M to return");
            var consoleLine = Console.ReadLine();
            if (consoleLine == "M")
            {
                return;
            }
            GameState.GameName = consoleLine;
            Engine.SaveGameStateWithName(GameState);
        }

        private string StartGame()
        {
            var firstMove = true;

            var done = false;
            do
            {
                GameUI.PrintBoard(GameState);

                int userXint;
                bool userCanceled;

                (userXint, userCanceled) = GetUserIntInput( "Enter X coordinate", 1, 7, 0);
                if (userCanceled)
                {
                    done = true;
                }
                else
                {
                    try
                    {
                        GameState = Engine.UpdateGameState(GameState.StateId, userXint - 1).Result;
                    } catch (GameException e) {
                        Console.WriteLine("This column is full!");
                    }
                }

                if (GameState.Winner != GameState.Win.NO_WINNER)
                {
                    done = true;
                }
            } while (!done);

            Console.Clear();

            var consoleLine = Console.ReadLine();
            if (consoleLine != "") ;
            Console.Clear();
            return "GAME OVER!!";
        }

        
    }
}