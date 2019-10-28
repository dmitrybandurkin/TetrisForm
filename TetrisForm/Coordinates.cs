using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisForm
{
    struct Coordinates
    {
        public int pX { get; set; }
        public int pY { get; set; }
        public CellKind pCellkind { get; set; }
        public Coordinates(int x, int y)
        {
            this.pX = x;
            this.pY = y;
            this.pCellkind = CellKind.Figure;
        }
    }
}
