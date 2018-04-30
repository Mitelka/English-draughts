namespace B18_EX02
{
    public class Cell
    {
        private byte m_CellRow;
        private byte m_CellCol;
        private eSign m_CellSign;
        private const int k_LegalNumberOfInput = 2;

        public Cell(byte i_CellRow, byte i_CellCol, eSign i_CellSign)
        {
            m_CellRow = i_CellRow;
            m_CellCol = i_CellCol;
            m_CellSign = CellSign = i_CellSign;
        }

        public eSign CellSign { get; set; }
        public byte CellRow { get; set; }
        public byte CellCol { get; set; }

        public static bool Parse(string i_Input, out Cell o_ParsedCell)
        {
            bool isValidInput = false;
            o_ParsedCell = null;
            if(i_Input.Length == k_LegalNumberOfInput)
            {
                if(char.IsUpper(i_Input[0]) && char.IsLower(i_Input[1]))
                {
                    o_ParsedCell = new Cell((byte)(i_Input[0] - 'A'), (byte)(i_Input[1] - 'a'), eSign.Empty);
                    isValidInput = true;
                }
            }
            return isValidInput;
        }
}
}
