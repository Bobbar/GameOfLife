using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private int rows = 400;
        private int cols = 400;
        private int padding = 200;
        private const int interval = 10;
        private bool[,] cells;
        private bool[,] nextCells;
        private int numAlive = 0;

        private PointF cellSize = new PointF();
        private Point cursor = new Point();

        private System.Windows.Forms.Timer stepper = new System.Windows.Forms.Timer();

        public Form1()
        {
            InitializeComponent();

            cells = new bool[cols, rows];
            nextCells = new bool[cols, rows];

            stepper.Interval = interval;
            stepper.Tick += Stepper_Tick;

            cellSize = new PointF(pictureBox.Width / (float)cols, pictureBox.Height / (float)rows);
        }

        private void RandomizeCells()
        {
            var rnd = new Random();

            int passes = rows * 10;

            for (int i = 0; i < passes; i++)
            {
                int rndX = rnd.Next(0, cols);
                int rndY = rnd.Next(0, rows);

                cells[rndX, rndY] = Convert.ToBoolean(rnd.Next(2));
            }



            //for (int x = 0; x < rows; x++)
            //{
            //    for (int y = 0; y < rows; y++)
            //    {
            //        cells[x, y] = Convert.ToBoolean(rnd.Next(2));
            //    }
            //}
        }

        private void InitCells()
        {

            cells = new bool[cols, rows];
            nextCells = new bool[cols, rows];
        }

        private void ParseState()
        {
            //var file = $@"C:\Downloads\golly-4.1-win-64bit\golly-4.1-win-64bit\Patterns\Life-Like\replicator.rle";
            //var file = $@"C:\Downloads\golly-4.1-win-64bit\golly-4.1-win-64bit\Patterns\Life-Like\spiral-growth.rle";
            var file = $@"C:\Downloads\golly-4.1-win-64bit\golly-4.1-win-64bit\Patterns\Life\Spaceships\orthogonal.rle";

            int startX = 0;
            int startY = 0;
            int sizeX = 0;
            int sizeY = 0;
            int x = 0;
            int y = 0;
            foreach (var line in File.ReadLines(file))
            {
                if (line[0] == 'x')
                {
                    var parms = line.Replace(" ", string.Empty).Split(',');
                    string xStr = parms[0].Substring(2);
                    string yStr = parms[1].Substring(2);
                    sizeX = int.Parse(xStr);
                    sizeY = int.Parse(yStr);
                    //x = startX + padding / 2;
                    //y = startY + padding / 2;

                    startX = padding / 2;
                    startY = padding / 2;
                    x = startX;
                    y = startY;

                    cols = sizeX + padding;
                    rows = sizeY + padding;
                    InitCells();

                    continue;
                }
                else if (line[0] == '#')
                    continue;
                else
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        int n = 1;
                        string nStr = string.Empty;

                        if (char.IsDigit(line[i]))
                        {
                            while (char.IsDigit(line[i]))
                            {
                                nStr += line[i].ToString();
                                i++;
                            }

                            n = int.Parse(nStr);
                        }

                        switch (line[i])
                        {
                            case 'b':
                                for (int j = 0; j < n; j++)
                                    cells[x++, y] = false;
                                break;

                            case 'o':
                                for (int j = 0; j < n; j++)
                                    cells[x++, y] = true;
                                break;

                            case '$':
                                x = startX;
                                y += n;
                                break;

                            case '!':
                                return;
                        }
                    }
                }

            }

        }

        private void UpdateCells()
        {
            const int iterations = 300;
            // Compute next states.

            for (int i = 0; i < iterations; i++)
            {
                var population = 0;

                for (int x = 0; x < cols; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        var cell = cells[x, y];
                        var nAlive = NumLivingNeighbors(x, y);

                        nextCells[x, y] = cell;

                        if (cell)
                        {
                            if (nAlive < 2)
                                nextCells[x, y] = false;

                            if (nAlive > 3)
                                nextCells[x, y] = false;
                        }
                        else
                        {
                            if (nAlive == 3)
                                nextCells[x, y] = true;
                        }

                        if (nextCells[x, y])
                            population++;
                    }
                }

                var tmp = cells;
                cells = nextCells;
                nextCells = tmp;

                numAlive = population;

            }
        }

        private int NumLivingNeighbors(int cellX, int cellY)
        {
            var living = 0;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int ox = cellX + x;
                    int oy = cellY + y;

                    if (ox >= 0 && oy >= 0 && ox < cols && oy < rows)
                        if (cells[ox, oy])
                            living++;

                }
            }

            return living;
        }

        private void Clear()
        {
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    cells[x, y] = false;
                }
            }

            pictureBox.Refresh();
        }

        private void Stepper_Tick(object? sender, EventArgs e)
        {
            UpdateCells();
            numAliveLabel.Text = $"Alive: {numAlive}";
            pictureBox.Refresh();
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            //float sizeX = pictureBox.Width / (float)cols;
            //float sizeY = pictureBox.Height / (float)rows;
            cellSize = new PointF(pictureBox.Width / (float)cols, pictureBox.Height / (float)rows);

            //float sizeX = 1.0f;
            //float sizeY = 1.0f;

            float posX = 0;
            float posY = 0;

            for (int x = 0; x < cols; x++)
            {
                posY = 0;
                for (int y = 0; y < rows; y++)
                {
                    var cell = cells[x, y];

                    if (cell)
                        e.Graphics.FillRectangle(Brushes.Black, posX, posY, cellSize.X, cellSize.Y);

                    posY += cellSize.Y;
                }
                posX += cellSize.X;
            }

            e.Graphics.DrawRectangle(Pens.Red, cursor.X * cellSize.X, cursor.Y * cellSize.Y, cellSize.X, cellSize.Y);

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            stepper.Enabled = !stepper.Enabled;

            if (stepper.Enabled)
            {
                stepper.Start();
                startButton.Text = "Stop";
            }
            else
            {
                stepper.Stop();
                startButton.Text = "Start";
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            cellSize = new PointF(pictureBox.Width / (float)cols, pictureBox.Height / (float)rows);


            var pos = e.Location;
            var cellIdx = new Point();

            cellIdx.X = (int)(pos.X / cellSize.X);
            cellIdx.Y = (int)(pos.Y / cellSize.Y);

            cursor = cellIdx;

            cells[cellIdx.X, cellIdx.Y] = !cells[cellIdx.X, cellIdx.Y];

            Debug.WriteLine($"Pos: {pos}  Idx: {cellIdx}");

            pictureBox.Refresh();
        }

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            RandomizeCells();
            pictureBox.Refresh();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            ParseState();
            pictureBox.Refresh();

        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            UpdateCells();
            numAliveLabel.Text = $"Alive: {numAlive}";
            pictureBox.Refresh();
        }

        private void pictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //switch (e.KeyData)
            //{
            //    case Keys.Up:
            //        cursor.Y -= (int)cellSize.Y;
            //        break;

            //    case Keys.Down:
            //        cursor.Y += (int)cellSize.Y;
            //        break;

            //    case Keys.Left:
            //        cursor.X -= (int)cellSize.X;
            //        break;

            //    case Keys.Right:
            //        cursor.X += (int)cellSize.X;
            //        break;

            //}

            //pictureBox.Refresh();
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.W:
                    cursor.Y -= 1;
                    break;

                case Keys.S:
                    cursor.Y += 1;
                    break;

                case Keys.A:
                    cursor.X -= 1;
                    break;

                case Keys.D:
                    cursor.X += 1;
                    break;

                case Keys.Space:
                    cells[cursor.X, cursor.Y] = !cells[cursor.X, cursor.Y];
                    break;

            }

            pictureBox.Refresh();

            //switch (e.KeyData)
            //{
            //    case Keys.W:
            //        cursor.Y -= (int)cellSize.Y;
            //        break;

            //    case Keys.S:
            //        cursor.Y += (int)cellSize.Y;
            //        break;

            //    case Keys.A:
            //        cursor.X -= (int)cellSize.X;
            //        break;

            //    case Keys.D:
            //        cursor.X += (int)cellSize.X;
            //        break;

            //}

            //pictureBox.Refresh();
        }
    }
}