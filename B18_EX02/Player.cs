namespace B18_EX02
{
    internal class Player
    {
        private ePlayerType m_PlayerType;
        private eSign m_Sign;
        private int m_Score;
        private string m_PlayerName;
        private int m_NumOfTokens;

        internal class PlayerMovelist
        {
            public Cell originalCell { get; set; }

            public Cell desiredCell { get; set; }
        }

        public Player(ePlayerType i_PlayerType, eSign i_Sign, string i_PlayerName)
        {
            m_PlayerType = i_PlayerType;
            m_Sign = i_Sign;
            m_PlayerName = i_PlayerName;
            m_NumOfTokens = 0;
        }

        public ePlayerType PlayerType { get => m_PlayerType; set => m_PlayerType = value; }

        public eSign Sign { get => m_Sign; set => m_Sign = value; }

        public int Score { get => m_Score; set => m_Score = value; }

        public string PlayerName { get => m_PlayerName; set => m_PlayerName = value; }

        public int NumOfTokens { get => m_NumOfTokens; set => m_NumOfTokens = value; }
    }
}
