using System;

namespace sergeantTheyDunHitThePentagon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[,] gameBoard = new string[10, 10];
            string[,] aiGameBoard = new string[10, 10];
            string[,] aiBoatBoard = new string[10, 10];
            string currentPlayer = "X";
            int yourRowMove = 0;
            int yourColumnMove = 0;
            int aiRowMove = 0;
            int aiColumnMove = 0;
            bool gameOver = false;
            bool winOrDraw = false;
            char playAgain = ' ';
            Random random = new Random();
            InitialiseGameBoard(gameBoard, aiGameBoard);
            DisplayBoard(gameBoard, aiGameBoard);

            while (!gameOver)
            {
                TakeTurn(gameBoard, aiGameBoard, aiBoatBoard, ref currentPlayer, ref aiRowMove, ref aiColumnMove, ref yourRowMove, ref yourColumnMove, ref random);
                DisplayBoard(gameBoard, aiGameBoard);
                winOrDraw = CheckForWin(gameBoard, aiGameBoard);

                if (winOrDraw)
                {
                    Console.WriteLine("Do you want to play again? Y for yes and any other key to exit.");
                    playAgain = Console.ReadKey().KeyChar;

                    if (char.ToUpper(playAgain) != 'Y')
                    {
                        gameOver = true;
                    }
                    else
                    {
                        InitialiseGameBoard(gameBoard, aiGameBoard);
                        DisplayBoard(gameBoard, aiGameBoard);
                    }
                }
            }
        }

        private static void InitialiseGameBoard(string[,] gameBoard, string[,] aiGameBoard)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    gameBoard[i, j] = " ";
                    aiGameBoard[i, j] = " ";
                }
            }

            // Add boats to the game board
            AddBoats(gameBoard, 4);
            AddBoats(aiGameBoard, 4);
        }

        private static void AddBoats(string[,] gameBoard, int boatCount)
        {
            Random random = new Random();

            for (int i = 0; i < boatCount; i++)
            {
                bool boatAdded = false;

                while (!boatAdded)
                {
                    int row = random.Next(0, 10);
                    int column = random.Next(0, 10);
                    bool isHorizontal = random.Next(0, 2) == 0;

                    if (isHorizontal && column + 2 < 10 && gameBoard[row, column] == " " && gameBoard[row, column + 1] == " " && gameBoard[row, column + 2] == " ")
                    {
                        gameBoard[row, column] = "B";
                        gameBoard[row, column + 1] = "B";
                        gameBoard[row, column + 2] = "B";
                        boatAdded = true;
                    }
                    else if (!isHorizontal && row + 2 < 10 && gameBoard[row, column] == " " && gameBoard[row + 1, column] == " " && gameBoard[row + 2, column] == " ")
                    {
                        gameBoard[row, column] = "B";
                        gameBoard[row, column] = "B";
                        gameBoard[row + 1, column] = "B";
                        gameBoard[row + 2, column] = "B";
                        boatAdded = true;
                    }
                }
            }
        }

        private static void DisplayBoard(string[,] gameBoard, string[,] aiGameBoard)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                YOUR BOAT'S                                          OPPS BOAT'S             ");
            Console.WriteLine("     1   2   3   4   5   6   7   8   9  10               1   2   3   4   5   6   7   8   9  10");

            for (int i = 0; i < 10; i++)
            {
                Console.Write($" {i + 1} |");

                for (int j = 0; j < 10; j++)
                {
                    Console.Write($" {gameBoard[i, j]} |");
                }

                Console.Write("        ");

                Console.Write($" {i + 1} |");

                for (int j = 0; j < 10; j++)
                {
                    Console.Write($" {aiGameBoard[i, j]} |");
                }

                Console.WriteLine();
            }
        }

        private static void TakeTurn(string[,] gameBoard, string[,] aiGameBoard, string[,] aiBoatBoard, ref string currentPlayer, ref int aiRowMove, ref int aiColumnMove, ref int yourRowMove, ref int yourColumnMove, ref Random random)
        {
            if (currentPlayer == "X")
            {
                Console.WriteLine("Your Turn");
                Console.WriteLine("Enter the row (1-10):");
                yourRowMove = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.WriteLine("Enter the column (1-10):");
                yourColumnMove = Convert.ToInt32(Console.ReadLine()) - 1;

                while (gameBoard[yourRowMove, yourColumnMove] != " ")
                {
                    Console.WriteLine("Invalid move. Try again.");
                    Console.WriteLine("Enter the row (1-10):");
                    yourRowMove = Convert.ToInt32(Console.ReadLine()) - 1;
                    Console.WriteLine("Enter the column (1-10):");
                    yourColumnMove = Convert.ToInt32(Console.ReadLine()) - 1;
                }

                if (aiBoatBoard[yourRowMove, yourColumnMove] == "B")
                {
                    Console.WriteLine("You hit an opponent's boat!");
                    aiGameBoard[yourRowMove, yourColumnMove] = "X";
                }
                else
                {
                    Console.WriteLine("You missed!");
                    aiGameBoard[yourRowMove, yourColumnMove] = "O";
                }

                currentPlayer = "O";
            }
            else
            {
                Console.WriteLine("AI's Turn");

                aiRowMove = random.Next(0, 10);
                aiColumnMove = random.Next(0, 10);

                while (gameBoard[aiRowMove, aiColumnMove] != " ")
                {
                    aiRowMove = random.Next(0, 10);
                    aiColumnMove = random.Next(0, 10);
                }

                if (gameBoard[aiRowMove, aiColumnMove] == "B")
                {
                    Console.WriteLine("AI hit your boat!");
                    gameBoard[aiRowMove, aiColumnMove] = "X";
                }
                else
                {
                    Console.WriteLine("AI missed!");
                    gameBoard[aiRowMove, aiColumnMove] = "O";
                }

                currentPlayer = "X";
            }
        }

        private static bool CheckForWin(string[,] gameBoard, string[,] aiGameBoard)
        {
            bool yourWin = CheckBoardForWin(gameBoard);
            bool aiWin = CheckBoardForWin(aiGameBoard);

            if (yourWin)
            {
                Console.WriteLine("Congratulations! You won!");
                return true;
            }
            else if (aiWin)
            {
                Console.WriteLine("Oops! AI won!");
                return true;
            }
            else if (IsBoardFull(gameBoard, aiGameBoard))
            {
                Console.WriteLine("It's a draw!");
                return true;
            }

            return false;
        }

        private static bool CheckBoardForWin(string[,] board)
        {
            // Check rows for win
            for (int i = 0; i < 10; i++)
            {
                if (board[i, 0] != " " && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 2] == board[i, 3])
                {
                    return true;
                }
            }

            // Check columns for win
            for (int i = 0; i < 10; i++)
            {
                if (board[0, i] != " " && board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[2, i] == board[3, i])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsBoardFull(string[,] gameBoard, string[,] aiGameBoard)
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (gameBoard[i, j] == " " || aiGameBoard[i, j] == " ")
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}


