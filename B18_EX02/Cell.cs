namespace B18_EX02
{
    public class Cell
    {
        private byte m_CellRow;
        private byte m_CellCol;
        private eSign m_CellSign;

        public Cell(byte i_CellRow, byte i_CellCol, eSign i_CellSign)
        {
            m_CellRow = i_CellRow;
            m_CellCol = i_CellCol;
            m_CellSign = i_CellSign;
        }

        public eSign CellSign { get; set; }
}
}
