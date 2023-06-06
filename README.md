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

1. **Board**: Represents the game board and contains methods to interact with the board, such as placing a disc, checking if a column is full, and checking for a winning condition.
   - Properties:
     - `Rows`: A constant representing the number of rows on the board (6).
     - `Cols`: A constant representing the number of columns on the board (7).
     - `cells`: A 2D array of `CellState` representing the cells on the board.
   - Methods:
     - `InitializeBoard()`: Initializes the board by setting all cells to `CellState.Empty`.
     - `IsColumnFull(col)`: Checks if a specific column is already full.
     - `PlaceDisc(col, player)`: Places a disc of the specified player in the specified column.
     - `RemoveDisc(col)`: Removes the top disc from the specified column.
     - `IsGameOver(player)`: Checks if the game is over by determining if the specified player has won.
     - `PrintBoard()`: Displays the current state of the game board.

2. **Player**: Represents a player in the game and contains methods for obtaining the player's move.
   - Properties:
     - `PlayerColor`: The color (`CellState`) of the player's discs.
   - Methods:
     - `GetMove(board)`: Retrieves the player's move by prompting for input. Returns the selected column as an integer.

3. **AIPlayer**: Extends the `Player` class and represents an AI player. It includes additional methods for the AI's move selection.
   - Properties:
     - `random`: An instance of the `Random` class for generating random moves.
     - `delay`: The delay time in milliseconds for the AI's move.
   - Methods:
     - `GetMove(board)`: Overrides the `GetMove` method of the `Player` class to implement AI logic for move selection.
     - `GetWinningMove(board)`: Checks if there is a winning move available for the AI player.
     - `GetBlockingMove(board)`: Checks if there is a blocking move available to prevent the opponent from winning.
     - `GetRandomMove(board)`: Selects a random valid move for the AI player.

4. **ConnectFourGame**: Represents the Connect Four game itself and manages the gameplay loop.
   - Properties:
     - `board`: An instance of the `Board` class representing the game board.
     - `player1`: An instance of the `Player` class representing the first player.
     - `player2`: An instance of the `Player` class representing the second player.
   - Methods:
     - `Start()`: Starts the game and handles the main gameplay loop.
     - `IsBoardFull()`: Checks if the game board is full.

## Game Modes
The Connect Four game supports the following game modes:

1. **Human vs. Human**: Both players are human players, taking turns to play on the same machine.
2. **Human vs. AI**: One player is a human player, and the other player is an AI player.
3. **AI vs. AI**: Both players are AI players, and the game is fully automated.

Upon starting the program, the user is prompted to select a game mode. The game then begins with the selected mode.

## Running the Program
To run the Connect Four game program, follow these steps:
1. Compile and execute the C# program using a C# compiler or an integrated development environment (IDE)
2. Upon execution, the program prompts you to select a game mode by entering a number from 1 to 3.
3. Choose the desired game mode, and the game will start.
4. Follow the instructions provided by the program to play the game.
5. Enjoy the Connect Four game!


