namespace ChordsDesktop
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.progressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.playButton = new System.Windows.Forms.Button();
            this.pauseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.doAutoScroll = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.recalculateButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bigChordLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.correctedChordTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 672);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 30);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1199, 32);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 3;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(14, 46);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1201, 477);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1132, 33);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenu});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(54, 29);
            this.toolStripMenuItem1.Text = "File";
            // 
            // openFileToolStripMenu
            // 
            this.openFileToolStripMenu.Name = "openFileToolStripMenu";
            this.openFileToolStripMenu.Size = new System.Drawing.Size(170, 34);
            this.openFileToolStripMenu.Text = "Open...";
            this.openFileToolStripMenu.Click += new System.EventHandler(this.OpenFileClicked);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(166, 29);
            this.toolStripMenuItem2.Text = "Machine Learning";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(242, 34);
            this.toolStripMenuItem3.Text = "Change Model...";
            // 
            // progressLabel
            // 
            this.progressLabel.AutoSize = true;
            this.progressLabel.BackColor = System.Drawing.Color.Transparent;
            this.progressLabel.Location = new System.Drawing.Point(12, 638);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(209, 25);
            this.progressLabel.TabIndex = 6;
            this.progressLabel.Text = "Audio file not loaded yet";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 606);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(209, 25);
            this.label1.TabIndex = 7;
            this.label1.Text = "Please open an audio file";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.playButton);
            this.flowLayoutPanel2.Controls.Add(this.pauseButton);
            this.flowLayoutPanel2.Controls.Add(this.stopButton);
            this.flowLayoutPanel2.Controls.Add(this.doAutoScroll);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(15, 537);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(661, 49);
            this.flowLayoutPanel2.TabIndex = 8;
            // 
            // playButton
            // 
            this.playButton.Location = new System.Drawing.Point(2, 2);
            this.playButton.Margin = new System.Windows.Forms.Padding(2);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(165, 41);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // pauseButton
            // 
            this.pauseButton.Location = new System.Drawing.Point(171, 2);
            this.pauseButton.Margin = new System.Windows.Forms.Padding(2);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(165, 41);
            this.pauseButton.TabIndex = 0;
            this.pauseButton.Text = "Pause";
            this.pauseButton.UseVisualStyleBackColor = true;
            this.pauseButton.Click += new System.EventHandler(this.PauseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(340, 2);
            this.stopButton.Margin = new System.Windows.Forms.Padding(2);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(165, 41);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Stop";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // doAutoScroll
            // 
            this.doAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.doAutoScroll.AutoSize = true;
            this.doAutoScroll.Location = new System.Drawing.Point(518, 2);
            this.doAutoScroll.Margin = new System.Windows.Forms.Padding(11, 2, 2, 2);
            this.doAutoScroll.Name = "doAutoScroll";
            this.doAutoScroll.Size = new System.Drawing.Size(125, 41);
            this.doAutoScroll.TabIndex = 1;
            this.doAutoScroll.Text = "Auto Scroll";
            this.doAutoScroll.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.recalculateButton);
            this.flowLayoutPanel3.Controls.Add(this.label2);
            this.flowLayoutPanel3.Controls.Add(this.numericUpDown1);
            this.flowLayoutPanel3.Controls.Add(this.label3);
            this.flowLayoutPanel3.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel3.Location = new System.Drawing.Point(680, 537);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(534, 49);
            this.flowLayoutPanel3.TabIndex = 8;
            // 
            // recalculateButton
            // 
            this.recalculateButton.Location = new System.Drawing.Point(365, 3);
            this.recalculateButton.Name = "recalculateButton";
            this.recalculateButton.Size = new System.Drawing.Size(166, 40);
            this.recalculateButton.TabIndex = 0;
            this.recalculateButton.Text = "Recalculate";
            this.recalculateButton.UseVisualStyleBackColor = true;
            this.recalculateButton.Click += new System.EventHandler(this.RecalculateButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "ms";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(196, 6);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(121, 31);
            this.numericUpDown1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(184, 25);
            this.label3.TabIndex = 3;
            this.label3.Text = "Time Window Length:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.bigChordLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.correctedChordTextBox, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1222, 47);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 18, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(318, 475);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // bigChordLabel
            // 
            this.bigChordLabel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.bigChordLabel, 2);
            this.bigChordLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bigChordLabel.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bigChordLabel.Location = new System.Drawing.Point(3, 49);
            this.bigChordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.bigChordLabel.Name = "bigChordLabel";
            this.bigChordLabel.Size = new System.Drawing.Size(312, 376);
            this.bigChordLabel.TabIndex = 1;
            this.bigChordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label4, 2);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 1);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(312, 47);
            this.label4.TabIndex = 0;
            this.label4.Text = "Selected Chord";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(204, 428);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 44);
            this.button1.TabIndex = 2;
            this.button1.Text = "Correct";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CorrectChordButton_Click);
            // 
            // correctedChordTextBox
            // 
            this.correctedChordTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.correctedChordTextBox.Location = new System.Drawing.Point(3, 435);
            this.correctedChordTextBox.Margin = new System.Windows.Forms.Padding(2, 9, 2, 2);
            this.correctedChordTextBox.Name = "correctedChordTextBox";
            this.correctedChordTextBox.Size = new System.Drawing.Size(153, 31);
            this.correctedChordTextBox.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1132, 640);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.menuStrip1);
            this.Location = new System.Drawing.Point(100, 100);
            this.Name = "Form1";
            this.Text = "Chords - Machine Learning detection of guitar chords";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenu;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.Button pauseButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.CheckBox doAutoScroll;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Button recalculateButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label bigChordLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox correctedChordTextBox;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
    }
}

