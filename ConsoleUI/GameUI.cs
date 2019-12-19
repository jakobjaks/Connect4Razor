﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
 using BLL.DTO;


 namespace ConsoleUI
{
    public static class GameUI
    {
        private static readonly string _verticalSeparator = "|";
        private static readonly string _horizontalSeparator = "-";
        private static readonly string _centerSeparator = "+";

        private static string _playerOneName = "X";
        private static string _playerTwoName = "O";

        public static void PrintBoard(GameState gameState)
        {
            MakeSpace(5);
            var board = gameState.Board;
            for (var yIndex = 0; yIndex < gameState.Height; yIndex++)
            {
                var line = "";
                for (var xIndex = 0; xIndex < gameState.Width; xIndex++)
                {
                    line = line + " " + GetSingleState(board[yIndex, xIndex]) + " ";
                    if (xIndex < gameState.Width - 1)
                    {
                        line = line + _verticalSeparator;
                    }
                }

                Console.WriteLine(line);

                if (yIndex >= gameState.Height - 1) continue;
                {
                    line = "";
                    for (var xIndex = 0; xIndex < gameState.Width; xIndex++)
                    {
                        line = line + _horizontalSeparator + _horizontalSeparator + _horizontalSeparator;
                        if (xIndex < gameState.Width - 1)
                        {
                            line = line + _centerSeparator;
                        }
                    }

                    Console.WriteLine(line);
                }
            }

            string bottomLine = "";
            for (int i = 1; i < gameState.Width + 1; i++)
            {
                bottomLine += " " + i + "  ";
            }

            Console.WriteLine(bottomLine);
        }

        private static void MakeSpace(int count)
        {
            for (var c = 0; c < count; c++)
            {
                Console.WriteLine();
            }
        }

        public static string GetSingleState(int state)
        {
            switch (state)
            {
                case 0:
                    return " ";
                case 1:
                    return "O";
                case 2:
                    return "X";
                default:
                    throw new InvalidEnumArgumentException("Unknown enum option!");
            }
        }

        public static void DisplayWin(string playerName)
        {
            Console.WriteLine($"{playerName} has won!" +
                              "Press O to return to menu :)");
        }


        public static void ChangeName(string newName, bool playerOne)
        {
            if (playerOne) _playerOneName = newName;
            else _playerTwoName = newName;
        }

        public static string PrintWin(bool playerOne)
        {
            return playerOne ? _playerOneName : _playerTwoName;
        }

        public static ICollection<Domain.GameState> DisplaySavedGames(ICollection<Domain.GameState> gameStates)
        {
            
            foreach (var name in gameStates)
            {
                Console.Write(name.GameStateName + " id: ");
                Console.Write(name.GameStateId);
                Console.WriteLine();

            }
            return gameStates;
        }
    }
}