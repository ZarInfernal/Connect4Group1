# Connect4Group1
Current Members: ZarInfernal = Michael Hiebert, Josshua Balita, Vergil Phan

# Connect Four Game - GROUP 1

The Connect Four Game is a two-player board game where the objective is to be the first player to connect four of their colored discs in a row, column, or diagonal on a grid of 6 rows and 7 columns. The game is played on a vertical standing board.

## How to Play
1. The game starts with an empty grid.
2. Players take turns dropping one of their colored discs into any of the seven columns.
3. The disc will occupy the lowest available position within the selected column.
4. The first player to connect four of their colored discs in a row, column, or diagonal wins the game.
5. If all columns are filled and no player has connected four discs, the game is a draw.

## Class Structure
The Connect Four game is implemented using the following classes:

1. **Board Class**: Represents the game board and contains methods to interact with the board, such as placing a disc, checking if a column is full, and checking for a winning condition.
   - Properties:
     - `Rows`: A constant representing the number of rows on the board (6).
     - `Cols`: A constant representing the number of columns on the board (7).
   
   - Methods:
     - `InitializeBoard()`: Initializes the board by setting all cells to `CellState.Empty`.
     - `ClearBoard()`: Clears the board and resets all cells to `CellState.Empty`.
     - `IsColumnFull(col)`: Checks if a specific column is already full. Returns a boolean value.
     - `GetCell(row, col)`: Retrieves the `CellState` at the specified row and column on the board.
     - `PlaceDisc(col, player)`: Places a disc of the specified player in the specified column. Returns a boolean value indicating if the disc was successfully placed.
     - `RemoveDisc(col)`: Removes the top disc from the specified column.
     - `IsGameOver(player)`: Checks if the game is over by determining if the specified player has won. Returns a boolean value.
     - `PrintBoard(moveRecords)`: Displays the current state of the game board. Accepts a list of move records as a parameter.
   
   The `Board` class maintains a 2D array called `cells`, which represents the cells on the board. Each cell is of type `CellState`, which can be one of the following:
     - `CellState.Empty`: Represents an empty cell on the board.
     - `CellState.Xeno`: Represents a cell occupied by a disc of the Xeno player.
     - `CellState.Oni`: Represents a cell occupied by a disc of the Oni player.

2. **Player Class**: Represents a player in the game and contains methods for obtaining the player's move.
   - Properties:
     - `PlayerColor`: The color (`CellState`) of the player's discs.
     - `playerName`: The name of the player.

   - Methods:
     - `GetMove(board)`: Retrieves the player's move by prompting for input. Returns the selected column as an integer.
     - `returnToMainMenu()`: Sets a flag indicating that the player wants to return to the main menu.
     - `SetLastMoveFromKey(key)`: Sets the player's last move based on the key input.

3. **AIPlayer Class**: Extends the `Player` class and represents an AI player. It includes additional methods for the AI's move selection.
   - Properties:
     - `random`: An instance of the `Random` class for generating random moves.
     - `difficulty`: The difficulty level of the AI player.

   - Methods:
     - `GetMove(board)`: Overrides the `GetMove` method of the `Player` class to implement AI logic for move selection.
     - `Minimax(board, depth, alpha, beta, maximizingPlayer)`: Implements the minimax algorithm for AI move selection.
     - `EvaluateBoard(board)`: Evaluates the score of the game board for the AI player.
     - `EvaluateLine(board, startRow, startCol, rowIncrement, colIncrement)`: Evaluates the score of a line of cells on the game board.

4. **GameController Class**: The `GameController` class controls the flow of the Connect Four game, manages the game state, and handles user input.

   - Properties:
     - `board`: An instance of the `Board` class representing the game board.
     - `player1`: An instance of the `IPlayer` interface representing the first player.
     - `player2`: An instance of the `IPlayer` interface representing the second player.
     - `currentPlayer`: An instance of the `IPlayer` interface representing the current player.
     - `moveRecords`: A list of strings representing the move records made by the players.
     - `gamePaused`: A boolean indicating whether the game is currently paused.
     - `mainMenu`: A boolean indicating whether the game is in the main menu.
       
   - Methods:
     - `StartGame()`: Starts the Connect Four game.
     - `ShowPauseMenuOptions()`: Displays the pause menu options and handles user input. Returns a boolean value indicating whether to resume the game or go to the main menu.
     - `ShowEndGameOptions()`: Displays the end game options and handles user input. Returns a boolean value indicating whether to reset the game or go to the main menu.
     - `ResetGame()`: Resets the game by clearing the board, move records, and setting the current player to player2.
     - `ClearInputBuffer()`: Clears the input buffer by consuming any available key presses.
     - `GetPlayerMoveAsync(player)`: Asynchronously gets the player's move. Returns a Task<int> representing the column chosen by the player.
     - `PlayerInputLoop()`: Monitors and handles player input, including pausing the game and cancelling moves.

## Game Modes
The Connect Four game supports the following game modes:

1. **Human vs. Human**: Both players are human players, taking turns to play on the same machine. They take turns entering their moves by specifying the column number (1-7) where they want to drop their disc.
2. **Human vs. AI**: One player is a human player, and the other player is an AI player. The human player enters their moves as in the Human vs. Human mode, while the AI opponent uses the minimax algorithm to make its moves.
3. **AI vs. AI**: Both players are AI players, and the game is fully automated. Both AI players use the minimax algorithm with different difficulty levels to make their moves.
4. Exit Game: This option allows you to exit the game.

To select a game mode, enter the corresponding number (1-4) when prompted.

## Player Types
1. Human: A human player who interacts with the game by entering moves through the command line.
2. AI: Represents an AI player who can make intelligent moves based on predefined strategies.

## Game Board
- The game board is represented as a grid of cells. Each cell can be in one of three states:
1. Empty (#): Represents an empty cell.
2. Xeno (X): Represents a disc placed by the Xeno player.
3. Oni (O): Represents a disc placed by the Oni player.
- The board is displayed at the start of each player's turn, showing the current state of the game.

-- Upon starting the program, the user is prompted to select a game mode. The game then begins with the selected mode.

## Running the Program
To run the Connect Four game program, follow these steps:
1. Compile and execute the C# program using a C# compiler or an integrated development environment (IDE)
2. Upon execution, the program prompts you to select a game mode by entering a number from 1 to 3.
3. Choose the desired game mode, and the game will start.
4. Follow the instructions provided by the program to play the game.
5. Enjoy the Connect Four game!


