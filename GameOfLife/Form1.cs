using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private int rows = 500;
        private int cols = 500;
        private const int interval = 10;
        private int stepsPerCycle = 1;
        private int[,] cells;
        private int[,] nextCells;

        private System.Windows.Forms.Timer stepper = new System.Windows.Forms.Timer();
        private readonly List<NamedRule> rules = Rules.LifeRules;
        private NamedRule currentRule;
        private OpenCLCompute ocl;
        private int oclGPUIdx = 0;
        private bool UseOpenCL = true;
        private Bitmap cellFieldImg;

        public Form1()
        {
            InitializeComponent();

            InitCells();

            stepper.Interval = interval;
            stepper.Tick += Stepper_Tick;

            InitFieldImage();

            InitOpenCL();

            PopRules();

            rowsTextBox.Text = rows.ToString();
            colsTextBox.Text = cols.ToString();
            stepsNumericUpDown.Value = stepsPerCycle;
        }

        private void PopRules()
        {
            foreach (var rule in rules)
            {
                ruleComboBox.Items.Add(rule.Name);
            }

            ruleComboBox.SelectedIndex = 8;
        }

        private void InitOpenCL()
        {
            ocl?.Dispose();
            ocl = new OpenCLCompute(oclGPUIdx, new int2() { X = cols, Y = rows });
        }

        private void InitFieldImage()
        {
            cellFieldImg?.Dispose();
            cellFieldImg = new Bitmap(cols, rows);

            pictureBox.Image = cellFieldImg;
        }

        private void InitCells()
        {
            cells = new int[cols, rows];
            nextCells = new int[cols, rows];
        }

        private void RandomizeCells()
        {
            var rnd = new Random();

            int passes = rows * 10;

            for (int i = 0; i < passes; i++)
            {
                int rndX = rnd.Next(0, cols);
                int rndY = rnd.Next(0, rows);

                cells[rndX, rndY] = rnd.Next(2);
            }
        }

        private void UpdateCells(int steps, bool useOCL)
        {
            // Compute next states.
            if (useOCL)
            {
                ocl.ComputeNextState(ref cells, steps, currentRule.Rule);
            }
            else
            {
                for (int i = 0; i < steps; i++)
                {
                    ParallelHelpers.ParallelForSlim(cols, 8, (start, len) =>
                    {
                        for (int x = start; x < len; x++)
                        {
                            for (int y = 0; y < rows; y++)
                            {
                                var cell = cells[x, y];
                                var nAlive = NumLivingNeighbors(x, y);
                                nextCells[x, y] = GetState(cell, nAlive, currentRule.Rule);
                            }
                        }
                    });

                    var tmp = cells;
                    cells = nextCells;
                    nextCells = tmp;
                }
            }
        }

        private int GetState(int current, int nAlive, Rule rules)
        {
            int next = 0;
            int rule = current == 1 ? rules.S : rules.B;
            for (int i = 0; i < 9; i++)
            {
                int ruleVal = 1 << i;
                if ((ruleVal & rule) != 0)
                {
                    if (nAlive == i)
                        next = 1;
                }
            }

            return next;
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
                        if (cells[ox, oy] == 1)
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
                    cells[x, y] = 0;
                }
            }

            RedrawFieldImage();
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
            int padding = 200;
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
                                    cells[x++, y] = 0;
                                break;

                            case 'o':
                                for (int j = 0; j < n; j++)
                                    cells[x++, y] = 1;
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

        private void Stepper_Tick(object? sender, EventArgs e)
        {
            UpdateCells(stepsPerCycle, UseOpenCL);
            RedrawFieldImage();
        }

        private unsafe void RedrawFieldImage()
        {
            int population = 0;

            // Write the cells directly to the bitmap.
            var data = cellFieldImg.LockBits(new Rectangle(0, 0, cols, rows), ImageLockMode.ReadWrite, cellFieldImg.PixelFormat);
            byte* pixels = (byte*)data.Scan0;

            const int alphaOffset = 3;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var cell = cells[x, y];
                    int pidx = (y * cols + x) * 4;

                    if (cell == 1)
                    {
                        pixels[pidx + alphaOffset] = 255;
                        population++;
                    }
                    else
                    {
                        pixels[pidx + alphaOffset] = 10;
                    }
                }
            }

            cellFieldImg.UnlockBits(data);

            numAliveLabel.Text = $"Population: {population}";
            pictureBox.Refresh();
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

        private void randomizeButton_Click(object sender, EventArgs e)
        {
            RandomizeCells();
            RedrawFieldImage();
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            ParseState();
            RedrawFieldImage();
        }

        private void stepButton_Click(object sender, EventArgs e)
        {
            UpdateCells(1, UseOpenCL);
            RedrawFieldImage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ocl?.Dispose();
        }

        private void ruleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentRule = rules[ruleComboBox.SelectedIndex];
        }

        private void useOpenCLCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UseOpenCL = useOpenCLCheckBox.Checked;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            int newRows, newCols;

            if (int.TryParse(rowsTextBox.Text.Trim(), out newRows) && int.TryParse(colsTextBox.Text.Trim(), out newCols))
            {
                rows = newRows;
                cols = newCols;
                InitCells();
                InitOpenCL();
                InitFieldImage();
                RedrawFieldImage();
            }
            else
            {
                rowsTextBox.Text = rows.ToString();
                colsTextBox.Text = cols.ToString();
            }
        }

        private void stepsNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            stepsPerCycle = (int)stepsNumericUpDown.Value;
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var posScaled = TranslateZoomMousePosition(e.Location);

            var cellIdx = new Point(posScaled.X, posScaled.Y);

            if (cellIdx.X >= 0 && cellIdx.X <= cols && cellIdx.Y >= 0 && cellIdx.Y <= rows)
                cells[cellIdx.X, cellIdx.Y] = cells[cellIdx.X, cellIdx.Y] == 1 ? 0 : 1;

            RedrawFieldImage();
        }

        private Point TranslateZoomMousePosition(Point coordinates)
        {
            if (pictureBox.Width == 0 || pictureBox.Height == 0 || cols == 0 || rows == 0) return coordinates;
            float imageAspect = (float)cols / rows;
            float controlAspect = (float)pictureBox.Width / pictureBox.Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                float ratioWidth = (float)cols / pictureBox.Width;
                newX *= ratioWidth;
                float scale = (float)pictureBox.Width / cols;
                float displayHeight = scale * rows;
                float diffHeight = Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                float ratioHeight = (float)rows / pictureBox.Height;
                newY *= ratioHeight;
                float scale = (float)pictureBox.Height / rows;
                float displayWidth = scale * cols;
                float diffWidth = pictureBox.Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }
    }
}