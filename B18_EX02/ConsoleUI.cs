namespace B18_EX02
{
    using System.Text;
    public class ConsoleUI
    {
        private const int maxCharInPlayerName = 20;
        private const byte numOfPlayers = 2;
        private static string FirstPlayerName = " ";
        private static string secondPlayerName = " ";
        private readonly Player[] players = new Player[numOfPlayers];
        private readonly GameLogic GameLogic;

        public ConsoleUI()
        {
            FirstPlayerName = getPlayerName();
            byte boardSize = getBoardSize();
            eGameType gameType = getGameType();
            players[0] = new Player(ePlayerType.Human, eSign.O, FirstPlayerName);
            if (gameType == eGameType.HumanVsHuman)
            {
                players[1] = new Player(ePlayerType.Human, eSign.X, secondPlayerName);
            }
            else
            {
                players[1] = new Player(ePlayerType.Computer, eSign.X, secondPlayerName);
            }

            GameLogic = new GameLogic(players, boardSize, gameType);
            printBoard();
        }

        private static string getPlayerName()
        {
            string name = " "; 
            bool nameToBigFlag = true;
            bool nameHasSpaceFlag = name.Contains(" ");

            System.Console.WriteLine("Please enter Player name:");

            while (nameToBigFlag || nameHasSpaceFlag)
            {
                name = System.Console.ReadLine();

                if (name.Length > maxCharInPlayerName)
                {
                    System.Console.WriteLine("Player Name is to big!! try again");               
                }

                if (name.Contains(" "))
                {
                    System.Console.WriteLine("Player Name Contains Space!! try again");
                }

                nameToBigFlag = false;
                nameHasSpaceFlag = false;
            }
 
            return name;
        }

        private static byte getBoardSize()
        {
            byte boardSize = 0;
            bool isVaildSizeFlag = false;

            System.Console.WriteLine("Please enter the board size - 6 or 8 or 10");

            while (!isVaildSizeFlag)
            {
                if (byte.TryParse(System.Console.ReadLine(), out boardSize))
                {
                    switch (boardSize)
                    {
                        case 6:
                            isVaildSizeFlag = true;
                            break;
                        case 8:
                            isVaildSizeFlag = true;
                            break;
                        case 10:
                            isVaildSizeFlag = true;
                            break;
                        default:
                            System.Console.WriteLine("Invalid board size. Please try again.");
                            break;
                    }    
                }
                else
                {
                    System.Console.WriteLine("Invalid input. Must by a number, and 6 or 8 or 10 only. Please try again.");
                }
            }

            return boardSize;
        }

        private static eGameType getGameType()
        {
            eGameType gameType = 0;
            bool validGameTypeFlag = false;
            System.Console.WriteLine(@"
Please enter the desired game type: 
'1' for HumanVsHuman 
'2' for HumanVsComputer");
            while (!validGameTypeFlag)
            {
                if (System.Enum.TryParse(System.Console.ReadLine(), out gameType) && System.Enum.IsDefined(typeof(eGameType), gameType))
                {
                    validGameTypeFlag = true;
                }
                else
                {
                    System.Console.WriteLine("Invalid game type. Please try again.");
                }
            }

            if (gameType == eGameType.HumanVsHuman)
            {
                secondPlayerName = getPlayerName();
            }
            else
            {
                secondPlayerName = "Computer";
            }

            return gameType;
        }

        private void printBoard()
        {
            StringBuilder boardStringBuilder;
            //TODO: add clear screen
            addColNumbersToString(out boardStringBuilder);
            printBoardRowSep(ref boardStringBuilder);
            for (byte currentRow = 0; currentRow < GameLogic.Board.BoardSize; currentRow++)
            {
                boardStringBuilder.Append($"{(char)('a' + currentRow)}");
                boardStringBuilder.Append("|");
                for (byte currentCol = 0; currentCol < GameLogic.Board.BoardSize; currentCol++)
                {
                    char signToPrint = getSignToPrint(GameLogic.Board[currentRow, currentCol].CellSign);
                    boardStringBuilder.Append($" {signToPrint} |");
                }
                printBoardRowSep(ref boardStringBuilder);
            }
            System.Console.WriteLine(boardStringBuilder);
        }

        private void printBoardRowSep(ref StringBuilder io_BoardStringBuilder)
        {
            io_BoardStringBuilder.AppendLine();
            io_BoardStringBuilder.Append(" ");
            for (byte currentCol = 0; currentCol < GameLogic.Board.BoardSize; currentCol++)
            {
                io_BoardStringBuilder.Append("====");
            }
            io_BoardStringBuilder.Append("=");
            io_BoardStringBuilder.AppendLine();
        }

        private char getSignToPrint(eSign i_CellSign)
        {
            char signChar;
            switch(i_CellSign)
            {
                case eSign.Empty:
                    signChar = ' ';
                    break;
                case eSign.O:
                    signChar = 'O';
                    break;
                case eSign.X:
                    signChar = 'X';
                    break;
                case eSign.U:
                    signChar = 'U';
                    break;
                case eSign.K:
                    signChar = 'K';
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(i_CellSign), i_CellSign, null);
            }
            return signChar;
        }

        private void addColNumbersToString(out StringBuilder o_StringBuilder)
        {
            o_StringBuilder = new StringBuilder();
            o_StringBuilder.Append(' ', 3);
            for (byte currentCol = 0; currentCol < GameLogic.Board.BoardSize; currentCol++)
            {
                o_StringBuilder.Append($"{(char)('A' + currentCol)}");
                o_StringBuilder.Append(' ', 3);
            }
        }
    }
}