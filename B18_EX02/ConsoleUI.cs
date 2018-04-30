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
        private bool isGameOver = false;
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
            addColLettersToString(out boardStringBuilder);
            printBoardRowSep(ref boardStringBuilder);
            for (byte currentRow = 0; currentRow < GameLogic.gameBoard.BoardSize; currentRow++)
            {
                boardStringBuilder.Append($"{(char)('a' + currentRow)}");
                boardStringBuilder.Append("|");
                for (byte currentCol = 0; currentCol < GameLogic.gameBoard.BoardSize; currentCol++)
                {
                    char signToPrint = getSignToPrint(GameLogic.gameBoard[currentRow, currentCol].CellSign);
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
            for (byte currentCol = 0; currentCol < GameLogic.gameBoard.BoardSize; currentCol++)
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

        private void addColLettersToString(out StringBuilder o_StringBuilder)
        {
            o_StringBuilder = new StringBuilder();
            o_StringBuilder.Append(' ', 3);
            for (byte currentCol = 0; currentCol < GameLogic.gameBoard.BoardSize; currentCol++)
            {
                o_StringBuilder.Append($"{(char)('A' + currentCol)}");
                o_StringBuilder.Append(' ', 3);
            }
        }

        public void StartGame()
        {
            bool continueGameRoundsFlag = true;

            while(continueGameRoundsFlag)
            {
                printBoard();
                continueGameRoundsFlag = runRound();
                if(continueGameRoundsFlag)
                {
                    resetRound();
                }
            }
        }

        private bool runRound()
        {
            bool quitRequest = false;
            while(!isGameOver && !quitRequest)
            {
                for (byte playerIndex = 0; playerIndex < players.Length; playerIndex++)
                {
                    Cell requestedMoveCell = getLegalDesiredCell(playerIndex, ref quitRequest);
                    if(quitRequest)
                    {
                        break;
                    }
                    //GameLogic.MakeMoveOnBoard(requestedMoveCell, playerIndex);
                    printBoard();
                    //isGameOver = GameLogic.checkIfGameOver(requestedMoveCell, playerIndex);
                    if(isGameOver)
                    {
                        //showResults();
                    }
                }
            }
            return true;
        }

        private void resetRound()
        {
            GameLogic.gameBoard.ResetBoard();
        }

        private Cell getLegalDesiredCell(int i_PlayerIndex, ref bool io_QuitRequest)
        {
            Cell legalDesiredCell;
            Cell legalOriginCell;
            eSign playerSign = players[i_PlayerIndex].Sign;
            ePlayerType playerType = players[i_PlayerIndex].PlayerType;

            if(playerType == ePlayerType.Human)
            {
                getUserCellMove(i_PlayerIndex, playerSign, ref io_QuitRequest, out legalOriginCell, out legalDesiredCell);
            }
            else
            {
                GameLogic.getComputerCellMove(i_PlayerIndex, playerSign, out legalOriginCell, out legalDesiredCell);    
            }

            return legalDesiredCell;
        }

        private void getUserCellMove(int i_PlayerIndex, eSign i_PlayerSign, ref bool io_QuitRequest, out Cell o_LegalOriginCell, out Cell o_LegalDestCell)
        {
            bool isLegalMove = false;
            o_LegalOriginCell = null;
            o_LegalDestCell = null;
            //TODO: in the example the previous step is written, should we do it as well?
            System.Console.WriteLine($@"{players[i_PlayerIndex].PlayerName}'s ({players[i_PlayerIndex].Sign}) turn:
Enter your desirable coordinate as follows: PrevRowPrevCol > RowCol");

            while(!isLegalMove)
            {
                string userInput = getUserInput(ref io_QuitRequest);
                string[] splitInput = userInput.Split('>');
                if(splitInput.Length != 2)
                {
                    continue;
                }
                if(!isGameOver)
                {
                    if(Cell.Parse(splitInput[0], out o_LegalOriginCell) && Cell.Parse(splitInput[1], out o_LegalDestCell) && GameLogic.areCellsLegal(o_LegalOriginCell, o_LegalDestCell))
                    {
                        o_LegalDestCell.CellSign = i_PlayerSign;
                        o_LegalOriginCell.CellSign = eSign.Empty;
                        isLegalMove = true;
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid input, please try again.");   
                    }
                }
                else
                {
                    break;
                }
            }
        }

        private string getUserInput(ref bool io_QuitRequest)
        {
            string userInput = System.Console.ReadLine();
            if(userInput.ToUpper() == "Q")
            {
                System.Console.WriteLine("You chose to quite.");
                io_QuitRequest = true;
            }

            return userInput;
        }
    }
}