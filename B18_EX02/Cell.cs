using System.Text;

namespace B18_EX02
{
    public class Cell
    {
        private const int k_LegalNumberOfInput = 2;
        private byte m_CellRow;
        private byte m_CellCol;
        private eSign m_CellSign;

        public Cell(byte i_CellRow, byte i_CellCol, eSign i_CellSign)
        {
            m_CellRow = i_CellRow;
            m_CellCol = i_CellCol;
            m_CellSign = i_CellSign;
        }

        public eSign CellSign { get => m_CellSign; set => m_CellSign = value; }

        public byte CellRow { get => m_CellRow; set => m_CellRow = value; }

        public byte CellCol { get => m_CellCol; set => m_CellCol = value; }

        public static bool Parse(string i_Input, out Cell o_ParsedCell)
        {
            bool isValidInput = false;
            o_ParsedCell = null;
            if (i_Input.Length == k_LegalNumberOfInput)
            {
                if (char.IsUpper(i_Input[0]) && char.IsLower(i_Input[1]))
                {
                    o_ParsedCell = new Cell((byte)(i_Input[1] - 'a'), (byte)(i_Input[0] - 'A'), eSign.Empty);
                    isValidInput = true;
                }
            }

            return isValidInput;
        }

        public string GetCellStr()
        {
            StringBuilder cellStr = new StringBuilder();
            cellStr.Append((char)(m_CellCol + 'A'));
            cellStr.Append((char)(m_CellRow + 'a'));

            return cellStr.ToString();
        }
    }
}
