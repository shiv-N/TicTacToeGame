using System;

namespace TicTacToeGame
{
    class Program
    {
        public const int HEAD = 0;
        public const int TAIL = 1;
        public enum Player { USER, COMPUTER };
        public enum GameStatus { WON, FULL_BOARD, CONTINUE };

        static void Main(string[] args)
        {
            while (true)
            {
                char[] board = createBoard();
                char userLetter = chooseUserLetter();
                char computerLetter = (userLetter == 'X') ? 'O' : 'X';
                Player player = getWhoStartsFirst();
                bool gameIsPlaying = true;
                GameStatus gameStatus;
                while (gameIsPlaying)
                {
                    // Players Turn
                    if (player.Equals(Player.USER))
                    {
                        showBoard(board);
                        int userMove = getUserMove(board);
                        String wonMessage = "Hooray! You have won the game!";
                        gameStatus = getGameStatus(board, userMove, userLetter, wonMessage);
                        player = Player.COMPUTER;
                    }
                    else
                    {
                        // Computer Turn
                        String wonMessage = "The computer has beaten you! You lose.";
                        int computerMove = getComputerMove(board, computerLetter, userLetter);
                        gameStatus = getGameStatus(board, computerMove, computerLetter, wonMessage);
                        player = Player.USER;
                    }
                    if (gameStatus.Equals(GameStatus.CONTINUE)) continue;
                    gameIsPlaying = false;
                }
                if (!playAgain()) break;
            }
        }

        /* UC 1 */
        private static char[] createBoard()
        {
            char[] board = new char[10];
            for (int i = 0; i < board.Length; i++)
            {
                board[i] = ' ';
            }
            return board;
        }

        /* UC 2 */
        private static char chooseUserLetter()
        {
            Console.WriteLine("Choose your Letter: ");
            string userLetter = Console.ReadLine();
            return char.ToUpper(userLetter[0]);
        }

        /* UC 3 */
        private static Player getWhoStartsFirst()
        {
            int toss = getOneFromRandomChoices(2);
            return (toss == HEAD) ? Player.USER : Player.COMPUTER;
        }

        private static int getOneFromRandomChoices(int choices)
        {
            Random random = new Random();
            return (int)(random.Next() * 10) % choices;
        }

        /* UC 4 */
        private static void showBoard(char[] board)
        {
            Console.WriteLine("\n " + board[1] + " | " + board[2] + " | " + board[3]);
            Console.WriteLine("-----------");
            Console.WriteLine(" " + board[4] + " | " + board[5] + " | " + board[6]);
            Console.WriteLine("-----------");
            Console.WriteLine(" " + board[7] + " | " + board[8] + " | " + board[9]);
        }

        /* UC 5 */
        private static int getUserMove(char[] board)
        {
            int[] validCells = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            while (true)
            {
                Console.WriteLine("What is your next move? (1-9): ");
                int index = Convert.ToInt32(Console.ReadLine());
                if (Array.Find<int>(validCells,element => element == index)!= 0 && isSpaceFree(board, index))
                    return index;
            }
        }

        private static bool isSpaceFree(char[] board, int index)
        {
            return board[index] == ' ';
        }

        /* UC 6 */
        private static void makeMove(char[] board, int index, char letter)
        {
            bool spaceFree = isSpaceFree(board, index);
            if (spaceFree) board[index] = letter;
        }

        /* UC 7 */
        private static bool isWinner(char[] b, char ch)
        {
            return ((b[1] == ch && b[2] == ch && b[3] == ch) || // across the top
                     (b[4] == ch && b[5] == ch && b[6] == ch) || // across the middle
                     (b[7] == ch && b[8] == ch && b[9] == ch) || // across the top
                     (b[1] == ch && b[4] == ch && b[7] == ch) || // across the left
                     (b[2] == ch && b[5] == ch && b[8] == ch) || // across the middle
                     (b[3] == ch && b[6] == ch && b[9] == ch) || // across the right
                     (b[1] == ch && b[5] == ch && b[9] == ch) || // across the top left diagonal
                     (b[7] == ch && b[5] == ch && b[3] == ch));  // across the bottom left diagonal
        }

        private static bool isBoardFull(char[] board)
        {
            for (int index = 1; index < board.Length; index++)
            {
                if (isSpaceFree(board, index)) return false;
            }
            return true;
        }



        /* UC 8, 9, 10 & 11 */
        private static int getComputerMove(char[] board, char computerLetter, char userLetter)
        {
            int winningMove = getWinningMove(board, computerLetter);
            if (winningMove != 0) return winningMove;
            int userWinningMove = getWinningMove(board, userLetter);
            if (userWinningMove != 0) return userWinningMove;
            int[] cornorMoves = { 1, 3, 7, 9 };
            int computerMove = getRandomMoveFromList(board, cornorMoves);
            if (computerMove != 0) return computerMove;
            if (isSpaceFree(board, 5)) return 5; // Center Move
            int[] sideMoves = { 2, 4, 6, 8 };
            computerMove = getRandomMoveFromList(board, sideMoves);
            if (computerMove != 0) return computerMove;
            return 0;
        }

        /* UC 12 */
        private static GameStatus getGameStatus(char[] board, int move, char letter,
                                                String wonMessage)
        {
            makeMove(board, move, letter);
            if (isWinner(board, letter))
            {
                showBoard(board);
                Console.WriteLine(wonMessage);
                return GameStatus.WON;
            }
            if (isBoardFull(board))
            {
                showBoard(board);
                Console.WriteLine("Game is Tie");
                return GameStatus.FULL_BOARD;
            }
            return GameStatus.CONTINUE;
        }

        /* UC 8 */
        private static int getWinningMove(char[] board, char letter)
        {
            for (int index = 1; index < board.Length; index++)
            {
                char[] copyOfBoard = getCopyOfBoard(board);
                if (isSpaceFree(copyOfBoard, index))
                {
                    makeMove(copyOfBoard, index, letter);
                    if (isWinner(copyOfBoard, letter))
                        return index;
                }
            }
            return 0;
        }

        private static char[] getCopyOfBoard(char[] board)
        {
            char[] boardCopy = new char[10];
            Array.ConstrainedCopy(board, 0, boardCopy, 0, board.Length);
            return boardCopy;
        }

        /* UC 10 */
        private static int getRandomMoveFromList(char[] board, int[] moves)
        {
            for (int index = 0; index < moves.Length; index++)
            {
                if (isSpaceFree(board, moves[index])) return moves[index];
            }
            return 0;
        }

        /* UC 13 */
        private static bool playAgain()
        {
            Console.WriteLine("Do you want to play again? (yes or no) ");
            string option = Console.ReadLine().ToLower();
            if (option.Equals("yes")) return true;
            return false;
        }
    }
}
