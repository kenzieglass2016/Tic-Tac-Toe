using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace FormExample
{
    public partial class Form1 : Form
    {
        Random rng = new Random();
        bool startup = true;
        bool SongPlay = true;
        int x;
        int y;
        int turn;
        int winner;
        int cellSize;
        int margin = 10;
        int row = -1;
        int col = -1;
        int[][] board = new int[3][] { new int[3] { 0, 0, 0 }, new int[3] { 0, 0, 0 }, new int[3] { 0, 0, 0 } };

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            UpdateSize();
            NewGame();
            PlayMusic();
        }

        private void UpdateSize()
        {
            cellSize = (Math.Min(ClientSize.Width, ClientSize.Height) - 2 * margin) / 3;
            if (ClientSize.Width > ClientSize.Height)
            {
                x = (ClientSize.Width - 3 * cellSize) / 2;
                y = margin;
            }
            else
            {
                x = margin;
                y = (ClientSize.Height - 3 * cellSize) / 2;
            }
        }

        protected override void OnResize(EventArgs e)
        {

            base.OnResize(e);
            UpdateSize();
            Refresh();
        }
        
        private void NewGame()
        {
            winner = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[j][i] = 0;
                }
            }
            turn = rng.Next(1, 3);
            Refresh();
        }
        
        private int CheckWin()
        {
            #region Crazy Check Winner
            //Checks Top Row and Left Column, returning the link (0,0), if a win is found.
            if (board[0][0].Equals(board[0][1]) && board[0][1].Equals(board[0][2])
             || board[0][0].Equals(board[1][0]) && board[1][0].Equals(board[2][0]))
            {
                if (board[0][0] != 0) return board[0][0];
            }
            //Checks Middle Row and Middle Column, returning the link (1,1), if a win is found.
            if (board[1][0].Equals(board[1][1]) && board[1][1].Equals(board[1][2])
             || board[0][1].Equals(board[1][1]) && board[1][1].Equals(board[2][1]))
            {
                if (board[1][1] != 0) return board[1][1];
            }
            //Checks Bottom Row and Right Column, returning the link (2,2), if a win is found.
            if (board[2][0].Equals(board[2][1]) && board[2][1].Equals(board[2][2])
             || board[0][2].Equals(board[1][2]) && board[1][2].Equals(board[2][2]))
            {
                if (board[2][2] != 0) return board[2][2];
            }
            //Checks both of the Diagonals, returning the link (1,1), if a win is found.
            if (board[0][0].Equals(board[1][1]) && board[1][1].Equals(board[2][2])
             || board[0][2].Equals(board[1][1]) && board[1][1].Equals(board[2][0]))
            {
                if (board[1][1] != 0) return board[1][1];
            }
            return 0;
            #endregion
        }

        private bool BoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[j][i] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            col = (int)Math.Floor((e.X - x) * 1.0 / cellSize);
            row = (int)Math.Floor((e.Y - y) * 1.0 / cellSize);
            if (col < 3 && col >= 0 && row < 3 && row >= 0)
            {
                if (turn == 1)
                {
                    if (board[col][row] == 0)
                    {
                        board[col][row] = 1;
                        turn = 2;
                    }
                }
                else
                {
                    if (board[col][row] == 0)
                    {
                        board[col][row] = 2;
                        turn = 1;
                    }
                }
                winner = CheckWin();
            }
            Refresh();
            if (winner != 0)
            {
                if (winner == 1)
                {
                    MessageBox.Show("The Winner is X!");
                }
                else
                {
                    MessageBox.Show("The Winner is O!");
                }
                NewGame();
            }
            else if (BoardFull())
            {
                MessageBox.Show("The Game is a Tie!");
                NewGame();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K)
            {
                if(SongPlay == true)
                {
                    SongPlay = false;
                    PlayMusic();
                }
                else
                {
                    SongPlay = true;
                    PlayMusic();
                }
            }

            if (e.KeyCode == Keys.Space)
            {
                startup = false;
                Refresh();
            }
         }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (startup == true)
            {
                e.Graphics.DrawImage(FormExample.Properties.Resources.TicTacToe, 0, 0, ClientSize.Width, ClientSize.Height);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Rectangle rect = new Rectangle(x + i * cellSize, y + j * cellSize, cellSize, cellSize);
                        e.Graphics.DrawRectangle(Pens.Black, rect);
                        System.Drawing.Font font = new System.Drawing.Font("Ubuntu", cellSize * 3 * 72 / 96 / 4);
                        if (board[j][i] == 1)
                        {
                            e.Graphics.DrawString("X", font, Brushes.DarkRed, x + (cellSize / 8) + (j * cellSize), y + (cellSize / 8) + (i * cellSize) - (cellSize / 20));
                        }
                        else if (board[j][i] == 2)
                        {
                            e.Graphics.DrawString("O", font, Brushes.Black, x + (cellSize / 8) + (j * cellSize) - (cellSize / 20), y + (cellSize / 8) + (i * cellSize) - (cellSize / 20));
                        }
                    }
                }
            }
        }

        private void PlayMusic()
        {
            SoundPlayer play = new SoundPlayer(FormExample.Properties.Resources.Sans);
            if(SongPlay == true)
            {
                play.PlayLooping();
            }
            else
            {
                play.Stop();
            }
        }

    }

}