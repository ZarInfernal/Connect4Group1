using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4Group1FinalProject
{
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
                    if (CheckSequence(player, cells[row, col], cells[row, col + 1], cells[row, col + 2], cells[row, col + 3]))
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
                for (int col = Cols - 1; col >= Cols - 4; col--)
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
                    switch (cells[row, col])
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
}
