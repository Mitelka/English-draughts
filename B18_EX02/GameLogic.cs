using System;
using System.Collections.Generic;

namespace B18_EX02
{
    internal class GameLogic
    {
        private Player[] m_Players;
        private byte m_BoardSize;
        private eGameType m_GameType;
        public Board m_GameBoard;

        public GameLogic(Player[] i_players, byte i_boardSize, eGameType i_gameType)
        {
            m_Players = i_players;
            m_BoardSize = i_boardSize;
            m_GameType = i_gameType;
            initializeBoard(m_BoardSize);
        }

        internal Board GameBoard { get => m_GameBoard; set => m_GameBoard = value; }

        private void initializeBoard(byte i_BoardSize)
        {
            m_GameBoard = new Board(i_BoardSize);
        }

        public void getComputerCellMove(int i_PlayerIndex, eSign playerSign, out Cell o_legalOriginCell, out Cell o_legalDesiredCell)
        {
            List<Player.PlayerMovelist> potentialMoveList = new List<Player.PlayerMovelist>();
            o_legalDesiredCell = null;
            o_legalOriginCell = null;

            foreach (Cell cell in m_GameBoard.m_PlayBoard)
            {
                if (cell.CellSign == playerSign)
                {
                    foreach (Cell matchCell in m_GameBoard.m_PlayBoard)
                    {
                        if (matchCell.CellSign == eSign.Empty)
                        {
                            if (AreCellsLegal(cell, matchCell, playerSign))
                            {
                                potentialMoveList.Add(new Player.PlayerMovelist() { originalCell = cell, desiredCell = matchCell});

                            }
                        }
                    }
                }                 
            }
            Random rndNumber = new Random();
            int playerMove = rndNumber.Next(0, potentialMoveList.Count);
            o_legalOriginCell = potentialMoveList[playerMove].originalCell;
            o_legalDesiredCell = potentialMoveList[playerMove].desiredCell;
        }

        public bool AreCellsLegal(Cell i_OriginCell, Cell i_DestCell, eSign i_PlayerSign)
        {
            bool moveIsLegal = true;
            //if (i_OriginCell.CellSign != i_PlayerSign)
            //{
               // moveIsLegal = false;
            //}
            
            if (m_GameBoard[i_DestCell].CellSign != eSign.Empty)
            {
                moveIsLegal = false;
            }
            else if (m_GameBoard[i_OriginCell].CellSign == eSign.Empty)
            {
                moveIsLegal = false;
            }
            else if (m_GameBoard[i_DestCell].CellRow > m_BoardSize || m_GameBoard[i_DestCell].CellCol > m_BoardSize || m_GameBoard[i_DestCell].CellRow < 0 || m_GameBoard[i_DestCell].CellCol < 0)
            {
                moveIsLegal = false;
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
                        //TODO
                        break;
                    case eSign.U:
                        //TODO
                        break;
                }
            }

            return moveIsLegal;
        }
        public bool IsPossibleToEat(Cell i_TheCellInTheMidlle)
        {
            return true; // Todo
        }

        public bool CheckingOplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLegal = true;
            if (i_OriginCell.CellRow >= i_DestCell.CellRow || i_OriginCell.CellCol == i_DestCell.CellCol)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow < i_DestCell.CellRow - 2 || i_DestCell.CellCol - 2 > i_OriginCell.CellCol && i_OriginCell.CellCol > i_DestCell.CellCol + 2)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol - 2)
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol + 1]))
                {
                    isLegal = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol + 2)
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol - 1]))
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

            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || i_DestCell.CellCol - 2 > i_OriginCell.CellCol && i_OriginCell.CellCol > i_DestCell.CellCol + 2)
            {
                isLegal = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && (i_OriginCell.CellCol == i_DestCell.CellCol - 2)) 
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol - 1]))
                {
                    isLegal = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && (i_OriginCell.CellCol == i_DestCell.CellCol +2))
            {
                if (!IsPossibleToEat(m_GameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol + 1]))
                {
                    isLegal = false;
                }
            }

            return isLegal;
        }

        public void MakeMoveOnBoard(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex)
        {
            m_GameBoard[i_OriginCell] = i_OriginCell;
            m_GameBoard[i_DestCell] = i_DestCell;
        }

    }
}