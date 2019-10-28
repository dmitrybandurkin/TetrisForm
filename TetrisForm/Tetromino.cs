using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisForm
{
    enum TetrisType
    {
        I,
        O,
        Z,
        S,
        L,
        IL,
        T
    }
    class Tetromino
    {
        Dictionary<TetrisType, Coordinates[]> tetro_list;
        Random random;
        Coordinates[] coordinates;
        public Coordinates[] pC => coordinates;
        public TetrisType pTetris_type { get; set; }

        public Tetromino()
        {
            coordinates = new Coordinates[4];
            tetro_list = new Dictionary<TetrisType, Coordinates[]>();
            tetro_list.Add(TetrisType.I, new Coordinates[4] { new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(5, 2), new Coordinates(5, 3) });
            tetro_list.Add(TetrisType.O, new Coordinates[4] { new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(4, 0), new Coordinates(4, 1) });
            tetro_list.Add(TetrisType.Z, new Coordinates[4] { new Coordinates(4, 0), new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(6, 1) });
            tetro_list.Add(TetrisType.S, new Coordinates[4] { new Coordinates(4, 1), new Coordinates(5, 1), new Coordinates(5, 0), new Coordinates(6, 0) });
            tetro_list.Add(TetrisType.L, new Coordinates[4] { new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(5, 2), new Coordinates(6, 2) });
            tetro_list.Add(TetrisType.IL, new Coordinates[4] { new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(5, 2), new Coordinates(4, 2) });
            tetro_list.Add(TetrisType.T, new Coordinates[4] { new Coordinates(5, 0), new Coordinates(5, 1), new Coordinates(5, 2), new Coordinates(4, 1) });
        }

        public Tetromino GetTetromino()
        {
            random = new Random();
            int num = tetro_list.Count;
            this.pTetris_type = (TetrisType)(random.Next(num));
            this.coordinates = tetro_list[this.pTetris_type];
            return this;
        }

        public void Rotate()
        {
            if (this.pTetris_type != TetrisType.O)
            {
                int[,] rM = { { 0, 1 }, { -1, 0 } };
                int[] tM = { this.coordinates[1].pX, this.coordinates[1].pY };
                for (int i = 0; i < 4; i++)
                {
                    int[] cM = { this.coordinates[i].pX, this.coordinates[i].pY };
                    this.coordinates[i].pX = (cM[0] - tM[0]) * rM[0, 0] + (cM[1] - tM[1]) * rM[0, 1] + tM[0];
                    this.coordinates[i].pY = (cM[0] - tM[0]) * rM[1, 0] + (cM[1] - tM[1]) * rM[1, 1] + tM[1];
                }
            }
            
        }

        public Tetromino CopyTetromino()
        {
            Tetromino copy = new Tetromino();
            copy.pTetris_type = this.pTetris_type;
            for (int i = 0; i < 4; i++)
            {
                copy.coordinates[i].pX = this.coordinates[i].pX;
                copy.coordinates[i].pY = this.coordinates[i].pY;
            }
            return copy;
        }
    }
}
