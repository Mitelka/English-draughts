using System;
using System.Collections.Generic;

namespace B18_EX02
{
    internal class GameLogic
    {
        private Player[] players;
        private byte boardSize;
        private eGameType gameType;
        public Board gameBoard;

        public GameLogic(Player[] i_players, byte i_boardSize, eGameType i_gameType)
        {
            players = i_players;
            boardSize = i_boardSize;
            gameType = i_gameType;
            initializeBoard(boardSize);
        }

        internal Board GameBoard { get; set; }

        private void initializeBoard(byte i_BoardSize)
        {
            gameBoard = new Board(i_BoardSize);
        }

        public void getComputerCellMove(int i_PlayerIndex, eSign playerSign, out Cell o_legalOriginCell, out Cell o_legalDesiredCell)
        {
            List<Player.PlayerMovelist> potintalMoveList = new List<Player.PlayerMovelist>();
            o_legalDesiredCell = null;
            o_legalOriginCell = null;

            foreach (Cell cell in gameBoard.m_PlayBoard)
            {
                if (cell.CellSign == playerSign)
                {
                    foreach (Cell matchCell in gameBoard.m_PlayBoard)
                    {
                        if (matchCell.CellSign == eSign.Empty)
                        {
                            if (areCellsLegal(cell, matchCell))
                            {
                                potintalMoveList.Add(new Player.PlayerMovelist() { originalCell = cell, desiredCell = matchCell});

                            }

                        }
                    }
                    
                }                 

            }
            Random rndNumber = new Random();
            int playerMove = rndNumber.Next(0, potintalMoveList.Count);
            o_legalOriginCell = potintalMoveList[playerMove].originalCell;
            o_legalDesiredCell = potintalMoveList[playerMove].desiredCell;
        }

        public bool areCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool moveIsLeagel = true;
            if (i_DestCell.CellSign != eSign.Empty)
            {
                moveIsLeagel = false;
               
            }
            else if (i_OriginCell.CellSign == eSign.Empty)
            {
                moveIsLeagel = false;
            }
            else if (i_DestCell.CellRow > boardSize || i_DestCell.CellCol > boardSize || i_DestCell.CellRow < 0 || i_DestCell.CellCol < 0)
            {
                moveIsLeagel = false;
            }

            switch (i_OriginCell.CellSign)
            {
                case eSign.O:
                    moveIsLeagel = CheakingOplayerCellsLegal(i_OriginCell, i_DestCell);
                    break;

                case eSign.X:
                    moveIsLeagel =  CheakingXplayerCellsLegal(i_OriginCell, i_DestCell);
                    break;
                case eSign.K:
                    //TODO
                    break;
                case eSign.U:
                    //TODO
                    break;
            }

            return moveIsLeagel;
        }
        public bool isPossibeleToEat(Cell i_TheCellInTheMidlle)
        {
            return true; // Todo
        }

        public bool CheakingOplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLeagel = true;
            if (i_OriginCell.CellRow <= i_DestCell.CellRow)
            {
                isLeagel = false;
            }
            else if (i_OriginCell.CellRow < i_DestCell.CellRow - 2 || i_OriginCell.CellCol < i_DestCell.CellCol - 2 || i_OriginCell.CellCol < i_DestCell.CellCol + 2)
            {
                isLeagel = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol - 2)
            {
                if (!isPossibeleToEat(gameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol + 1]))
                {
                    isLeagel = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && i_OriginCell.CellCol == i_DestCell.CellCol + 2)
            {
                if (!isPossibeleToEat(gameBoard.m_PlayBoard[i_OriginCell.CellRow + 1, i_OriginCell.CellCol - 1]))
                {
                    isLeagel = false;
                }
            }

            return isLeagel;
        }

        public bool CheakingXplayerCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            bool isLeagel = true;

            if (i_OriginCell.CellRow >= i_DestCell.CellRow)
            {
                isLeagel = false;
            }

            else if (i_OriginCell.CellRow > i_DestCell.CellRow + 2 || i_OriginCell.CellCol < i_DestCell.CellCol - 2 || i_OriginCell.CellCol < i_DestCell.CellCol + 2)
            {
                isLeagel = false;
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && (i_OriginCell.CellCol == i_DestCell.CellCol - 2)) 
            {
                if (!isPossibeleToEat(gameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol - 1]))
                {
                    isLeagel = false;
                }
            }
            else if (i_OriginCell.CellRow == i_DestCell.CellRow - 2 && (i_OriginCell.CellCol == i_DestCell.CellCol +2))
            {
                if (!isPossibeleToEat(gameBoard.m_PlayBoard[i_OriginCell.CellRow - 1, i_OriginCell.CellCol + 1]))
                {
                    isLeagel = false;
                }
            }

            return isLeagel;
        }

    }
}