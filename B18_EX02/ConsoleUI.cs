namespace B18_EX02
{
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
            if (gameType == eGameType.HumenVsHumen)
            {
                players[1] = new Player(ePlayerType.Human, eSign.X, secondPlayerName);
            }
            else
            {
                players[1] = new Player(ePlayerType.Computer, eSign.X, secondPlayerName);
            }

            GameLogic = new GameLogic(players, boardSize, gameType);
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
'1' for HumenVsHumen 
'2' for HumenVsComputer");
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

            if (gameType == eGameType.HumenVsHumen)
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
            byte size = GameLogic.BoardSize;

            // TODO
        }
    }
}