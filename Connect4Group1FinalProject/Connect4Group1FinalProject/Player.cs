using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connect4Group1FinalProject
{
    class Player : IPlayer
    {
        public CellState playerType { get; }
        public string playerName { get; }

        private int? lastMove;

        public Player(CellState playerType, string playerName)
        {
            this.playerType = playerType;
            this.playerName = playerName;
            lastMove = null;
        }

        public async Task<int> GetMove(Board board)
        {
            int move = -1;
            bool inputReceived = false;

            while (!inputReceived)
            {
                if (lastMove.HasValue)
                {
                    move = lastMove.Value;
                    lastMove = null;
                    inputReceived = true;
                    break;
                }

                await Task.Delay(50); // Delay to avoid CPU usage spike
            }

            return move;
        }

        public void SetLastMoveFromKey(char key)
        {
            if (int.TryParse(key.ToString(), out int move))
            {
                move--; // Adjust the move index
                lastMove = move;
            }
        }
    }







    class AIPlayer : IPlayer
    {
        private readonly int difficulty;
        private readonly Random random;

        public CellState playerType { get; }
        public string playerName { get; }

        public AIPlayer(CellState playerType, int difficulty, string playerName)
        {
            this.playerType = playerType;
            this.difficulty = difficulty;
            this.playerName = "AI " + playerName;
            random = new Random();
        }

        public Task<int> GetMove(Board board)
        {
            Console.WriteLine("AI is thinking...");
            Task.Delay(1000).Wait();

            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
            {
                return Task.FromResult(-1);
            }

            int move = Minimax(board, difficulty, int.MinValue, int.MaxValue, true).move;

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

            for (int row = 0; row < Board.Rows; row++)
            {
                for (int col = 0; col <= Board.Cols - 4; col++)
                {
                    score += EvaluateLine(board, row, col, 0, 1);
                }
            }

            for (int col = 0; col < Board.Cols; col++)
            {
                for (int row = 0; row <= Board.Rows - 4; row++)
                {
                    score += EvaluateLine(board, row, col, 1, 0);
                }
            }

            for (int row = 0; row <= Board.Rows - 4; row++)
            {
                for (int col = 0; col <= Board.Cols - 4; col++)
                {
                    score += EvaluateLine(board, row, col, 1, 1);
                }
            }

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
