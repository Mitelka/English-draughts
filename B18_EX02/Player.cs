namespace B18_EX02
{
    internal class Player
    {
        public Player(ePlayerType i_PlayerType, eSign i_Sign, string i_playerName)
        {
            PlayerType = i_PlayerType;
            Sign = i_Sign;
            playerName = i_playerName;
        }

        public ePlayerType PlayerType { get; set; }
        public eSign Sign { get; set; }
        public string playerName { get; set; }
        
        

    }
}
