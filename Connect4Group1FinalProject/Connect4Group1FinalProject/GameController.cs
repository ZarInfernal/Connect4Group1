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
                    int move;
                    try
                    {
                        move = GetPlayerMoveAsync(currentPlayer).GetAwaiter().GetResult();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error occurred: {e.Message}");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        continue;
                    }

                    while (board.IsColumnFull(move))
                    {
                        Console.WriteLine("Column is full. Choose a different column.");
                        try
                        {
                            move = GetPlayerMoveAsync(currentPlayer).GetAwaiter().GetResult();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error occurred: {e.Message}");
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                            continue;
                        }
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
            ClearInputBuffer();
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
                    Console.Clear();
                    board.PrintBoard(moveRecords);
                    Console.WriteLine($"Current player: {currentPlayer.playerName}");
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
            ClearInputBuffer();
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

        private void ClearInputBuffer()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(false);
        }

        private async Task<int> GetPlayerMoveAsync(IPlayer player)
        {
            int move = -1;
            bool inputReceived = false;

            while (!inputReceived)
            {
                move = await player.GetMove(board);

                if (move == -1) // Check if move is canceled (escape key pressed)
                {
                    Console.WriteLine("Move canceled. Try again.");
                }
                else if (move < 0 || move >= Board.Cols)
                {
                    Console.WriteLine("Invalid column. Try again.");
                }
                else if (board.IsColumnFull(move))
                {
                    Console.WriteLine("Column is full. Choose a different column.");
                }
                else
                {
                    inputReceived = true; // Mark input as received
                }

                await Task.Delay(50); // Delay to avoid high CPU usage
            }

            return move;
        }

        private void PlayerInputLoop()
        {
            while (true)
            {
                try
                {
                    if (!gamePaused && Console.KeyAvailable)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            gamePaused = true;
                            Console.Clear();
                            board.PrintBoard(moveRecords);
                            if (!ShowPauseMenuOptions())
                                return;

                        }
                        else if (currentPlayer is Player)
                        {
                            var player = (Player)currentPlayer;
                            player.SetLastMoveFromKey(keyInfo.KeyChar);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    // Ignore the exception caused by clearing the input buffer
                }
                Thread.Sleep(50);
            }
        }
    }
}
