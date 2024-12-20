namespace Demo
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.comboPort = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.comboMode = new System.Windows.Forms.ToolStripComboBox();
            this.buttonIsFlipped = new System.Windows.Forms.ToolStripLabel();
            this.comboIsFlipped = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.textAngleOffset = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonStart = new System.Windows.Forms.ToolStripButton();
            this.buttonStop = new System.Windows.Forms.ToolStripButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelSPC = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelPPS = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackDisplayRange = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDisplayRange)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.comboPort,
            this.toolStripLabel2,
            this.comboMode,
            this.buttonIsFlipped,
            this.comboIsFlipped,
            this.toolStripLabel3,
            this.textAngleOffset,
            this.toolStripSeparator1,
            this.buttonStart,
            this.buttonStop});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStrip1.Size = new System.Drawing.Size(1205, 32);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(38, 25);
            this.toolStripLabel1.Text = "Port:";
            // 
            // comboPort
            // 
            this.comboPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPort.Name = "comboPort";
            this.comboPort.Size = new System.Drawing.Size(105, 28);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(51, 25);
            this.toolStripLabel2.Text = "Mode:";
            // 
            // comboMode
            // 
            this.comboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboMode.Name = "comboMode";
            this.comboMode.Size = new System.Drawing.Size(173, 28);
            // 
            // buttonIsFlipped
            // 
            this.buttonIsFlipped.Name = "buttonIsFlipped";
            this.buttonIsFlipped.Size = new System.Drawing.Size(62, 25);
            this.buttonIsFlipped.Text = "Flipped:";
            // 
            // comboIsFlipped
            // 
            this.comboIsFlipped.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIsFlipped.Items.AddRange(new object[] {
            "No",
            "Yes"});
            this.comboIsFlipped.Name = "comboIsFlipped";
            this.comboIsFlipped.Size = new System.Drawing.Size(99, 28);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(93, 25);
            this.toolStripLabel3.Text = "Angle offset:";
            // 
            // textAngleOffset
            // 
            this.textAngleOffset.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.textAngleOffset.Name = "textAngleOffset";
            this.textAngleOffset.Size = new System.Drawing.Size(65, 28);
            this.textAngleOffset.Text = "0";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Image = global::Demo.Properties.Resources.PlayHS;
            this.buttonStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(64, 25);
            this.buttonStart.Text = "&Start";
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Image = global::Demo.Properties.Resources.StopHS;
            this.buttonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(64, 25);
            this.buttonStop.Text = "Sto&p";
            this.buttonStop.Click += new System.EventHandler(this.ButtonStop_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 594);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1146, 6);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 32);
            this.pictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(1146, 685);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.labelSPC,
            this.toolStripStatusLabel3,
            this.labelPPS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 717);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1205, 26);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(146, 20);
            this.toolStripStatusLabel2.Text = "Scans per second:";
            // 
            // labelSPC
            // 
            this.labelSPC.AutoSize = false;
            this.labelSPC.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelSPC.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.labelSPC.Name = "labelSPC";
            this.labelSPC.Size = new System.Drawing.Size(50, 20);
            this.labelSPC.Text = "-";
            this.labelSPC.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(130, 20);
            this.toolStripStatusLabel3.Text = "Points per scan:";
            // 
            // labelPPS
            // 
            this.labelPPS.AutoSize = false;
            this.labelPPS.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.labelPPS.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
            this.labelPPS.Name = "labelPPS";
            this.labelPPS.Size = new System.Drawing.Size(50, 20);
            this.labelPPS.Text = "-";
            this.labelPPS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.trackDisplayRange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1146, 32);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(59, 685);
            this.panel1.TabIndex = 6;
            // 
            // trackDisplayRange
            // 
            this.trackDisplayRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackDisplayRange.Location = new System.Drawing.Point(0, 16);
            this.trackDisplayRange.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.trackDisplayRange.Maximum = 50;
            this.trackDisplayRange.Minimum = 1;
            this.trackDisplayRange.Name = "trackDisplayRange";
            this.trackDisplayRange.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackDisplayRange.Size = new System.Drawing.Size(59, 669);
            this.trackDisplayRange.TabIndex = 6;
            this.trackDisplayRange.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackDisplayRange.Value = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Range:";
            // 
            // logBox
            // 
            this.logBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.logBox.Location = new System.Drawing.Point(0, 600);
            this.logBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.logBox.Name = "logBox";
            this.logBox.Size = new System.Drawing.Size(1146, 117);
            this.logBox.TabIndex = 7;
            this.logBox.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 743);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RPLidar.NET demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackDisplayRange)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton buttonStart;
        private System.Windows.Forms.ToolStripButton buttonStop;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripComboBox comboPort;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox comboMode;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel labelSPC;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackDisplayRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel labelPPS;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox textAngleOffset;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel buttonIsFlipped;
        private System.Windows.Forms.ToolStripComboBox comboIsFlipped;
        private System.Windows.Forms.RichTextBox logBox;
    }
}

