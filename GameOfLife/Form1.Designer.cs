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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(189, 6);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(618, 607);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
            this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
            this.pictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.pictureBox_PreviewKeyDown);
            // 
            // startButton
            // 
            this.startButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.startButton.Location = new System.Drawing.Point(446, 641);
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
            this.randomizeButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.randomizeButton.Location = new System.Drawing.Point(325, 641);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.Size = new System.Drawing.Size(91, 37);
            this.randomizeButton.TabIndex = 2;
            this.randomizeButton.TabStop = false;
            this.randomizeButton.Text = "Randomize";
            this.randomizeButton.UseVisualStyleBackColor = true;
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // clearButton
            // 
            this.clearButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.clearButton.Location = new System.Drawing.Point(716, 641);
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
            this.numAliveLabel.Location = new System.Drawing.Point(189, 616);
            this.numAliveLabel.Name = "numAliveLabel";
            this.numAliveLabel.Size = new System.Drawing.Size(42, 15);
            this.numAliveLabel.TabIndex = 4;
            this.numAliveLabel.Text = "Living:";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.loadButton.Location = new System.Drawing.Point(35, 230);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(91, 37);
            this.loadButton.TabIndex = 5;
            this.loadButton.TabStop = false;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // stepButton
            // 
            this.stepButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.stepButton.Location = new System.Drawing.Point(543, 641);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(91, 37);
            this.stepButton.TabIndex = 6;
            this.stepButton.TabStop = false;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 690);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.stepButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.numAliveLabel);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.randomizeButton);
            this.Controls.Add(this.startButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.Form1_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
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
    }
}