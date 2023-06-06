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
            Console.WriteLine(" 1 2 3 4 5 6 7");
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
      // Player class
    class Player
    { 
        public string Name { get; private set; }
        public char Symbol { get; private set; }
        public Player(string name, char symbol) 
        {
            Name = name;
            Symbol = symbol;
        }
        public int GetMove()
        {
            Console.WriteLine("Player{Name}, enter your move(1-7): ");
            string input = Console.ReadLine();
            int move;
            //While loop to check if User(Players) input is 1 >= 7, if not output invalid input/move
            while (!int.TryParse(input, out move) || move < 1 || move > GameBoard.Columns)
            {
                Console.WriteLine("Invalid move. Please enter a valid column number.");
                input = Console.ReadLine();
            }
            //returns move subtracted to 1 to convert indexing which starts from 0
            // now it starts from 1 and ends with 7
            return move - 1;
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
           
        }
    }

}
