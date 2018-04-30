using System;

namespace B18_EX02
{
    internal class GameLogic
    {
        private Player[] players;
        private byte boardSize;
        private eGameType gameType;
        //private Board gameBoard;

        public GameLogic(Player[] i_players, byte i_boardSize, eGameType i_gameType)
        {
            players = i_players;
            boardSize = i_boardSize;
            gameType = i_gameType;
            initializeBoard(boardSize);
        }

        internal Board Board { get; set; }

        private void initializeBoard(byte i_BoardSize)
        {
            Board = new Board(i_BoardSize);
        }

        public void getComputerCellMove(int i_PlayerIndex, eSign playerSign, out Cell o_legalOriginCell, out Cell o_legalDesiredCell)
        {
            o_legalDesiredCell = null;
            o_legalOriginCell = null;
        }

        public bool areCellsLegal(Cell i_OriginCell, Cell i_DestCell)
        {
            return true;
        }

    }
}