namespace RealtimeChart
{
    partial class RealChart
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
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnStart.Font = new System.Drawing.Font("隶书", 16.2F);
            this.btnStart.ForeColor = System.Drawing.Color.LightPink;
            this.btnStart.Location = new System.Drawing.Point(131, 72);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(162, 58);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "开始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chart1
            // 
            this.chart1.BackColor = System.Drawing.Color.Azure;
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.LeftRight;
            this.chart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chart1.Location = new System.Drawing.Point(16, 138);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1327, 487);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnStop.Font = new System.Drawing.Font("隶书", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.ForeColor = System.Drawing.Color.LightPink;
            this.btnStop.Location = new System.Drawing.Point(301, 13);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(157, 58);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnInit
            // 
            this.btnInit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnInit.Font = new System.Drawing.Font("隶书", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInit.ForeColor = System.Drawing.Color.LightPink;
            this.btnInit.Location = new System.Drawing.Point(131, 13);
            this.btnInit.Margin = new System.Windows.Forms.Padding(4);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(162, 58);
            this.btnInit.TabIndex = 0;
            this.btnInit.Text = "初始化";
            this.btnInit.UseVisualStyleBackColor = false;
            this.btnInit.UseWaitCursor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveBtn.Font = new System.Drawing.Font("隶书", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.saveBtn.ForeColor = System.Drawing.Color.LightPink;
            this.saveBtn.Location = new System.Drawing.Point(301, 72);
            this.saveBtn.Margin = new System.Windows.Forms.Padding(4);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(157, 58);
            this.saveBtn.TabIndex = 6;
            this.saveBtn.Text = "保存";
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Gainsboro;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.Font = new System.Drawing.Font("隶书", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(554, 26);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(680, 87);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "Result: ";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // RealChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1356, 627);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.btnStart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "RealChart";
            this.Text = "Network";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}