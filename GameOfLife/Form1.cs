using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using GameOfLife.OpenCL;
using GameOfLife.Rules;
using GameOfLife.Rendering;

namespace GameOfLife
{
    public partial class Form1 : Form
    {
        private int rows = 500;
        private int cols = 500;
        private int interval = 10;
        private int stepsPerCycle = 1;
        private const int NUM_THREADS = 8;
        private int[,] cells;
        private int[,] nextCells;
        private bool invertColors = false;
        private bool showGrid = false;
        private string customRulesPath = $@"{Environment.CurrentDirectory}\RuleList.txt";

        private System.Windows.Forms.Timer stepper = new System.Windows.Forms.Timer();
        private List<NamedRule> rules;
        private NamedRule currentRule;
        private OpenCLCompute ocl;
        private int oclGPUIdx = 0;
        private bool useOpenCL = true;
        private Bitmap cellFieldImg;
        private PanZoomRenderer renderer;
        private int curAliveAlpha = 255;
        private int curDeadAlpha = 10;

        public Form1()
        {
            InitializeComponent();

            InitCells();

            stepper.Interval = interval;
            stepper.Tick += Stepper_Tick;

            InitFieldImage();
            RedrawFieldImage();
            InitOpenCL();
            PopRules();

            cycleIntervalNumeric.Value = interval;
            ruleComboBox.SelectedIndex = 0;
            rowsTextBox.Text = rows.ToString();
            colsTextBox.Text = cols.ToString();
            stepsNumericUpDown.Value = stepsPerCycle;
            useOpenCLCheckBox.Checked = useOpenCL;
        }

        private void PopRules()
        {
            rules = IncludedRules.LifeRules;

            if (File.Exists(customRulesPath))
            {
                var customRules = JsonSerializer.Deserialize<List<NamedRule>>(File.ReadAllText(customRulesPath));

                if (customRules != null)
                    rules = rules.Union(customRules, new NameRuleComparer()).ToList();
            }

            ruleComboBox.Items.Clear();

            foreach (var rule in rules)
            {
                ruleComboBox.Items.Add($"{rule.Name}  ( {rule.RuleVal} )");
            }
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

            renderer?.Dispose();
            renderer = new PanZoomRenderer(cellFieldImg, pictureBox);
            renderer.MouseDown += Renderer_MouseDown;
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
                ComputeNextState(steps);
            }
        }

        private void ComputeNextState(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                ParallelHelpers.ParallelForSlim(cols, NUM_THREADS, (start, len) =>
                {
                    for (int x = start; x < len; x++)
                    {
                        for (int y = 0; y < rows; y++)
                        {
                            var cell = cells[x, y];
                            var nAlive = NumLivingNeighbors(x, y);
                            int next = GetState(cell, nAlive, currentRule.Rule);
                            nextCells[x, y] = next;
                        }
                    }
                });

                var tmp = cells;
                cells = nextCells;
                nextCells = tmp;
            }
        }

        private int GetState(int current, int nAlive, Rule rules)
        {
            int next = 0;

            // Select the appropriate rule based on the current state.
            int rule = current >= 1 ? rules.S : rules.B;

            for (int i = 0; i < 9; i++)
            {
                int ruleVal = 1 << i;
                if ((ruleVal & rule) != 0)
                {
                    if (nAlive == i)
                        next = 1;
                }
            }

            // Handle generational (multi-state) rules.
            if (rules.C > 0)
            {
                if (next == 0 && current == 1)
                    next = 2;
                else if (current >= 2)
                    next = ((current + 1) % rules.C);
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

        private void Stepper_Tick(object? sender, EventArgs e)
        {
            stepper.Interval = interval;
            UpdateCells(stepsPerCycle, useOpenCL);
            RedrawFieldImage();
        }

        private unsafe void RedrawFieldImage()
        {
            const int alphaOffset = 3;
            const int redOffset = 2;
            byte aliveAlpha = (byte)curAliveAlpha;
            byte deadAlpha = (byte)curDeadAlpha;
            int population = 0;

            if (invertColors)
            {
                aliveAlpha = (byte)curDeadAlpha;
                deadAlpha = (byte)curAliveAlpha;
            }

            // Write the cells directly to the bitmap.
            var data = renderer.Image.LockBits(new Rectangle(0, 0, cols, rows), ImageLockMode.WriteOnly, renderer.Image.PixelFormat);

            byte* pixels = (byte*)data.Scan0;

            ParallelHelpers.ParallelForSlim(cols, NUM_THREADS, (start, len) =>
            {
                for (int x = start; x < len; x++)
                {
                    for (int y = 0; y < rows; y++)
                    {
                        var cell = cells[x, y];
                        int cellIdx = (y * cols + x);
                        int pidx = cellIdx * 4;

                        if (cell >= 1)
                        {
                            pixels[pidx + alphaOffset] = aliveAlpha;

                            // Draw gradient for generational rules.
                            if (currentRule.Rule.C > 0)
                                pixels[pidx + redOffset] = (byte)(cell * (255 / currentRule.Rule.C));

                            population++;
                        }
                        else
                        {
                            pixels[pidx + alphaOffset] = deadAlpha;
                            pixels[pidx + redOffset] = 0;
                        }

                        if (showGrid)
                            pixels[pidx + alphaOffset] -= (byte)(((x + y) % 2) * deadAlpha);
                    }
                }
            });

            renderer.Image.UnlockBits(data);

            numAliveLabel.Text = $"Population: {population}";

            renderer.Refresh();
        }

        private string GenerateRandomRule(bool multiState)
        {
            const int maxStates = 48;
            var rnd = new Random();
            int bLen = rnd.Next(1, 9);
            int sLen = rnd.Next(1, 9);
            string B = string.Empty;
            string S = string.Empty;
            string C = string.Empty;

            for (int i = 0; i < bLen; i++)
            {
                var rndVal = rnd.Next(9).ToString();

                while (B.Contains(rndVal))
                    rndVal = rnd.Next(9).ToString();

                B += rndVal;

            }

            B = String.Concat(B.OrderBy(c => c));

            for (int i = 0; i < sLen; i++)
            {
                var rndVal = rnd.Next(9).ToString();

                while (S.Contains(rndVal))
                    rndVal = rnd.Next(9).ToString();

                S += rndVal;

            }

            S = String.Concat(S.OrderBy(c => c));

            if (multiState)
            {
                C = rnd.Next(2, maxStates).ToString();
                return $"B{B}/S{S}/C{C}";
            }

            return $"B{B}/S{S}";
        }

        private void SaveCustomRule()
        {
            using (var saveDialog = new SaveRuleDialog(currentRule))
            {
                if (saveDialog.ShowDialog(this) == DialogResult.OK)
                {
                    currentRule.Name = saveDialog.RuleName;

                    if (!File.Exists(customRulesPath))
                    {
                        var newRules = new List<NamedRule> { currentRule };

                        var newList = JsonSerializer.Serialize(newRules, new JsonSerializerOptions { WriteIndented = true });

                        if (newList != null)
                            File.WriteAllText(customRulesPath, newList);
                    }
                    else
                    {
                        var currentList = JsonSerializer.Deserialize<List<NamedRule>>(File.ReadAllText(customRulesPath));

                        if (currentList != null)
                            currentList.Add(currentRule);

                        var newList = JsonSerializer.Serialize(currentList, new JsonSerializerOptions { WriteIndented = true });

                        if (newList != null)
                            File.WriteAllText(customRulesPath, newList);
                    }

                    PopRules();

                    ruleComboBox.SelectedIndex = rules.Count - 1;
                }
            }
        }

        private void Fill(int xStep, int yStep)
        {
            if (xStep > 0 && yStep > 0)
            {
                for (int x = 0; x < cols; x += xStep)
                {
                    for (int y = 0; y < rows; y += yStep)
                    {
                        cells[x, y] = 1;
                    }
                }

                RedrawFieldImage();
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            stepper.Enabled = !stepper.Enabled;

            if (stepper.Enabled)
            {
                stepper.Start();
                startButton.BackColor = Color.LightGreen;
                startButton.Text = "Stop";
            }
            else
            {
                stepper.Stop();
                startButton.BackColor = Color.LightCoral;
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

        private void stepButton_Click(object sender, EventArgs e)
        {
            UpdateCells(1, useOpenCL);
            RedrawFieldImage();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ocl?.Dispose();
        }

        private void ruleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ruleComboBox.SelectedIndex < 0)
                return;

            currentRule = rules[ruleComboBox.SelectedIndex];
            customRuleTextBox.Clear();
        }

        private void useOpenCLCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            useOpenCL = useOpenCLCheckBox.Checked;
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

        private void Renderer_MouseDown(object? sender, MouseEventArgs e)
        {
            var cellIdx = e.Location;

            if (e.Button == MouseButtons.Right)
            {
                if (cellIdx.X >= 0 && cellIdx.X < cols && cellIdx.Y >= 0 && cellIdx.Y < rows)
                    cells[cellIdx.X, cellIdx.Y] = cells[cellIdx.X, cellIdx.Y] >= 1 ? 0 : 1;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (cellIdx.X >= 0 && cellIdx.X < cols && cellIdx.Y >= 0 && cellIdx.Y < rows)
                    Debug.WriteLine($"{cellIdx}: {cells[cellIdx.X, cellIdx.Y]}");
            }

            RedrawFieldImage();
        }

        private void invertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            invertColors = invertCheckBox.Checked;
            RedrawFieldImage();
        }

        private void randomRuleButton_Click(object sender, EventArgs e)
        {
            var rndRule = GenerateRandomRule(multiStateCheckBox.Checked);
            currentRule = new NamedRule("Random", rndRule);
            customRuleTextBox.Text = currentRule.RuleVal;
            ruleComboBox.SelectedIndex = -1;
        }

        private void applyRuleButton_Click(object sender, EventArgs e)
        {
            currentRule = new NamedRule("Custom", customRuleTextBox.Text.Trim());
            ruleComboBox.SelectedIndex = -1;
        }

        private void aliveContrastNumeric_ValueChanged(object sender, EventArgs e)
        {
            curAliveAlpha = (int)aliveContrastNumeric.Value;
            aliveContrastNumeric.Refresh();
            if (!stepper.Enabled)
                RedrawFieldImage();
        }

        private void deadContrastNumeric_ValueChanged(object sender, EventArgs e)
        {
            curDeadAlpha = (int)deadContrastNumeric.Value;
            aliveContrastNumeric.Refresh();
            if (!stepper.Enabled)
                RedrawFieldImage();
        }

        private void saveRuleButton_Click(object sender, EventArgs e)
        {
            SaveCustomRule();
        }

        private void cycleIntervalNumeric_ValueChanged(object sender, EventArgs e)
        {
            interval = (int)cycleIntervalNumeric.Value;
        }

        private void fillButton_Click(object sender, EventArgs e)
        {
            if (int.TryParse(fillStepXTextBox.Text.Trim(), out int xStep) && int.TryParse(fillStepYTextBox.Text.Trim(), out int yStep))
            {
                Fill(xStep, yStep);
            }
        }

        private void showGridCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            showGrid = showGridCheckBox.Checked;
            RedrawFieldImage();
        }
    }
}