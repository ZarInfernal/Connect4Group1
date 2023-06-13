﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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

    // Class to representing the game board
    class Board
    {
        public const int Rows = 6;
        public const int Cols = 7;
        private CellState[,] cells;

        public Board()
        {
            cells = new CellState[Rows, Cols];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
                for (int col = 0; col < Cols; col++)
                    cells[row, col] = CellState.Empty;

        }
        
        public void ClearBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    cells[row, col] = CellState.Empty;
                }
            }
        }
        public bool IsColumnFull(int col)
        {
            return cells[0, col] != CellState.Empty;
        }

        public CellState GetCell(int row, int col)
        {
            return cells[row, col];
        }

        public bool PlaceDisc(int col, CellState player) //Input player cellstate
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (cells[row, col] == CellState.Empty)
                {
                    cells[row, col] = player;
                    return true;
                }
            }

            return false; //Column is full
        }

        public void RemoveDisc(int col)
        {
            for (int row = 0; row < Rows; row++)
            {
                if (cells[row, col] != CellState.Empty)
                {
                    cells[row, col] = CellState.Empty;
                    break;
                }
            }
        }

        public bool IsGameOver(CellState player)
        {
            return CheckHorizontal(player) || CheckVertical(player) || CheckDiagonal(player);
        }

        private bool CheckHorizontal(CellState player)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (CheckSequence(player, cells[row, col], cells[row, col+1], cells[row, col+2], cells[row, col+3]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckVertical(CellState player)
        {
            for (int col = 0; col < Cols; col++)
            {
                for (int row = 0; row < Rows - 3; row++)
                {
                    if (CheckSequence(player, cells[row, col], cells[row + 1, col], cells[row + 2, col], cells[row + 3, col]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDiagonal(CellState player)
        {
            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = 0; col < Cols - 3; col++)
                {
                    if (CheckSequence(player, cells[row, col], cells[row + 1, col + 1], cells[row + 2, col + 2], cells[row + 3, col + 3]))
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row < Rows - 3; row++)
            {
                for (int col = Cols-1; col >= Cols - 4; col--)
                {
                    if (CheckSequence(player, cells[row, col], cells[row + 1, col - 1], cells[row + 2, col - 2], cells[row + 3, col - 3]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckSequence(CellState player, params CellState[] cellState)
        {
            foreach (CellState cell in cellState)
            {
                if (cell != player)
                    return false;
            }
            return true;
        }

        public void PrintBoard(List<string> moveRecords)
        {
            for (int row = 0; row < Rows; row++)
            {
                Console.Write("| ");
                for (int col = 0; col < Cols; col++)
                {
                    switch(cells[row, col])
                    {
                        case CellState.Empty:
                            Console.Write("# ");
                            break;
                        case CellState.Xeno:
                            Console.Write("X ");
                            break;
                        case CellState.Oni:
                            Console.Write("O ");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                Console.Write("|");
                Console.WriteLine();
            }
            Console.WriteLine("--1-2-3-4-5-6-7--");
            Console.WriteLine();

            Console.WriteLine("Player Moves:");
            Console.WriteLine("=============");
            int start = Math.Max(moveRecords.Count - 2, 0);
            for (int i = start; i < moveRecords.Count; i++)
            {
                Console.WriteLine(moveRecords[i]);
            }

            Console.WriteLine("\n");
        }

    }

    class Player : IPlayer
    {
        public CellState playerType { get; }
        public string playerName { get; }

         public Player(CellState PlayerType, string playerName)
         {
            playerType = PlayerType;
            this.playerName = playerName;
         }

         public async Task<int> GetMove(Board board)
         {
            Console.WriteLine("Enter the column number (1-7): ");
            string input = await Task.Run(Console.ReadLine);
            int move = int.Parse(input);
            return move - 1; // This will make so if 1 is entered it will be 0 instead so the inputs are 1 - 7 instead of 0 - 6
         }
    }

    class AIPlayer : IPlayer
    {
        private readonly int difficulty;

        public CellState playerType { get; }
        public string playerName { get; }

        public AIPlayer(CellState playerType, int difficulty, string playerName)
        {
            this.playerType = playerType;
            this.difficulty = difficulty;
            this.playerName = "AI " + playerName;
        }

        public Task<int> GetMove(Board board)
        {
            Console.WriteLine("AI is thinking...");
            System.Threading.Thread.Sleep(1000);

            int move = Minimax(board, difficulty, int.MinValue, int.MaxValue, true).move;

            // If no valid move is found, choose a random move from the available columns
            if (move == -1)
            {
                List<int> availableColumns = new List<int>();
                for (int col = 0; col < Board.Cols; col++)
                {
                    if (!board.IsColumnFull(col))
                    {
                        availableColumns.Add(col);
                    }
                }

                Random random = new Random();
                move = availableColumns[random.Next(availableColumns.Count)];
            }

            return Task.FromResult(move);
        }

        private (int move, int score) Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            if (depth == 0 || board.IsGameOver(playerType) || board.IsGameOver(GetOpponentPlayerType()))
            {
                return (move: -1, score: EvaluateBoard(board));
            }

            int bestMove = -1;
            int bestScore = maximizingPlayer ? int.MinValue : int.MaxValue;

            for (int col = 0; col < Board.Cols; col++)
            {
                if (!board.IsColumnFull(col))
                {
                    board.PlaceDisc(col, maximizingPlayer ? playerType : GetOpponentPlayerType());

                    int score = Minimax(board, depth - 1, alpha, beta, !maximizingPlayer).score;

                    board.RemoveDisc(col);

                    if (maximizingPlayer)
                    {
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = col;
                        }
                        alpha = Math.Max(alpha, bestScore);
                    }
                    else
                    {
                        if (score < bestScore)
                        {
                            bestScore = score;
                            bestMove = col;
                        }
                        beta = Math.Min(beta, bestScore);
                    }

                    if (beta <= alpha)
                    {
                        break;
                    }
                }
            }

            return (move: bestMove, score: bestScore);
        }

        private int EvaluateBoard(Board board)
        {
            int score = 0;

            // Evaluate rows
            for (int row = 0; row < Board.Rows; row++)
            {
                for (int col = 0; col <= Board.Cols - 4; col++)
                {
                    score += EvaluateLine(board, row, col, 0, 1);
                }
            }

            // Evaluate columns
            for (int col = 0; col < Board.Cols; col++)
            {
                for (int row = 0; row <= Board.Rows - 4; row++)
                {
                    score += EvaluateLine(board, row, col, 1, 0);
                }
            }

            // Evaluate diagonals (top-left to bottom-right)
            for (int row = 0; row <= Board.Rows - 4; row++)
            {
                for (int col = 0; col <= Board.Cols - 4; col++)
                {
                    score += EvaluateLine(board, row, col, 1, 1);
                }
            }

            // Evaluate diagonals (bottom-left to top-right)
            for (int row = 3; row < Board.Rows; row++)
            {
                for (int col = 0; col <= Board.Cols - 4; col++)
                {
                    score += EvaluateLine(board, row, col, -1, 1);
                }
            }

            return score;
        }

        private int EvaluateLine(Board board, int startRow, int startCol, int rowIncrement, int colIncrement)
        {
            int score = 0;
            int playerCount = 0;
            int opponentCount = 0;

            for (int i = 0; i < 4; i++)
            {
                CellState cell = board.GetCell(startRow + i * rowIncrement, startCol + i * colIncrement);

                if (cell == playerType)
                {
                    playerCount++;
                }
                else if (cell == GetOpponentPlayerType())
                {
                    opponentCount++;
                }
            }

            if (playerCount == 4)
            {
                score += 1000;
            }
            else if (playerCount == 3 && opponentCount == 0)
            {
                score += 100;
            }
            else if (playerCount == 2 && opponentCount == 0)
            {
                score += 10;
            }
            else if (playerCount == 1 && opponentCount == 0)
            {
                score += 1;
            }
            else if (opponentCount == 4)
            {
                score -= 1000;
            }
            else if (opponentCount == 3 && playerCount == 0)
            {
                score -= 100;
            }
            else if (opponentCount == 2 && playerCount == 0)
            {
                score -= 10;
            }
            else if (opponentCount == 1 && playerCount == 0)
            {
                score -= 1;
            }

            return score;
        }

        private CellState GetOpponentPlayerType()
        {
            return playerType == CellState.Xeno ? CellState.Oni : CellState.Xeno;
        }
    }

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

