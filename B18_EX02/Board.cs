namespace B18_EX02
{
    public class Board
    {
        private readonly byte NumOfRows;
        private readonly byte NumOfCols;
        private Cell[,] m_PlayBoard;


        public Board(byte i_boardSize)
        {
            NumOfRows = NumOfCols = BoardSize = i_boardSize;
            setBoard();
        }


        private void ResetBoard()
        {
            for (byte curRow = 0; curRow < NumOfRows; curRow++)
            {
                for (byte curCol = 0; curCol < NumOfCols; curCol++)
                {
                    m_PlayBoard[curRow, curCol].CellSign = eSign.Empty;
                }
            }
        }

        public byte BoardSize { get; }

        public Cell this[Cell i_CurrentCell]
        {
            get { return m_PlayBoard[i_CurrentCell.CellRow, i_CurrentCell.CellCol]; }
        }

        public Cell this[byte i_CurrRow, byte i_CurrCol] => m_PlayBoard[i_CurrRow, i_CurrCol];

        private void setBoard()
        {
            m_PlayBoard = new Cell[NumOfRows, NumOfCols];
            for (byte curRow = 0; curRow < NumOfRows; curRow++)
            {
                for (byte curCol = 0; curCol < NumOfCols; curCol++)
                {
                    m_PlayBoard[curRow, curCol] = new Cell(curRow, curCol,eSign.Empty);
                }
            }
        }

    }

    
}
