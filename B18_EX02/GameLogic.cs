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
            return true;
        }

        public void MakeMoveOnBoard(Cell i_OriginCell, Cell i_DestCell, int i_PlayerIndex)
        {
            gameBoard[i_OriginCell] = i_OriginCell;
            gameBoard[i_DestCell] = i_DestCell;
        }

    }
}