using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4Group1FinalProject
{
    // Enum to represent the different players
    enum PlayerType
    {
        Human,
        AI
    }
    // Enum to represent the different cell states on the board
    enum CellState
    {
        Empty,
        Xeno,
        Oni
    }

    // Interface for the players
    interface IPlayer
    {
        CellState playerType { get; }
        string playerName { get; }
        Task<int> GetMove(Board board);
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool exitGame = false;

            while (!exitGame)
            {


                try
                {

                    Console.WriteLine("Connect Four Game");
                    Console.WriteLine("=================");
                    Console.WriteLine("Game Modes:");
                    Console.WriteLine("1. Human vs. Human");
                    Console.WriteLine("2. Human vs. AI");
                    Console.WriteLine("3. AI vs. AI");
                    Console.WriteLine("4. Exit Game");
                    Console.WriteLine();



                    Console.Write("Enter the game mode (1-4): ");
                    int mode;
                    while (true)
                    {
                        if (int.TryParse(Console.ReadLine(), out mode) && mode >= 1 && mode <= 4)
                        {
                            break;
                        }



                        Console.WriteLine("Invalid game mode. Try again.");
                    }

                    if (mode == 4)
                    {
                        Console.WriteLine("Exiting the game...");
                        exitGame = true;
                        break;
                    }

                    IPlayer player1, player2;
                    switch (mode)
                    {
                        case 1:
                            player1 = new Player(CellState.Xeno, GetPlayerName("Xeno", 'X'));
                            player2 = new Player(CellState.Oni, GetPlayerName("Oni", 'O'));
                            break;
                        case 2:
                            player1 = new Player(CellState.Xeno, GetPlayerName("Xeno", 'X'));
                            player2 = new AIPlayer(CellState.Oni, GetAIDifficulty("Oni"), "Oni(O)");
                            break;
                        case 3:
                            player1 = new AIPlayer(CellState.Xeno, GetAIDifficulty("Xeno"), "Xeno(X)");
                            player2 = new AIPlayer(CellState.Oni, GetAIDifficulty("Oni"), "Oni(O)");
                            break;
                        default:
                            throw new InvalidOperationException("Invalid game mode.");
                    }

                    ConnectFourGame game = new ConnectFourGame(player1, player2);
                    Task task = game.StartGame();
                    task.Wait();
                }




                catch (FormatException ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
                
            }
        }

        static string GetPlayerName(string playerType, char display)
        {
            Console.WriteLine($"\nType display name for {playerType}: ");
            string playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = playerType;
            }
            return playerName + "(" + display + ")";

        }
        static int GetAIDifficulty(string aiPlayerType)
        {
            Console.WriteLine($"\nSelect AI {aiPlayerType} difficulty (1-10): ");
            Console.WriteLine("WARNING: Higher difficulty means more time the AI needs to think");
            int aiDifficulty;
            while (true)
            {
                if(int.TryParse(Console.ReadLine(), out aiDifficulty) && aiDifficulty >= 1 && aiDifficulty <= 10)
                {
                    break;
                }
                Console.WriteLine("Invalid AI difficulty. Try again.");
            }
            return aiDifficulty;
        }
    }
}

