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
        Xeno,
        Yao
    }

    //Class to representing the game board
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

        //Make another class here to help with ai later

        public bool IsGameOver(CellState player)
        {
            // Check for winning conditions (Will come back later)
            return false;
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
                        case CellState.Yao:
                            Console.Write("Y");
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

    }


        class Player
        {
            public CellState PlayerType { get; }

            public Player(CellState playerType)
            {
                PlayerType = playerType;
            }
        }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello");
        }
    }
}
