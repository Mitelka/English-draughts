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

        public void ResetBoard()
        {
            setBoardInitialValues();
        }

        public byte BoardSize { get; }
        public Cell[,] PlayBoard { get; set; }

        public Cell this[Cell i_CurrentCell]
        {
            get { return m_PlayBoard[i_CurrentCell.CellRow, i_CurrentCell.CellCol]; }
        }

        public Cell this[byte i_CurrRow, byte i_CurrCol] => m_PlayBoard[i_CurrRow, i_CurrCol];

        private void setBoard()
        {
            m_PlayBoard = new Cell[NumOfRows, NumOfCols];
            setBoardInitialValues();
        }

        public void setBoardInitialValues()
        {
            int numOfRowsForPlayer = (BoardSize - 2) / 2;
            eSign signToSet;
            for (byte curRow = 0; curRow < NumOfRows; curRow++)
            {
                if (curRow < numOfRowsForPlayer)
                {
                    signToSet = eSign.O;
                }
                else if (curRow == numOfRowsForPlayer || curRow == numOfRowsForPlayer + 1)
                {
                    signToSet = eSign.Empty;
                }
                else
                {
                    signToSet = eSign.X;
                }
                for (byte curCol = 0; curCol < NumOfCols; curCol++)
                {
                    if ((curCol + curRow) % 2 == 0)
                    {
                        m_PlayBoard[curRow, curCol] = new Cell(curRow, curCol, eSign.Empty);
                    }
                    else
                    {
                        m_PlayBoard[curRow, curCol] = new Cell(curRow, curCol, signToSet);
                    }
                }
            }
        }
    }
}
