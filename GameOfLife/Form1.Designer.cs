namespace GameOfLife
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.startButton = new System.Windows.Forms.Button();
            this.randomizeButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.numAliveLabel = new System.Windows.Forms.Label();
            this.loadButton = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.ruleComboBox = new System.Windows.Forms.ComboBox();
            this.useOpenCLCheckBox = new System.Windows.Forms.CheckBox();
            this.rowsTextBox = new System.Windows.Forms.TextBox();
            this.colsTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.stepsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.invertCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.customRuleTextBox = new System.Windows.Forms.TextBox();
            this.randomRuleButton = new System.Windows.Forms.Button();
            this.applyRuleButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepsNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BackColor = System.Drawing.Color.White;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(246, 6);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(851, 738);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(66, 460);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(91, 37);
            this.startButton.TabIndex = 1;
            this.startButton.TabStop = false;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // randomizeButton
            // 
            this.randomizeButton.Location = new System.Drawing.Point(66, 358);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.Size = new System.Drawing.Size(91, 45);
            this.randomizeButton.TabIndex = 2;
            this.randomizeButton.TabStop = false;
            this.randomizeButton.Text = "Randomize Cells";
            this.randomizeButton.UseVisualStyleBackColor = true;
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(66, 576);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(91, 37);
            this.clearButton.TabIndex = 3;
            this.clearButton.TabStop = false;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // numAliveLabel
            // 
            this.numAliveLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numAliveLabel.AutoSize = true;
            this.numAliveLabel.Location = new System.Drawing.Point(12, 732);
            this.numAliveLabel.Name = "numAliveLabel";
            this.numAliveLabel.Size = new System.Drawing.Size(68, 15);
            this.numAliveLabel.TabIndex = 4;
            this.numAliveLabel.Text = "Population:";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.loadButton.Location = new System.Drawing.Point(12, 48);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(91, 37);
            this.loadButton.TabIndex = 5;
            this.loadButton.TabStop = false;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Visible = false;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(66, 503);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(91, 37);
            this.stepButton.TabIndex = 6;
            this.stepButton.TabStop = false;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // ruleComboBox
            // 
            this.ruleComboBox.FormattingEnabled = true;
            this.ruleComboBox.Location = new System.Drawing.Point(12, 169);
            this.ruleComboBox.Name = "ruleComboBox";
            this.ruleComboBox.Size = new System.Drawing.Size(215, 23);
            this.ruleComboBox.TabIndex = 7;
            this.ruleComboBox.SelectedIndexChanged += new System.EventHandler(this.ruleComboBox_SelectedIndexChanged);
            // 
            // useOpenCLCheckBox
            // 
            this.useOpenCLCheckBox.AutoSize = true;
            this.useOpenCLCheckBox.Checked = true;
            this.useOpenCLCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useOpenCLCheckBox.Location = new System.Drawing.Point(66, 640);
            this.useOpenCLCheckBox.Name = "useOpenCLCheckBox";
            this.useOpenCLCheckBox.Size = new System.Drawing.Size(91, 19);
            this.useOpenCLCheckBox.TabIndex = 8;
            this.useOpenCLCheckBox.Text = "Use OpenCL";
            this.useOpenCLCheckBox.UseVisualStyleBackColor = true;
            this.useOpenCLCheckBox.CheckedChanged += new System.EventHandler(this.useOpenCLCheckBox_CheckedChanged);
            // 
            // rowsTextBox
            // 
            this.rowsTextBox.Location = new System.Drawing.Point(66, 282);
            this.rowsTextBox.Name = "rowsTextBox";
            this.rowsTextBox.Size = new System.Drawing.Size(43, 23);
            this.rowsTextBox.TabIndex = 9;
            this.rowsTextBox.Text = "500";
            // 
            // colsTextBox
            // 
            this.colsTextBox.Location = new System.Drawing.Point(115, 282);
            this.colsTextBox.Name = "colsTextBox";
            this.colsTextBox.Size = new System.Drawing.Size(42, 23);
            this.colsTextBox.TabIndex = 10;
            this.colsTextBox.Text = "500";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Rows/Cols";
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(66, 311);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(91, 27);
            this.applyButton.TabIndex = 12;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // stepsNumericUpDown
            // 
            this.stepsNumericUpDown.Location = new System.Drawing.Point(66, 431);
            this.stepsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stepsNumericUpDown.Name = "stepsNumericUpDown";
            this.stepsNumericUpDown.Size = new System.Drawing.Size(91, 23);
            this.stepsNumericUpDown.TabIndex = 13;
            this.stepsNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.stepsNumericUpDown.ValueChanged += new System.EventHandler(this.stepsNumericUpDown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 413);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 14;
            this.label2.Text = "Steps Per Cycle";
            // 
            // invertCheckBox
            // 
            this.invertCheckBox.AutoSize = true;
            this.invertCheckBox.Location = new System.Drawing.Point(147, 12);
            this.invertCheckBox.Name = "invertCheckBox";
            this.invertCheckBox.Size = new System.Drawing.Size(93, 19);
            this.invertCheckBox.TabIndex = 15;
            this.invertCheckBox.Text = "Invert Colors";
            this.invertCheckBox.UseVisualStyleBackColor = true;
            this.invertCheckBox.CheckedChanged += new System.EventHandler(this.invertCheckBox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Rule";
            // 
            // customRuleTextBox
            // 
            this.customRuleTextBox.Location = new System.Drawing.Point(93, 198);
            this.customRuleTextBox.Name = "customRuleTextBox";
            this.customRuleTextBox.Size = new System.Drawing.Size(134, 23);
            this.customRuleTextBox.TabIndex = 17;
            // 
            // randomRuleButton
            // 
            this.randomRuleButton.Location = new System.Drawing.Point(12, 198);
            this.randomRuleButton.Name = "randomRuleButton";
            this.randomRuleButton.Size = new System.Drawing.Size(75, 23);
            this.randomRuleButton.TabIndex = 18;
            this.randomRuleButton.Text = "Random";
            this.randomRuleButton.UseVisualStyleBackColor = true;
            this.randomRuleButton.Click += new System.EventHandler(this.randomRuleButton_Click);
            // 
            // applyRuleButton
            // 
            this.applyRuleButton.Location = new System.Drawing.Point(115, 227);
            this.applyRuleButton.Name = "applyRuleButton";
            this.applyRuleButton.Size = new System.Drawing.Size(75, 23);
            this.applyRuleButton.TabIndex = 19;
            this.applyRuleButton.Text = "Apply";
            this.applyRuleButton.UseVisualStyleBackColor = true;
            this.applyRuleButton.Click += new System.EventHandler(this.applyRuleButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 756);
            this.Controls.Add(this.applyRuleButton);
            this.Controls.Add(this.randomRuleButton);
            this.Controls.Add(this.customRuleTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.invertCheckBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.stepsNumericUpDown);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colsTextBox);
            this.Controls.Add(this.rowsTextBox);
            this.Controls.Add(this.useOpenCLCheckBox);
            this.Controls.Add(this.ruleComboBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.stepButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.numAliveLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.randomizeButton);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stepsNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox pictureBox;
        private Button startButton;
        private Button randomizeButton;
        private Button clearButton;
        private Label numAliveLabel;
        private Button loadButton;
        private Button stepButton;
        private ComboBox ruleComboBox;
        private CheckBox useOpenCLCheckBox;
        private TextBox rowsTextBox;
        private TextBox colsTextBox;
        private Label label1;
        private Button applyButton;
        private NumericUpDown stepsNumericUpDown;
        private Label label2;
        private CheckBox invertCheckBox;
        private Label label3;
        private TextBox customRuleTextBox;
        private Button randomRuleButton;
        private Button applyRuleButton;
    }
}