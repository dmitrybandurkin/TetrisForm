using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisForm
{
    class Board
    {
        public Cells[,] field;
        public int lines { get; set; }
        public int score { get; set; }
        public int combo { get; set; }
        public Board(int rows, int columns)
        {
            field = new Cells[rows, columns];
            InitField();

            lines = 0;
            score = 0;
        }
        public void InitField()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for(int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = new Cells(CellKind.Border);
                }
            }

            for (int i = 0; i < field.GetLength(0)-1; i++)
            {
                for (int j = 1; j < field.GetLength(1)-1; j++)
                {
                    field[i, j].pCellkind = CellKind.Empty;
                }
            }
        }
        public void ClearBoard()
        {
            bool clear = true;
            int row = 0;
            int combo = 0;

            while (clear)
            {
                for (int i = field.GetLength(0) - 2; i > 0; i--)
                {
                    clear = true;
                    for (int j = 1; j < field.GetLength(1) - 1; j++)
                    {
                        if (field[i,j].pCellkind != CellKind.Block) clear = false;
                    }
                    if (clear)
                    {
                        row = i;
                        lines++;
                        break;
                    }
                }

                if (clear)
                {
                    for (int i = row; i > 4; i--)
                    {
                        for (int j = 1; j < field.GetLength(1) - 1; j++)
                        {
                            field[i, j].pCellkind = field[i - 1, j].pCellkind;
                        }
                    }
                    combo++;
                }
            }
            score += GetScore(combo);
        }
        public int GetScore(int c) => (c < 2)? 80*c:(80*c*c);
    }
}
