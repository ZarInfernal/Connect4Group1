using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        // Vergil: I updated Xeno and Oni back to Red and Yellow to make it clearer
        Red,
        Yellow
    }

    // Interface for the players
    interface IPlayer
    {
        CellState playerType { get; }
        int GetMove(Board board);
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

        public void PrintBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    switch(cells[row, col])
                    {
                        case CellState.Empty:
                            Console.Write("#");
                            break;
                        case CellState.Xeno:
                            Console.Write("X");
                            break;
                        case CellState.Oni:
                            Console.Write("O");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            Console.WriteLine("Player Moves:");
            Console.WriteLine("=============");
            foreach (string move in moveRecords)
            {
                Console.WriteLine(move);
            }

            Console.WriteLine();
        }

    }

    class Player : IPlayer
    {
         public CellState playerType { get; }

         public Player(CellState PlayerType)
         {
             playerType = PlayerType;
         }

         public virtual int GetMove(Board board)
         {
             Console.WriteLine("Enter the column number (1-7): ");
             int move = int.Parse(Console.ReadLine());
             return move - 1; // This will make so if 1 is entered it will be 0 instead so the inputs are 1 - 7 instead of 0 - 6
         }
    }

    class AIPlayer : IPlayer
    {
        private readonly int difficulty;

        public CellState playerType { get; }

        public AIPlayer(CellState playerColor, int difficulty)
        {
            playerType = playerColor;
            this.difficulty = difficulty;
        }

        public int GetMove(Board board)
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

            return move;
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

    // Class that will manage the game
// Vergil: I updated the class ConnectFourGame to have the records of two players

class ConnectFourGame
    {
        private Board board;
        private Player redPlayer;
        private Player yellowPlayer;
        private List<string> moveRecords;

        public ConnectFourGame(PlayerType redPlayerType, PlayerType yellowPlayerType)
        {
            board = new Board();
            redPlayer = CreatePlayer(redPlayerType, CellState.Red);
            yellowPlayer = CreatePlayer(yellowPlayerType, CellState.Yellow);
            moveRecords = new List<string>();
        }

        private Player CreatePlayer(PlayerType playerType, CellState playerColor)
        {
            switch (playerType)
            {
                case PlayerType.Human:
                    return new Player(playerColor);
                case PlayerType.AI:
                    return new AIPlayer(playerColor, 2000); // AI delay: 2 seconds
                default:
                    throw new NotImplementedException();
            }
        }

        public void StartGame()
        {
            Player currentPlayer = redPlayer;

            while (true)
            {
                board.PrintBoard(moveRecords);

                Console.WriteLine($"Current player: {(currentPlayer == redPlayer ? "Red" : "Yellow")}");

                int move = currentPlayer.GetMove(board);
                while (board.IsColumnFull(move))
                {
                    Console.WriteLine("Column is full. Choose a different column.");
                    move = currentPlayer.GetMove(board);
                }

                board.PlaceDisc(move, currentPlayer.PlayerColor);
                moveRecords.Add($"{(currentPlayer == redPlayer ? "Red" : "Yellow")} player chose column {move + 1}.");

                if (board.IsGameOver(currentPlayer.PlayerColor))
                {
                    board.PrintBoard(moveRecords);
                    Console.WriteLine($"{(currentPlayer == redPlayer ? "Red" : "Yellow")} player wins!");
                    break;
                }

                currentPlayer = (currentPlayer == redPlayer) ? yellowPlayer : redPlayer;
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
        Console.WriteLine("Connect Four Game");
        Console.WriteLine("=================");
        Console.WriteLine("Game Modes:");
        Console.WriteLine("1. Human vs. Human");
        Console.WriteLine("2. Human vs. AI");
        Console.WriteLine("3. AI vs. AI");
        Console.WriteLine();
            
            // Vergil: I updated this part so it could work with the class ConnectFourGame
            
                        Console.Write("Enter the game mode (1-3): ");
            int mode;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out mode) && mode >= 1 && mode <= 3)
                {
                    break;
                }

                Console.WriteLine("Invalid game mode. Try again.");
            }

            PlayerType player1Type, player2Type;
            switch (mode)
            {
                case 1:
                    player1Type = PlayerType.Human;
                    player2Type = PlayerType.Human;
                    break;
                case 2:
                    player1Type = PlayerType.Human;
                    player2Type = PlayerType.AI;
                    break;
                case 3:
                    player1Type = PlayerType.AI;
                    player2Type = PlayerType.AI;
                    break;
                default:
                    throw new InvalidOperationException("Invalid game mode.");
            }

            ConnectFourGame game = new ConnectFourGame(player1Type, player2Type);
            game.StartGame();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}

