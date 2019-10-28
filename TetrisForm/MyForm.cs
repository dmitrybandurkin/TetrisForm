using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace TetrisForm
{
    enum Direction
    {
        Left,
        Right,
        Down,
        IDown,
        Rotate
    }
    class MyForm : Form
    {
        Timer game_timer;
        Board board;
        Tetromino current;
        Label menu;
        Graphics gr;

        int height_in_cells;
        int width_in_cells;
        int cell_size;

        string str;

        public MyForm() : base()
        {
            Init();
        }
        private void Init()
        {
            gr = this.CreateGraphics();
            height_in_cells = 24;
            width_in_cells = 11;
            cell_size = 20;
            menu = new Label();
            menu.SetBounds(-10, 480, 237, 60);
            menu.BackColor = Color.Transparent;
            menu.Font = new Font("Courier New",12,FontStyle.Bold);
            menu.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(menu);
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Height = 600;
            this.Width = 237;
            this.Text = "Tetris v.2.0";
            this.Font = new Font("Courier New", 20, FontStyle.Bold);
            this.BackgroundImage = Image.FromFile(Environment.CurrentDirectory + @"\333.jpg");
            
            board = new Board(height_in_cells, width_in_cells);
            current = new Tetromino();
            current.GetTetromino();

            game_timer = new Timer()
            {
                Interval = 1000,
                Enabled = true
            };

            game_timer.Tick += DownShift;
            game_timer.Start();
            this.KeyDown += MyForm_KeyDown;
            this.DoubleBuffered = true;
            this.Paint += MyForm_Paint;
        }
        private void DownShift(object sender, EventArgs e)
        {
            MoveDirection(Direction.Down);
        }
        private void MyForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.P:
                    if (game_timer.Enabled) game_timer.Stop();
                    else { game_timer.Start(); this.Refresh(); };

                    str = (!game_timer.Enabled) ? "PAUSE" : "";
                    gr.DrawString(str, this.Font, Brushes.Black, 70 ,200) ;

                    break;
                case Keys.Left when game_timer.Enabled:
                    MoveDirection(Direction.Left);
                    break;
                case Keys.Right when game_timer.Enabled:
                    MoveDirection(Direction.Right);
                    break;
                case Keys.Down when game_timer.Enabled:
                    MoveDirection(Direction.Down);
                    break;
                case Keys.Space when game_timer.Enabled:
                    MoveDirection(Direction.IDown);
                    break;
                case Keys.Up when game_timer.Enabled:
                    MoveDirection(Direction.Rotate);
                    break;
            }
        }
        public void MyForm_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics graph = e.Graphics;

            menu.Text = ("Линий:" + board.lines + "\nОчков:" + board.score);
            if (StopGame()) menu.Text = ("GAME OVER");

            for (int y = 0; y < board.field.GetLength(0) - 1; y++)
            {
                for (int x = 1; x < board.field.GetLength(1) - 1; x++)
                {
                    if (board.field[y, x].pCellkind != CellKind.Block) board.field[y, x].pCellkind = CellKind.Empty;
                }
            }

            foreach (Coordinates c in current.pC)
            {
                board.field[c.pY, c.pX].pCellkind = c.pCellkind;
            }

            for (int y = 0; y < board.field.GetLength(0); y++)
            {
                for (int x = 0; x < board.field.GetLength(1); x++)
                {
                    if (board.field[y, x].pCellkind == CellKind.Figure)
                    {
                        graph.DrawRectangle(new Pen(this.BackColor, 2), x * cell_size, y * cell_size, cell_size, cell_size);
                        graph.FillRectangle(new SolidBrush(Color.Blue), x * cell_size, y * cell_size, cell_size, cell_size);

                    }
                    if (board.field[y, x].pCellkind == CellKind.Border)
                    {
                        graph.DrawImage(Image.FromFile("444.jpg"), x * cell_size, y * cell_size, cell_size, cell_size);
                        //graph.DrawRectangle(new Pen(this.BackColor, 2), x * cell_size, y * cell_size, cell_size, cell_size);
                        //graph.FillRectangle(new SolidBrush(Color.Yellow), x * cell_size, y * cell_size, cell_size, cell_size);
                    }
                    if (board.field[y, x].pCellkind == CellKind.Block)
                    {
                        graph.DrawRectangle(new Pen(this.BackColor, 2), x * cell_size, y * cell_size, cell_size, cell_size);
                        graph.FillRectangle(new SolidBrush(Color.Green), x * cell_size, y * cell_size, cell_size, cell_size);
                    }
                }
            }
        }
        private bool StopGame()
        {
            for (int x = 4; x <= 5;x++)
            {
                if (board.field[4, x].pCellkind == CellKind.Block)
                {
                    game_timer.Tick -= DownShift;
                    this.KeyDown -= MyForm_KeyDown;
                    return true;
                }
            }
            return false;
        }
        private void MoveDirection(Direction direction)
        {
            StopGame();
            int shift = 0;
            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    shift = (direction == Direction.Left) ? -1 : 1;
                    if (CanMove(shift, 0,current))
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            current.pC[i].pX += shift;
                        }
                    }
                    break;

                case Direction.Rotate:
                    Tetromino rotated = current.CopyTetromino();
                    rotated.Rotate();
                    if (CanMove(shift, 0, rotated))
                    {
                        current.Rotate();
                    }
                    break;

                case Direction.IDown:
                    Down(true);
                    break;

                case Direction.Down:
                    Down();
                    break;
            }
            this.Refresh();
        }
        private void Down(bool inst=false)
        {
            int shift=1;
            bool canshift=true;

            if (inst)
            {
                do
                {
                    if (CanMove(0, shift, current))
                    {
                        shift++;
                    }
                    else canshift = false;
                } while (canshift);

                for (int i = 0; i < 4; i++)
                {
                    current.pC[i].pY += shift-1;
                }

                BlockRestart();
            }

            else
            {
                if (CanMove(0, shift, current))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        current.pC[i].pY += shift;
                    }
                }
                else
                {
                    BlockRestart();
                }
            } 
        }
        private void BlockRestart()
        {
            foreach (Coordinates c in current.pC)
            {
                board.field[c.pY, c.pX].pCellkind = CellKind.Block;
            }
            board.ClearBoard();
            current = new Tetromino();
            current.GetTetromino();
        }
        private bool CanMove(int x, int y,Tetromino tetromino)
        {
            foreach (Coordinates c in tetromino.pC)
            {
                if (c.pX + x <= 0) return false;
                if (c.pX + x >= (width_in_cells - 1)) return false;
                if (c.pY + y < 0 ) return false;
                if (board.field[c.pY, c.pX + x].BorderOrBlock) return false;
                if (board.field[c.pY + y, c.pX].BorderOrBlock) return false;
            }
            return true;
        }
    }
}
