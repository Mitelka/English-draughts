using System;
using System.Collections.Generic;

namespace B18_EX02
{
    internal class GameLogic
    {
        private Player[] m_Players;
        private byte m_BoardSize;
        private eGameType m_GameType;
        private eGameResult m_GameResult;
        public Board m_GameBoard;


        public GameLogic(Player[] i_players, byte i_boardSize, eGameType i_gameType)
        {
            m_Players = i_players;
            m_BoardSize = i_boardSize;
            m_GameType = i_gameType;
            initializeBoard();
            initializeTokens();
        }

        internal Board GameBoard { get => m_GameBoard; set => m_GameBoard = value; }

        public eGameResult GameResult { get => m_GameResult; }

        private void initializeBoard()
        {
            m_GameBoard = new Board(m_BoardSize);
        }

        private void initializeTokens()
        {
            int numberOfTokens = ((m_BoardSize - 2) / 2) * (m_BoardSize / 2);
            for (int playerIndex = 0; playerIndex < m_Players.Length; playerIndex++)
            {
                m_Players[playerIndex].NumOfTokens = numberOfTokens;
            }
        }

        public void GetAllOptionalCellMove(int i_PlayerIndex, eSign playerSign, ref bool o_DidEat)
        {
            m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Clear();
            foreach (Cell cell in m_GameBoard.m_PlayBoard)
            {
                if (cell.CellSign == playerSign)
                {
                    foreach (Cell matchCell in m_GameBoard.m_PlayBoard)
                    {
                        if (matchCell.CellSign == eSign.Empty)
                        {
                            if (AreCellsLegal(cell, matchCell, playerSign, ref o_DidEat))
                            {
                                m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Add(new Player.PlayerMovelist() { originalCell = cell, desiredCell = matchCell });
                            }
                        }
                    }
                }
            }
            GetAllEatingOptionalCellMove(m_Players[i_PlayerIndex].PlayerPotentialMoveslist, i_PlayerIndex);
            
  
        }
        public void GetAllEatingOptionalCellMove(List<Player.PlayerMovelist> m_PlayerEatingOptionlist, int i_PlayerIndex)
        {
            m_Players[i_PlayerIndex].PlayerPotentialEatinglist.Clear();
            foreach (Player.PlayerMovelist optionaEatingMove in m_Players[i_PlayerIndex].PlayerPotentialMoveslist)
            {
                if (Math.Abs(optionaEatingMove.originalCell.CellRow - optionaEatingMove.desiredCell.CellRow) == 2)
                {
                    m_Players[i_PlayerIndex].PlayerPotentialEatinglist.Add(new Player.PlayerMovelist() { originalCell = optionaEatingMove.originalCell, desiredCell = optionaEatingMove.desiredCell });
                }
            }
            if (m_Players[i_PlayerIndex].PlayerPotentialEatinglist.Count != 0)
            {
                m_Players[i_PlayerIndex].PlayerPotentialMoveslist = m_Players[i_PlayerIndex].PlayerPotentialEatinglist;
            }
        }

        public bool AreCellsLegal(Cell i_OriginCell, Cell i_DestCell, eSign i_PlayerSign, ref bool o_DidEatFlag)
        {
            bool moveIsLegal = true;
            if (m_GameBoard[i_OriginCell].CellSign != i_PlayerSign || m_GameBoard[i_DestCell].CellSign != eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_DestCell].CellSign != eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_OriginCell].CellSign == eSign.Empty)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (m_GameBoard[i_DestCell].CellRow > m_BoardSize || m_GameBoard[i_DestCell].CellCol > m_BoardSize || m_GameBoard[i_DestCell].CellRow < 0 || m_GameBoard[i_DestCell].CellCol < 0)
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else if (Math.Abs(m_GameBoard[i_DestCell].CellRow - m_GameBoard[i_OriginCell].CellRow) != Math.Abs(m_GameBoard[i_DestCell].CellCol - m_GameBoard[i_OriginCell].CellCol))
            {
                moveIsLegal = false;
                o_DidEatFlag = false;
            }
            else
            {
                switch (m_GameBoard[i_OriginCell].CellSign)
                {
                    case eSign.O:
                        moveIsLegal = CheckingOplayerCellsLegal(i_OriginCell, i_DestCell);
                        break;

                    case eSign.X:
                        moveIsLegal = CheckingXplayerCellsLegal(i_OriginCell, i_DestCell);
                        break;
                    case eSign.K:
                        // TODO
                        break;
                    case eSign.U:
                        // TODO
                        break;
                }
            }

            return moveIsLegal;
        }

        public bool IsPossibleToEat(Cell i_TheCellInTheMiddle, Cell i_OriginCell)
        {

            if (m_GameBoard[i_TheCellInTheMiddle].CellSign != eSign.Empty && m_GameBoard[i_TheCellInTheMiddle].CellSign != m_GameBoard[i_OriginCell].CellSign)
            {
                return true;
            }

            return false;
        }

        public bool CheckingOplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLegal = true;
            if (i_OriginCell.CellRow >= i_DestCell.CellRow || i_OriginCell.CellCol == i_DestCell.CellCol)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow < i_DestCell.CellRow - 2 || (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) > 2))
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol - 2)
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol + 1], i_OriginCell))
                {
                    isLegal = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol + 2)
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol - 1], i_OriginCell))
                {
                    isLegal = false;
                }
             
            }

            return isLegal;
        }

        public bool CheckingXplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLegal = true;

            if (i_OriginCell.CellRow <= i_DestCell.CellRow || i_OriginCell.CellCol == i_DestCell.CellCol)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || (i_DestCell.CellCol - 2 > i_OriginCell.CellCol && i_OriginCell.CellCol > i_DestCell.CellCol + 2))
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow + 2 && (i_OriginCell.CellCol == i_DestCell.CellCol - 2))
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol + 1], i_OriginCell))
                {
                    isLegal = false;
                }
           
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow + 2 && (i_OriginCell.CellCol == i_DestCell.CellCol + 2))
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol - 1], i_OriginCell))
                {
                    isLegal = false;
                }
      
            }
            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || (Math.Abs(i_DestCell.CellCol - i_OriginCell.CellCol) > 2))
            {
                isLegal = false;
            }

            return isLegal;
        }

        public void MakeMoveOnBoard(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex, out bool o_IsKingFlag, out bool o_DidEat)
        {
            o_DidEat = false;
            eSign kingSign;
            o_IsKingFlag = false;
            if (checkIfKing(i_DestCell, i_PlayerIndex, out kingSign))
            {
                o_IsKingFlag = true;
                i_DestCell.CellSign = kingSign;
            }

            if (Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow) == 2)
            {
                if (m_Players[i_PlayerIndex].Sign == eSign.O)
                {
                    m_GameBoard[(byte)Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow), (byte)Math.Abs(i_OriginCell.CellCol - i_DestCell.CellCol + 1)].CellSign = eSign.Empty;
                }
                else if (m_Players[i_PlayerIndex].Sign == eSign.X)
                {
                    m_GameBoard[(byte)Math.Abs(i_OriginCell.CellRow - i_DestCell.CellRow + 1), (byte)Math.Abs(i_OriginCell.CellCol - i_DestCell.CellCol) ].CellSign = eSign.Empty;
                }

                o_DidEat = true;
            }
           
            m_GameBoard[i_OriginCell] = i_OriginCell;
            m_GameBoard[i_DestCell] = i_DestCell;
        }

        public void UpdatePlayerTokens(int i_PlayerIndex, bool i_DidEat, bool i_IsKing)
        {
            if (i_DidEat)
            {
                m_Players[i_PlayerIndex].NumOfTokens += 1;
                m_Players[GetOtherPlayerIndex(i_PlayerIndex)].NumOfTokens -= 1;
            }
            if (i_IsKing)
            {
                m_Players[i_PlayerIndex].NumOfTokens += 4;
            }
        }

        public int GetOtherPlayerIndex(int i_CurrPlayerIndex)
        {
            return (i_CurrPlayerIndex + 1) % (m_Players.Length);
        }

        public void UpdateWinnerScore(int i_WinnerPlayerIdx)
        {
            int otherPlayerIdx = GetOtherPlayerIndex(i_WinnerPlayerIdx);
            m_Players[i_WinnerPlayerIdx].Score = m_Players[i_WinnerPlayerIdx].NumOfTokens - m_Players[otherPlayerIdx].NumOfTokens;
        }

        public int GetWinnerOfAllGamesIndex()
        {
            int winnerIndex = 0;
            int maxScore = 0;
            for (int playerIndex = 0; playerIndex < m_Players.Length; playerIndex++)
            {
                if (m_Players[playerIndex].Score > maxScore)
                {
                    winnerIndex = playerIndex;
                }
            }

            return winnerIndex;
        }

        private bool checkIfKing(Cell i_DestCell, int i_PlayerIndex, out eSign o_Sign)
        {
            bool isKingFlag = false;
            o_Sign = eSign.Empty;
            if ((m_Players[i_PlayerIndex].Sign == eSign.O) && (i_DestCell.CellRow == m_BoardSize - 1))
            {
                o_Sign = eSign.U;
                isKingFlag = true;
            }
            else if ((m_Players[i_PlayerIndex].Sign == eSign.X) && (i_DestCell.CellRow == 0))
            {
                o_Sign = eSign.K;
                isKingFlag = true;
            }

            return isKingFlag;
        }

        public bool CheckIfGameOver(Cell i_RequestedCell, int i_PlayerIndex)
        {
            bool didEat = false;
            bool gameOverFlag = false;
            int otherPlayerIndex = GetOtherPlayerIndex(i_PlayerIndex);
            GetAllOptionalCellMove(otherPlayerIndex, m_Players[otherPlayerIndex].Sign, ref didEat);
            GetAllOptionalCellMove(i_PlayerIndex, m_Players[i_PlayerIndex].Sign, ref didEat);

            if (m_Players[otherPlayerIndex].NumOfTokens == 0)
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.WINNER;
            }
            else if ((m_Players[otherPlayerIndex].PlayerPotentialMoveslist.Count == 0) && (m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Count == 0))
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.TIE;
            }
            else if ((m_Players[otherPlayerIndex].PlayerPotentialMoveslist.Count == 0))
            {
                gameOverFlag = true;
                m_GameResult = eGameResult.WINNER;
            }

            return gameOverFlag;
        }

        public void SetComputerMove(int i_PlayerIndex, out Cell o_legalOriginCell, out Cell o_legalDesiredCell ,ref bool o_Dideat)
        {
            Random rndNumber = new Random();
            int playerMove = rndNumber.Next(0, m_Players[i_PlayerIndex].PlayerPotentialMoveslist.Count);
            o_legalOriginCell = m_Players[i_PlayerIndex].PlayerPotentialMoveslist[playerMove].originalCell;
            o_legalDesiredCell = m_Players[i_PlayerIndex].PlayerPotentialMoveslist[playerMove].desiredCell;
        }

        public bool CheckIfCellsInThePossibleList(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex)
        {
            bool isLegal = false;
            foreach (Player.PlayerMovelist checkingCells in m_Players[i_PlayerIndex].PlayerPotentialMoveslist)
            {
                if (checkingCells.originalCell.CellCol == i_OriginCell.CellCol && checkingCells.originalCell.CellRow == i_OriginCell.CellRow)
                {
                    isLegal = true;
                    break;
                }
            }

            return isLegal;
        }

        public bool IsPlayerEligToQuit(int i_PlayerIndex)
        {
            bool isEligToQuit = false;
            int otherPlayerIdx = GetOtherPlayerIndex(i_PlayerIndex);
            int playerCurrScore;
            int otherPlayerCurrScore;

            playerCurrScore = m_Players[i_PlayerIndex].Score + m_Players[i_PlayerIndex].NumOfTokens;
            otherPlayerCurrScore = m_Players[otherPlayerIdx].Score + m_Players[otherPlayerIdx].NumOfTokens;
            if(playerCurrScore < otherPlayerCurrScore)
            {
                isEligToQuit = true;
            }

            return isEligToQuit;
        }
    }
}