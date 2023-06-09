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
        public const int Rows = 6;
        public const int Columns = 7;
        public char[,] board;
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
            Console.WriteLine("--------------------------------");
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
                    board[row, col] = playerSymbol;
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
                    if (CheckSequence(playerSymbol, board[row, col], board[row, col + 1], board[row, col + 2], board[row, col + 3]))
                        return true;
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
                    if (CheckSequence(playerSymbol, board[row, col], board[row + 1, col], board[row + 2, col], board[row + 3, col]))
                        return true;
                }
            }
            return false;
        }
        private bool CheckDiagonal(char playerSymbol)
        {
            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = 0; col <= Columns - 4; col++)
                {
                    if (CheckSequence(playerSymbol, board[row, col], board[row + 1, col + 1], board[row + 2, col + 2], board[row + 3, col + 3]))
                        return true;
                }
            }

            for (int row = 0; row <= Rows - 4; row++)
            {
                for (int col = Columns - 1; col >= 3; col--)
                {
                    if (CheckSequence(playerSymbol, board[row, col], board[row + 1, col - 1], board[row + 2, col - 2], board[row + 3, col - 3]))
                        return true;
                }
            }

            return false;
        }
        private bool CheckSequence(char playerSymbol, params char[] sequence)
        {
            foreach (char c in sequence)
            {
                if (c != playerSymbol)
                    return false;
            }
            return true;
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
        public virtual int GetMove()
        {
            int move;
            bool isValidMove = false;
            //do-While loop to check if User(Players) input is 1 >= 7, if not output invalid input/move
            do
            {
                Console.Write($"{Name}, enter your move (1-7): ");
                string input = Console.ReadLine();

                isValidMove = int.TryParse(input, out move) && move >= 1 && move <= GameBoard.Columns;

                if (!isValidMove)
                    Console.WriteLine("Invalid move. Please enter a valid column number.");

            } while (!isValidMove);
            //returns move subtracted to 1 to convert indexing which starts from 0
            // now it starts from 1 and ends with 7
            return move - 1;
        }
    }
    //AI class that is inherited from Player class
    class AI : Player
    {
        public AI(char symbol) : base("AI", symbol) { }
        public override int GetMove()
        {
            //Now, we implement AI logic to choose a move that generates a random move
            Random random = new Random();
            return random.Next(0, GameBoard.Columns);
        }
    }
    //starting menu class
    class StartingMenu
    {
        public static void ShowMenu()
        {
            Console.WriteLine("Welcome to Connect 4!");
            Console.WriteLine("\nPlease select an option:");
            Console.WriteLine("1. One Player");
            Console.WriteLine("2. Two Players ");
            Console.WriteLine("3. AI vs AI");
            Console.WriteLine("4. Exit Game");

            int option = GetValidOption();

            Console.WriteLine();

            switch (option)
            {
                case 1:
                    PlayAgainstAI();
                    break;
                case 2:
                    PlayAgainstPlayer();
                    break;
                case 3:
                    AIvsAI();
                    break;
                case 4:
                    ExitGame();
                    break;
            }
        }

        private static int GetValidOption()
        {
            bool validOption = false;
            int option = 0;

            while (!validOption)
            {
                Console.Write("\nEnter your choice (1-4): ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out option))
                {
                    if (option >= 1 && option <= 4)
                    {
                        validOption = true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a number between 1 and 4.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }

            return option;
        }
        // method to ask player's name 
        private static string GetPlayerName(string playerLabel)
        {
            Console.Write($"{playerLabel}'s name: ");
            return Console.ReadLine();
        }
        private static void PlayAgainstAI()
        {
            Console.WriteLine("Player vs AI");
            Console.WriteLine("--------------------------------");

            string playerName = GetPlayerName("Player");
            GameBoard board = new GameBoard();
            Player player1 = new Player(playerName, 'X');
            AI ai = new AI('O');


            PlayGame(board, player1, ai);
        }

        private static void PlayAgainstPlayer()
        {
            Console.WriteLine("Player 1 vs Player 2");
            Console.WriteLine("--------------------------------");

            string player1Name = GetPlayerName("Player 1");
            string player2Name = GetPlayerName("Player 2");

            GameBoard board = new GameBoard();
            Player player1 = new Player(player1Name, 'X');
            Player player2 = new Player(player2Name, 'O');

            PlayGame(board, player1, player2);
        }

        private static void AIvsAI()
        {
            Console.WriteLine("AI vs AI");
            Console.WriteLine("--------------------------------");

            GameBoard board = new GameBoard();
            AI ai1 = new AI('X');
            AI ai2 = new AI('O');

            PlayGame(board, ai1, ai2);
        }

        private static void PlayGame(GameBoard board, Player player1, Player player2)
        {
            while (true)
            {
                board.DrawBoard();

                int player1Move = GetPlayerMove(player1, board);
                board.MakeMove(player1Move, player1.Symbol);

                if (board.CheckWinCondition(player1.Symbol))
                {
                    board.DrawBoard();
                    Console.WriteLine("{0} wins!", player1.Name);
                    break;
                }

                if (board.IsBoardFull())
                {
                    board.DrawBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }

                int player2Move = GetPlayerMove(player2, board);
                board.MakeMove(player2Move, player2.Symbol);

                if (board.CheckWinCondition(player2.Symbol))
                {
                    board.DrawBoard();
                    Console.WriteLine("{0} wins!", player2.Name);
                    break;
                }

                if (board.IsBoardFull())
                {
                    board.DrawBoard();
                    Console.WriteLine("It's a draw!");
                    break;
                }
            }

            Console.WriteLine("GAME OVER!");
            Console.WriteLine();
            ShowMenu();
        }

        private static int GetPlayerMove(Player player, GameBoard board)
        {
            int playerMove = player.GetMove();
            while (!board.IsValidMove(playerMove))
            {
                Console.WriteLine("Invalid move. Please try again.");
                playerMove = player.GetMove();
            }
            return playerMove;
        }

        private static void ExitGame()
        {
            Console.WriteLine("Exiting game...");
            Environment.Exit(0);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            StartingMenu.ShowMenu();
        }    
    }
    
}
