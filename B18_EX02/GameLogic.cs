namespace B18_EX02
{
    internal class GameLogic
    {
        private Player[] players;
        private int boardSize;
        private eGameType gameType;

        public GameLogic(Player[] i_players, int i_boardSize, eGameType i_gameType)
        {
            players = i_players;
            boardSize = i_boardSize;
            gameType = i_gameType;
            // Todo initializeBoard
        }
    }
}