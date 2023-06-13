using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private CancellationTokenSource cancellationTokenSource;
        private bool gamePaused;

        public ConnectFourGame(IPlayer player1, IPlayer player2)
        {
            board = new Board();
            this.player1 = player1;
            this.player2 = player2;
            moveRecords = new List<string>();
            cancellationTokenSource = new CancellationTokenSource();
            gamePaused = false;
        }

        public async Task StartGame()
        {
            currentPlayer = player1;
            Task.Run(PlayerInputLoop);

            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (!gamePaused)
                {
                    Console.Clear();
                    board.PrintBoard(moveRecords);
                    Console.WriteLine($"Current player: {currentPlayer.playerName}");

                    try
                    {
                        int move;
                        if (currentPlayer is AIPlayer)
                        {
                            move = await currentPlayer.GetMove(board);
                        }
                        else
                        {
                            move = await currentPlayer.GetMove(board).ConfigureAwait(false);
                        }
                        while (board.IsColumnFull(move))
                        {
                            Console.WriteLine("Column is full. Choose a different column.");
                            if (currentPlayer is AIPlayer)
                            {
                                move = await currentPlayer.GetMove(board);
                            }
                            else
                            {
                                move = await currentPlayer.GetMove(board).ConfigureAwait(false);
                            }
                        }

                        board.PlaceDisc(move, currentPlayer.playerType);
                        moveRecords.Add($"{currentPlayer.playerName} chose column {move + 1}.");

                        if (board.IsGameOver(currentPlayer.playerType))
                        {
                            Console.Clear();
                            board.PrintBoard(moveRecords);
                            Console.WriteLine($"{currentPlayer.playerName} wins!");
                            ShowEndGameOptions();
                            break;
                        }

                        currentPlayer = (currentPlayer == player1) ? player2 : player1;
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("An error occurred: " + e.Message);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("An error occurred: " + e.Message);
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }

                }
                else
                {
                    Console.Clear();
                    board.PrintBoard(moveRecords);
                    ShowPauseMenuOptions();
                }
            }
        }

        private void ShowPauseMenuOptions()
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

            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }

            switch (choice)
            {
                case 1:
                    gamePaused = false;
                    break;
                case 2:
                    ResetGame();
                    break;
                case 3:
                    cancellationTokenSource.Cancel();
                    Console.Clear();
                    break;
                case 4:
                    Console.WriteLine("Exiting the game...");
                    cancellationTokenSource.Cancel();
                    Environment.Exit(0); // Exit the program
                    break;
                default:
                    Console.WriteLine("Invalid choice. Resuming game...");
                    gamePaused = false;
                    break;
            }
        }

        private void ShowEndGameOptions()
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

            while (Console.KeyAvailable)
            {
                Console.ReadKey(false);
            }

            switch (choice)
            {
                case 1:
                    ResetGame();
                    break;
                case 2:
                    cancellationTokenSource.Cancel();
                    Console.Clear();
                    break;
                case 3:
                    Console.WriteLine("Exiting the game...");
                    cancellationTokenSource.Cancel();
                    Environment.Exit(0); // Exit the program
                    break;
                default:
                    Console.WriteLine("Invalid choice. Resuming game...");
                    break;
            }
        }

        private void ResetGame()
        {
            board.ClearBoard();
            moveRecords.Clear();
            currentPlayer = player1;
            gamePaused = false;
        }

        private async Task PlayerInputLoop()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {

                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    gamePaused = !gamePaused;
                }

                await Task.Delay(50);
            }
        }

    }
}
