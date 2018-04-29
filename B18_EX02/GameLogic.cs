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

        private void initializeBoard(byte boardSize)
        {
            Board = new Board(boardSize);
        }

    }
}