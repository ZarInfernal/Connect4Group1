using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4Group1FinalProject
{
    class ConnectFourGame
    {
        private readonly Board board;
        private readonly IPlayer player1;
        private readonly IPlayer player2;
        private IPlayer currentPlayer;
        private List<string> moveRecords;
        private bool gamePaused;

        public ConnectFourGame(IPlayer player1, IPlayer player2)
        {
            board = new Board();
            this.player1 = player1;
            this.player2 = player2;
            moveRecords = new List<string>();
            gamePaused = false;
        }

        public void StartGame()
        {
            currentPlayer = player1;
            Task.Run(PlayerInputLoop);

            while (true)
            {
                if (!gamePaused)
                {
                    Console.Clear();
                    board.PrintBoard(moveRecords);
                    Console.WriteLine($"Current player: {currentPlayer.playerName}");

                    int move = GetPlayerMoveAsync(currentPlayer).GetAwaiter().GetResult();

                    while (board.IsColumnFull(move))
                    {
                        Console.WriteLine("Column is full. Choose a different column.");
                        move = GetPlayerMoveAsync(currentPlayer).GetAwaiter().GetResult();
                    }

                    board.PlaceDisc(move, currentPlayer.playerType);
                    moveRecords.Add($"{currentPlayer.playerName} chose column {move + 1}.");

                    if (board.IsGameOver(currentPlayer.playerType))
                    {
                        Console.Clear();
                        board.PrintBoard(moveRecords);
                        Console.WriteLine($"{currentPlayer.playerName} wins!");
                        if (!ShowEndGameOptions())
                            break;
                    }

                    currentPlayer = (currentPlayer == player1) ? player2 : player1;
                }
                else
                {
                    Console.Clear();
                    board.PrintBoard(moveRecords);
                    if (!ShowPauseMenuOptions())
                        break;
                }
            }
        }

        private bool ShowPauseMenuOptions()
        {
            Console.WriteLine("\nGame Paused");
            Console.WriteLine("1. Resume Game");
            Console.WriteLine("2. Reset Game");
            Console.WriteLine("3. Return to Main Menu");
            Console.WriteLine("4. Exit Game\n");

            Console.Write("Enter your choice (1-4): ");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine().Trim(), out choice) && choice >= 1 && choice <= 4)
                {
                    break;
                }

                Console.WriteLine("Invalid choice. Try again.");
            }

            switch (choice)
            {
                case 1:
                    gamePaused = false;
                    return true;
                case 2:
                    ResetGame();
                    return true;
                case 3:
                    Console.Clear();
                    return false;
                case 4:
                    Console.WriteLine("Exiting the game...");
                    Environment.Exit(0); // Exit the program
                    return false;
                default:
                    Console.WriteLine("Invalid choice. Resuming game...");
                    gamePaused = false;
                    return true;
            }
        }

        private bool ShowEndGameOptions()
        {
            Console.WriteLine("\nGAME OVER!");
            Console.WriteLine("1. Reset Game");
            Console.WriteLine("2. Return to Main Menu");
            Console.WriteLine("3. Exit Game\n");

            Console.Write("Enter your choice (1-3): ");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine().Trim(), out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }

                Console.WriteLine("Invalid choice. Try again.");
            }

            switch (choice)
            {
                case 1:
                    ResetGame();
                    return true;
                case 2:
                    Console.Clear();
                    return false;
                case 3:
                    Console.WriteLine("Exiting the game...");
                    Environment.Exit(0); // Exit the program
                    return false;
                default:
                    Console.WriteLine("Invalid choice. Resuming game...");
                    return true;
            }
        }

        private void ResetGame()
        {
            board.ClearBoard();
            moveRecords.Clear();
            currentPlayer = player1;
            gamePaused = false;
        }

        private async Task<int> GetPlayerMoveAsync(IPlayer player)
        {
            int move;
            if (player is AIPlayer)
            {
                move = await player.GetMove(board);
            }
            else
            {
                move = await Task.Run(() =>
                {
                    int playerMove;
                    while (!int.TryParse(Console.ReadLine().Trim(), out playerMove))
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                    return playerMove - 1;
                });
            }
            return move;
        }

        private void PlayerInputLoop()
        {
            while (true)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    gamePaused = !gamePaused;
                }
                Thread.Sleep(50);
            }
        }
    }
}
