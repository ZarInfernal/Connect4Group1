using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Game
{
    // Game Board Class 
    // Methods and Properties to manage the game board
    class GameBoard 
    {
        private const int Rows = 6;
        private const int Columns = 7;
        private char[,] board;
        public GameBoard()
        {
            board = new char[Rows, Columns];
            InitializeBoard();
        }
        private void InitializeBoard()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                    board[row, column] = '-';
            }
        }
        public void DrawBoard()
        {
            Console.WriteLine("  1 2 3 4 5 6 7");
            Console.WriteLine("---------------");
            for (int row = 0; row < Rows; row++)
            {
                Console.Write("|");
                for (int col = 0; col < Columns; col++)
                {
                    Console.Write(board[row, col]);
                    Console.Write("|");
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------");
        }
        public bool IsValidMove(int col)
        {
            return col >= 0 && col < Columns && board[0, col] == '-';
        }
        public void MakeMove(int col, char playerSymbol)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (board[row, col] == '-')
                {
                    board[row,col] = playerSymbol;
                    break;
                }
            }
        }
        public bool IsBoardFull()
        {
            for (int col = 0; col < Columns; col++)
            {
                if (board[0, col] == '-')
                    return false;
            }
            return true;
        }
        public bool CheckWinCondition(char playerSymbol)
        {
            return CheckHorizontal(playerSymbol) || CheckVertical(playerSymbol) || CheckDiagonal(playerSymbol);
        }
        private bool CheckHorizontal(char playerSymbol)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (board[row, col] != '-' &&
                        board[row, col] == playerSymbol &&
                        board[row, col] == board[row, col + 1] &&
                        board[row, col] == board[row, col + 2] &&
                        board[row, col] == board[row, col + 3])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckVertical(char playerSymbol)
        {
            for (int col = 0; col < Columns; col++)
            {
                for (int row = 0; row <= Rows - 4; row++)
                {
                    if (board[row, col] != '-' &&
                        board[row, col] == playerSymbol &&
                        board[row, col] == board[row + 1, col] &&
                        board[row, col] == board[row + 2, col] &&
                        board[row, col] == board[row + 3, col])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private bool CheckDiagonal(char playerSymbol)
        {
            // Check diagonals from top-left to bottom-right
            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (board[row, col] != '-' &&
                        board[row, col] == playerSymbol &&
                        board[row, col] == board[row + 1, col + 1] &&
                        board[row, col] == board[row + 2, col + 2] &&
                        board[row, col] == board[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            // Check diagonals from top-right to bottom-left
            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = Columns - 1; col >= 3; col--)
                {
                    if (board[row, col] != '-' &&
                        board[row, col] == playerSymbol &&
                        board[row, col] == board[row + 1, col - 1] &&
                        board[row, col] == board[row + 2, col - 2] &&
                        board[row, col] == board[row + 3, col - 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
    //This is just a test case, but game is functional already
    //Will add more classes to make it Final
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Connect 4!");

            GameBoard gameBoard = new GameBoard();
            char currentPlayer = 'X';

            while (true)
            {
                gameBoard.DrawBoard();
                Console.WriteLine($"Player {currentPlayer}, enter the column (1-7): ");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int col))
                {
                    col--;
                    if (gameBoard.IsValidMove(col))
                    {
                        gameBoard.MakeMove(col, currentPlayer);
                        if (gameBoard.CheckWinCondition(currentPlayer))
                        {
                            gameBoard.DrawBoard();
                            Console.WriteLine($"Player {currentPlayer} wins!");
                            break;
                        }
                        else if (gameBoard.IsBoardFull())
                        {
                            gameBoard.DrawBoard();
                            Console.WriteLine("It's a tie!");
                            break;
                        }
                        currentPlayer = currentPlayer == 'X' ? '#' : 'X';
                    }
                    else
                    {
                        Console.WriteLine("Invalid move! Try again.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Try again.");
                }
            }

            Console.WriteLine("Thank you for playing Connect 4!");
      
        }
    }

}
