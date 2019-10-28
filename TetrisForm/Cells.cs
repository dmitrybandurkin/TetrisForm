using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisForm
{
    enum CellKind
    {
        Figure,
        Block,
        Empty,
        Border
    }
    class Cells
    {
        public CellKind pCellkind { get; set; }
        public bool BorderOrBlock => (pCellkind == CellKind.Border || pCellkind == CellKind.Block);
        public Cells(CellKind kind) => pCellkind = kind;
    }
}
