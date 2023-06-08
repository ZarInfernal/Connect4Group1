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
        Oni
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
            return CheckHorizontal(player) || CheckVertical(player) || CheckDiagonal(player);
        }

        private bool CheckHorizontal(CellState player)
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols - 4; col++)
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
                for (int row = 0; row < Rows - 4; row++)
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
            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = 0; col <= Cols - 4; col++)
                {
                    if (CheckSequence(player, cells[row, col], cells[row + 1, col + 1], cells[row + 2, col + 2], cells[row + 3, col + 3]))
                    {
                        return true;
                    }
                }
            }

            for (int row = 0; row <= Rows; row++)
            {
                for (int col = Cols-1; col >= 3; col--)
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
            foreach (CellState cell in cells)
            {
                if (cell == CellState.Empty)
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

         public virtual int GetMove(Board board)
         {
             Console.WriteLine("Enter the column number (1-7): ");
             int move = int.Parse(Console.ReadLine());
             return move - 1; // This will make so if 1 is entered it will be 0 instead so the inputs are 1 - 7 instead of 0 - 6
         }
    }

    // Class that will manage the game
    class ConnectFourGame
    {
        private readonly Board board;
        private readonly Player player1;
        private readonly Player player2;
        private Player currentPlayer;
        private int lastMove; // Will help with ai

        public ConnectFourGame(PlayerType player1Type, PlayerType player2Type)
        {
            board = new Board();

            switch (player1Type)
            {
                case PlayerType.Human:
                    player1 = new Player(CellState.Xeno);
                    break;
                case PlayerType.AI:
                    //Add ai selector once ai is made
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (player2Type)
            {
                case PlayerType.Human:
                    player2 = new Player(CellState.Oni);
                    break;
                case PlayerType.AI:
                    // Same as in player1
                    break;
                default:
                    throw new NotImplementedException();

            }

            currentPlayer = player1;
            lastMove = -1;
        }

        public void Start()
        {
            Console.WriteLine("Group 1 Connect Four Game");
            Console.WriteLine("Enter the column number (1-7) to make a move.");
            Console.WriteLine();

            while (true)
            {
                Console.Clear();
                board.PrintBoard();
                int move = currentPlayer.GetMove(board);
                if (board.PlaceDisc(move, currentPlayer.PlayerType))
                {
                    if (board.IsGameOver(currentPlayer.PlayerType))
                    {
                        Console.Clear();
                        board.PrintBoard();
                        Console.WriteLine($"Player {currentPlayer.PlayerType} wins!");
                        break;
                    }

                    if (IsBoardFull())
                    {
                        Console.WriteLine("It's a draw!");
                        break;
                    }

                    currentPlayer = (currentPlayer == player1) ? player2 : player1;
                }
                else
                {
                    Console.WriteLine("Column is full. Try again.");
                    Console.Read();
                }
            }

        }

        private bool IsBoardFull()
        {
            for (int col = 0; col < 7; col++)
            {
                if (!board.IsColumnFull(col))
                    return false;
            }
            return true;
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

        int mode;
        while (true)
        {
            Console.Write("Enter the game mode (1-3): ");
            if (int.TryParse(Console.ReadLine(), out mode) && mode >= 1 && mode <= 3)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid game mode.");
                Console.WriteLine();
            }
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
                Console.WriteLine("Invalid game mode.");
                return;
        }

        ConnectFourGame game = new ConnectFourGame(player1Type, player2Type);
        game.Start();

            
        }
    }
}
