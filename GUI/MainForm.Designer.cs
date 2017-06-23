namespace Recorder
{
    partial class MainForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Signals Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRecord = new System.Windows.Forms.Button();
            this.chart = new Accord.Controls.Wavechart();
            this.lbPosition = new System.Windows.Forms.Label();
            this.lbLength = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnIdentify = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelStatus = new System.Windows.Forms.Label();
            this.Testcases_select = new System.Windows.Forms.GroupBox();
            this.trackBar1 = new MetroFramework.Controls.MetroTrackBar();
            this.pruningToggle1 = new MetroFramework.Controls.MetroToggle();
            this.metroLabel1 = new System.Windows.Forms.Label();
            this.caseMilestone1 = new MetroFramework.Controls.MetroRadioButton();
            this.caseMilestone2 = new MetroFramework.Controls.MetroRadioButton();
            this.caseMilestone3 = new MetroFramework.Controls.MetroRadioButton();
            this.Testcase1_radioButton = new MetroFramework.Controls.MetroRadioButton();
            this.Testcase2_radioButton = new MetroFramework.Controls.MetroRadioButton();
            this.Testcase3_radioButton = new MetroFramework.Controls.MetroRadioButton();
            this.Testcases_select.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Wave files (*.wav)|*.wav";
            // 
            // btnStop
            // 
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnStop.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnStop.Location = new System.Drawing.Point(142, 195);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(55, 30);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "<";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRecord
            // 
            this.btnRecord.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRecord.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnRecord.Location = new System.Drawing.Point(264, 195);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(55, 30);
            this.btnRecord.TabIndex = 4;
            this.btnRecord.Text = "=";
            this.btnRecord.UseVisualStyleBackColor = true;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // chart
            // 
            this.chart.BackColor = System.Drawing.Color.Black;
            this.chart.ForeColor = System.Drawing.Color.DarkGreen;
            this.chart.Location = new System.Drawing.Point(98, 84);
            this.chart.Name = "chart";
            this.chart.RangeX = ((AForge.DoubleRange)(resources.GetObject("chart.RangeX")));
            this.chart.RangeY = ((AForge.DoubleRange)(resources.GetObject("chart.RangeY")));
            this.chart.SimpleMode = false;
            this.chart.Size = new System.Drawing.Size(143, 41);
            this.chart.TabIndex = 6;
            this.chart.Text = "chart1";
            // 
            // lbPosition
            // 
            this.lbPosition.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbPosition.Location = new System.Drawing.Point(20, 84);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(72, 41);
            this.lbPosition.TabIndex = 7;
            this.lbPosition.Text = "Position: 00.00 sec.";
            this.lbPosition.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLength
            // 
            this.lbLength.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbLength.Location = new System.Drawing.Point(247, 84);
            this.lbLength.Name = "lbLength";
            this.lbLength.Size = new System.Drawing.Size(72, 41);
            this.lbLength.TabIndex = 7;
            this.lbLength.Text = "Length: 00.00 sec.";
            this.lbLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            this.btnAdd.Enabled = false;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnAdd.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnAdd.Location = new System.Drawing.Point(20, 195);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(55, 30);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "a";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPlay.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnPlay.Location = new System.Drawing.Point(203, 195);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(55, 30);
            this.btnPlay.TabIndex = 4;
            this.btnPlay.Text = "4";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnIdentify
            // 
            this.btnIdentify.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnIdentify.Font = new System.Drawing.Font("Webdings", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnIdentify.Location = new System.Drawing.Point(81, 195);
            this.btnIdentify.Name = "btnIdentify";
            this.btnIdentify.Size = new System.Drawing.Size(55, 30);
            this.btnIdentify.TabIndex = 4;
            this.btnIdentify.Text = "s";
            this.btnIdentify.UseVisualStyleBackColor = true;
            this.btnIdentify.Click += new System.EventHandler(this.btnIdentify_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "wav";
            this.saveFileDialog1.FileName = "file.wav";
            this.saveFileDialog1.Filter = "Wave files|*.wav|All files|*.*";
            this.saveFileDialog1.Title = "Save wave file";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(17, 332);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(74, 13);
            this.labelStatus.TabIndex = 12;
            this.labelStatus.Text = "Status: Ready";
            // 
            // Testcases_select
            // 
            this.Testcases_select.Controls.Add(this.Testcase3_radioButton);
            this.Testcases_select.Controls.Add(this.Testcase2_radioButton);
            this.Testcases_select.Controls.Add(this.Testcase1_radioButton);
            this.Testcases_select.Controls.Add(this.caseMilestone3);
            this.Testcases_select.Controls.Add(this.caseMilestone2);
            this.Testcases_select.Controls.Add(this.caseMilestone1);
            this.Testcases_select.Location = new System.Drawing.Point(20, 231);
            this.Testcases_select.Name = "Testcases_select";
            this.Testcases_select.Size = new System.Drawing.Size(299, 69);
            this.Testcases_select.TabIndex = 15;
            this.Testcases_select.TabStop = false;
            this.Testcases_select.Text = "Testcase Selection:";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.Transparent;
            this.trackBar1.Location = new System.Drawing.Point(20, 145);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(299, 23);
            this.trackBar1.TabIndex = 16;
            this.trackBar1.Text = "Track Bar";
            this.trackBar1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // pruningToggle1
            // 
            this.pruningToggle1.AutoSize = true;
            this.pruningToggle1.Location = new System.Drawing.Point(56, 306);
            this.pruningToggle1.Name = "pruningToggle1";
            this.pruningToggle1.Size = new System.Drawing.Size(80, 17);
            this.pruningToggle1.TabIndex = 17;
            this.pruningToggle1.Text = "Off";
            this.pruningToggle1.UseVisualStyleBackColor = true;
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(17, 307);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(43, 13);
            this.metroLabel1.TabIndex = 18;
            this.metroLabel1.Text = "Pruning";
            // 
            // caseMilestone1
            // 
            this.caseMilestone1.AutoSize = true;
            this.caseMilestone1.Location = new System.Drawing.Point(6, 43);
            this.caseMilestone1.Name = "caseMilestone1";
            this.caseMilestone1.Size = new System.Drawing.Size(84, 15);
            this.caseMilestone1.TabIndex = 3;
            this.caseMilestone1.TabStop = true;
            this.caseMilestone1.Text = "Milestone 1";
            this.caseMilestone1.UseVisualStyleBackColor = true;
            // 
            // caseMilestone2
            // 
            this.caseMilestone2.AutoSize = true;
            this.caseMilestone2.Location = new System.Drawing.Point(109, 43);
            this.caseMilestone2.Name = "caseMilestone2";
            this.caseMilestone2.Size = new System.Drawing.Size(70, 15);
            this.caseMilestone2.TabIndex = 4;
            this.caseMilestone2.TabStop = true;
            this.caseMilestone2.Text = "1 Minute";
            this.caseMilestone2.UseVisualStyleBackColor = true;
            // 
            // caseMilestone3
            // 
            this.caseMilestone3.AutoSize = true;
            this.caseMilestone3.Location = new System.Drawing.Point(215, 43);
            this.caseMilestone3.Name = "caseMilestone3";
            this.caseMilestone3.Size = new System.Drawing.Size(70, 15);
            this.caseMilestone3.TabIndex = 5;
            this.caseMilestone3.TabStop = true;
            this.caseMilestone3.Text = "4 Minute";
            this.caseMilestone3.UseVisualStyleBackColor = true;
            // 
            // Testcase1_radioButton
            // 
            this.Testcase1_radioButton.AutoSize = true;
            this.Testcase1_radioButton.Location = new System.Drawing.Point(6, 20);
            this.Testcase1_radioButton.Name = "Testcase1_radioButton";
            this.Testcase1_radioButton.Size = new System.Drawing.Size(57, 15);
            this.Testcase1_radioButton.TabIndex = 19;
            this.Testcase1_radioButton.TabStop = true;
            this.Testcase1_radioButton.Text = "Case 1";
            this.Testcase1_radioButton.UseVisualStyleBackColor = true;
            // 
            // Testcase2_radioButton
            // 
            this.Testcase2_radioButton.AutoSize = true;
            this.Testcase2_radioButton.Location = new System.Drawing.Point(109, 20);
            this.Testcase2_radioButton.Name = "Testcase2_radioButton";
            this.Testcase2_radioButton.Size = new System.Drawing.Size(57, 15);
            this.Testcase2_radioButton.TabIndex = 19;
            this.Testcase2_radioButton.TabStop = true;
            this.Testcase2_radioButton.Text = "Case 2";
            this.Testcase2_radioButton.UseVisualStyleBackColor = true;
            // 
            // Testcase3_radioButton
            // 
            this.Testcase3_radioButton.AutoSize = true;
            this.Testcase3_radioButton.Location = new System.Drawing.Point(215, 20);
            this.Testcase3_radioButton.Name = "Testcase3_radioButton";
            this.Testcase3_radioButton.Size = new System.Drawing.Size(57, 15);
            this.Testcase3_radioButton.TabIndex = 19;
            this.Testcase3_radioButton.TabStop = true;
            this.Testcase3_radioButton.Text = "Case 3";
            this.Testcase3_radioButton.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BorderStyle = MetroFramework.Drawing.MetroBorderStyle.FixedSingle;
            this.ClientSize = new System.Drawing.Size(332, 365);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.pruningToggle1);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.Testcases_select);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.lbLength);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnIdentify);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.btnStop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Resizable = false;
            this.Style = MetroFramework.MetroColorStyle.Black;
            this.Text = "Speaker Identification";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
            this.Testcases_select.ResumeLayout(false);
            this.Testcases_select.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Accord.Controls.Wavechart chart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lbPosition;
        private System.Windows.Forms.Label lbLength;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnIdentify;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.GroupBox Testcases_select;
        private MetroFramework.Controls.MetroTrackBar trackBar1;
        private MetroFramework.Controls.MetroToggle pruningToggle1;
        private System.Windows.Forms.Label metroLabel1;
        private MetroFramework.Controls.MetroRadioButton Testcase3_radioButton;
        private MetroFramework.Controls.MetroRadioButton Testcase2_radioButton;
        private MetroFramework.Controls.MetroRadioButton Testcase1_radioButton;
        private MetroFramework.Controls.MetroRadioButton caseMilestone3;
        private MetroFramework.Controls.MetroRadioButton caseMilestone2;
        private MetroFramework.Controls.MetroRadioButton caseMilestone1;
    }
}
