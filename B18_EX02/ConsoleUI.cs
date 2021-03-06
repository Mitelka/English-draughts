﻿namespace B18_EX02
{
    using System.Text;

    public class ConsoleUI
    {
        private const int k_MaxCharInPlayerName = 20;
        private const byte k_NumOfPlayers = 2;
        private static string s_FirstPlayerName = " ";
        private static string s_SecondPlayerName = " ";
        private static string s_PrevStep = string.Empty;
        private readonly GameLogic m_GameLogic;
        private readonly Player[] m_Players = new Player[k_NumOfPlayers];
        private bool m_IsGameOver = false;
        
        public ConsoleUI()
        {
            s_FirstPlayerName = getPlayerName();
            byte boardSize = getBoardSize();
            eGameType gameType = getGameType();
            m_Players[0] = new Player(ePlayerType.Human, eSign.O, s_FirstPlayerName);
            if (gameType == eGameType.HumanVsHuman)
            {
                m_Players[1] = new Player(ePlayerType.Human, eSign.X, s_SecondPlayerName);
            }
            else
            {
                m_Players[1] = new Player(ePlayerType.Computer, eSign.X, s_SecondPlayerName);
            }

            m_GameLogic = new GameLogic(m_Players, boardSize, gameType);
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

                if (name.Length > k_MaxCharInPlayerName)
                {
                    System.Console.WriteLine("Player Name is to big!! try again");               
                }
                else if (name.Contains(" "))
                {
                    System.Console.WriteLine("Player Name Contains Space!! try again");
                }
                else
                {
                    nameToBigFlag = false;
                    nameHasSpaceFlag = false;
                }
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
                    System.Console.WriteLine("Invalid input. Must be a number, and 6 or 8 or 10 only. Please try again.");
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
                s_SecondPlayerName = getPlayerName();
            }
            else
            {
                s_SecondPlayerName = "Computer";
            }

            return gameType;
        }

        private void printBoard()
        {
            StringBuilder boardStringBuilder;
            Ex02.ConsoleUtils.Screen.Clear();
            addColLettersToString(out boardStringBuilder);
            printBoardRowSep(ref boardStringBuilder);
            for (byte currentRow = 0; currentRow < m_GameLogic.GameBoard.BoardSize; currentRow++)
            {
                boardStringBuilder.Append($"{(char)('a' + currentRow)}");
                boardStringBuilder.Append("|");
                for (byte currentCol = 0; currentCol < m_GameLogic.GameBoard.BoardSize; currentCol++)
                {
                    char signToPrint = getSignToPrint(m_GameLogic.GameBoard[currentRow, currentCol].CellSign);

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
            for (byte currentCol = 0; currentCol < m_GameLogic.GameBoard.BoardSize; currentCol++)
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
            for (byte currentCol = 0; currentCol < m_GameLogic.GameBoard.BoardSize; currentCol++)
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
            Cell originCell;
            Cell destCell;
            bool didEat = false;
            bool quitRequest = false;
            bool playerHasAnotherMove = false;
            bool isKing;

            m_GameLogic.UpdateAllOptionalCellMove(0, m_Players[0].Sign, ref didEat);
            while (!m_IsGameOver && !quitRequest)             
            {
                for (byte playerIndex = 0; playerIndex < m_Players.Length && !(m_IsGameOver || quitRequest); playerIndex++)
                {                   
                    getLegalDesiredCell(playerIndex, ref quitRequest, out originCell, out destCell, ref didEat, playerHasAnotherMove);
                    if(quitRequest)
                    {
                        int otherPlayerIdx = m_GameLogic.GetOtherPlayerIndex(playerIndex);
                        m_GameLogic.UpdatePlayersScore(otherPlayerIdx);
                        showResults(otherPlayerIdx);
                        break;
                    }

                    m_GameLogic.MakeMoveOnBoard(originCell, destCell, playerIndex, out isKing, out didEat);
                    m_GameLogic.UpdatePlayerTokens(playerIndex, didEat, isKing);
                    printBoard();
                    m_IsGameOver = m_GameLogic.CheckIfGameOver(destCell, playerIndex);

                    if (m_IsGameOver)
                    {
                        if(m_GameLogic.GameResult == eGameResult.WINNER)
                        {
                            m_GameLogic.UpdatePlayersScore(playerIndex);
                        }

                        showResults(m_GameLogic.GetWinnerOfAllGamesIndex());
                    }
                    else if (didEat)
                    {
                        if (m_GameLogic.CheckDoubleEatingMove(destCell, playerIndex))
                        {
                            playerIndex--;
                            playerHasAnotherMove = true;
                        }
                        else
                        {
                            playerHasAnotherMove = false;
                        }
                    }
                }
            }
            return checkIfUserContinueToAnotherRound();
        }

        private void resetRound()
        {
            m_GameLogic.GameBoard.ResetBoard();
            m_GameLogic.InitializeTokens();
            m_IsGameOver = false;
            s_PrevStep = string.Empty;
        }

        private void getLegalDesiredCell(int i_PlayerIndex, ref bool io_QuitRequest, out Cell o_LegalOriginCell, out Cell o_LegalDestCell, ref bool o_Dideat, bool playerHasAnotherMove)
        {
            o_LegalOriginCell = null;
            o_LegalDestCell = null;
            StringBuilder compStepStr = new StringBuilder();
            eSign playerSign = m_Players[i_PlayerIndex].Sign;
            ePlayerType playerType = m_Players[i_PlayerIndex].PlayerType;

            if(playerType == ePlayerType.Human)
            {
                getUserCellMove(i_PlayerIndex, playerSign, ref io_QuitRequest, out o_LegalOriginCell, out o_LegalDestCell, ref o_Dideat, playerHasAnotherMove);
            }
            else
            {
                m_GameLogic.SetComputerMove(i_PlayerIndex, out o_LegalOriginCell, out o_LegalDestCell, ref o_Dideat);
                s_PrevStep = compStepStr.Append(o_LegalOriginCell.GetCellStr()).Append(">").Append(o_LegalDestCell.GetCellStr()).ToString();
            }
        }

        private void getUserCellMove(int i_PlayerIndex, eSign i_PlayerSign, ref bool io_QuitRequest, out Cell o_LegalOriginCell, out Cell o_LegalDestCell, ref bool o_DidEat, bool playerHasAnotherMove)
        {
            bool isLegalMove = false;
            o_LegalOriginCell = null;
            o_LegalDestCell = null;
            int otherPlayerIndex;
            if (playerHasAnotherMove)
            {
                int tempSaverForCurrentPlayerIndex = i_PlayerIndex + 1;
                otherPlayerIndex = m_GameLogic.GetOtherPlayerIndex(tempSaverForCurrentPlayerIndex);
            }
            else
            {
                otherPlayerIndex = m_GameLogic.GetOtherPlayerIndex(i_PlayerIndex);
            }

            if (s_PrevStep != string.Empty)
            {
                System.Console.WriteLine($@"{m_Players[otherPlayerIndex].PlayerName}'s move was ({m_Players[otherPlayerIndex].Sign}): {s_PrevStep}");
            }

            System.Console.WriteLine($@"{m_Players[i_PlayerIndex].PlayerName}'s Turn ({m_Players[i_PlayerIndex].Sign}):
Enter your desirable coordinate as follows: PrevColPrevRow > ColRow");

            while(!isLegalMove)
            {
                string userInput = getUserInput(ref io_QuitRequest);
                string[] splitInput = userInput.Split('>');
                if (io_QuitRequest)
                {
                    if (m_GameLogic.IsPlayerEligToQuit(i_PlayerIndex))
                    {
                        System.Console.WriteLine("You chose to quite.");
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid input, please try again.");
                    }
                }

                if (splitInput.Length != 2)
                {
                    System.Console.WriteLine("Invalid input, please try again.");
                    continue;
                }

                if(!m_IsGameOver)
                {
                    if(Cell.Parse(splitInput[0], out o_LegalOriginCell) && Cell.Parse(splitInput[1], out o_LegalDestCell) && m_GameLogic.AreCellsLegal(o_LegalOriginCell, o_LegalDestCell, i_PlayerSign, ref o_DidEat))
                    {
                        if (m_GameLogic.CheckIfCellsInThePossibleList(o_LegalOriginCell, o_LegalDestCell, i_PlayerIndex))
                        {                          
                            s_PrevStep = userInput;
                            isLegalMove = true;
                        }
                        else
                        {
                            invalidInputMessage(splitInput);
                        }
                    }
                    else
                    {
                        invalidInputMessage(splitInput);
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
                io_QuitRequest = true;
            }

            return userInput;
        }

        private void showResults(int i_WinnerIndex)
        {
            StringBuilder strToPrint = new StringBuilder();
            strToPrint.Append($"The winner of this game is: {m_Players[i_WinnerIndex].PlayerName} ({m_Players[i_WinnerIndex].Sign})");
            strToPrint.Append($" with the score of: {m_Players[i_WinnerIndex].Score}");

            System.Console.WriteLine(strToPrint);
        }

        private bool checkIfUserContinueToAnotherRound()
        {
            bool continuePlayingFlag = true;

            System.Console.WriteLine("Press 'Q' to quit or any other key to continue playing");
            string userInput = System.Console.ReadLine();
            if(userInput.ToUpper() == "Q")
            {
                continuePlayingFlag = false;
            }

            return continuePlayingFlag;
        }

        private void invalidInputMessage(string[] splitInput)
        {
            System.Console.WriteLine("Invalid input, please try again.");
            System.Array.Clear(splitInput, 0, 1);
        }
    }
}