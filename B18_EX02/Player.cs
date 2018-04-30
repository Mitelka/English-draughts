namespace B18_EX02
{
    internal class Player
    {
        private ePlayerType playerType;
        private eSign sign;
        private int score;
        private string playerName;

        internal class PlayerMovelist
        {
            public Cell originalCell { get; set; }
            public Cell desiredCell { get; set; }

        }

        public Player(ePlayerType i_PlayerType, eSign i_Sign, string i_playerName)
        {
            PlayerType = i_PlayerType;
            Sign = i_Sign;
            playerName = PlayerName = i_playerName;
        }

        public ePlayerType PlayerType { get; set; }

        public eSign Sign { get; set; }

        public int Score { get; set; }

        public string PlayerName { get; set; }
    }
}
