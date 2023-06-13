using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Group1FinalProject
{
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
}
