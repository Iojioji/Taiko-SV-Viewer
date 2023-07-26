
namespace TaikoMapSVViewer
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripFile = new System.Windows.Forms.ToolStripMenuItem();
            this.loadBeatmapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateStripButt = new System.Windows.Forms.ToolStripMenuItem();
            this.SVChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BeatmapUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.OsuRunningTimer = new System.Windows.Forms.Timer(this.components);
            this.OsuRunningLabel = new System.Windows.Forms.Label();
            this.BeatmapUpdateLabel = new System.Windows.Forms.Label();
            this.InMapSelect = new System.Windows.Forms.Label();
            this.AutoUpdateMapCheckbox = new System.Windows.Forms.CheckBox();
            this.AutoUpdateModCheckbox = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.modLbl = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SVChart)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripFile,
            this.toolStripRefresh,
            this.settingsToolStripMenuItem,
            this.updateStripButt});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(875, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripFile
            // 
            this.toolStripFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadBeatmapToolStripMenuItem});
            this.toolStripFile.Name = "toolStripFile";
            this.toolStripFile.Size = new System.Drawing.Size(37, 20);
            this.toolStripFile.Text = "File";
            // 
            // loadBeatmapToolStripMenuItem
            // 
            this.loadBeatmapToolStripMenuItem.Name = "loadBeatmapToolStripMenuItem";
            this.loadBeatmapToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.loadBeatmapToolStripMenuItem.Text = "Load Beatmap";
            this.loadBeatmapToolStripMenuItem.Click += new System.EventHandler(this.LoadBeatmap_Click);
            // 
            // toolStripRefresh
            // 
            this.toolStripRefresh.Enabled = false;
            this.toolStripRefresh.Name = "toolStripRefresh";
            this.toolStripRefresh.Size = new System.Drawing.Size(58, 20);
            this.toolStripRefresh.Text = "Refresh";
            this.toolStripRefresh.Click += new System.EventHandler(this.toolStripRefresh_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Enabled = false;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // updateStripButt
            // 
            this.updateStripButt.Name = "updateStripButt";
            this.updateStripButt.Size = new System.Drawing.Size(115, 20);
            this.updateStripButt.Text = "Check for updates";
            this.updateStripButt.Click += new System.EventHandler(this.CheckUpdate_Click);
            // 
            // SVChart
            // 
            this.SVChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.SVChart.ChartAreas.Add(chartArea1);
            this.SVChart.Location = new System.Drawing.Point(12, 41);
            this.SVChart.Name = "SVChart";
            this.SVChart.Size = new System.Drawing.Size(851, 453);
            this.SVChart.TabIndex = 1;
            this.SVChart.Text = "chart1";
            this.SVChart.SelectionRangeChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.CursorEventArgs>(this.SVChartSelectionRangeChanged);
            this.SVChart.AxisViewChanged += new System.EventHandler<System.Windows.Forms.DataVisualization.Charting.ViewEventArgs>(this.SVChartAxisChanged);
            // 
            // BeatmapUpdateTimer
            // 
            this.BeatmapUpdateTimer.Interval = 20;
            this.BeatmapUpdateTimer.Tick += new System.EventHandler(this.BeatmapUpdateTimer_Tick);
            // 
            // OsuRunningTimer
            // 
            this.OsuRunningTimer.Interval = 500;
            this.OsuRunningTimer.Tick += new System.EventHandler(this.OsuRunningTimer_Tick);
            // 
            // OsuRunningLabel
            // 
            this.OsuRunningLabel.Location = new System.Drawing.Point(362, 25);
            this.OsuRunningLabel.Name = "OsuRunningLabel";
            this.OsuRunningLabel.Size = new System.Drawing.Size(115, 13);
            this.OsuRunningLabel.TabIndex = 2;
            this.OsuRunningLabel.Text = "OsuRunning: False";
            this.OsuRunningLabel.Visible = false;
            // 
            // BeatmapUpdateLabel
            // 
            this.BeatmapUpdateLabel.Location = new System.Drawing.Point(604, 25);
            this.BeatmapUpdateLabel.Name = "BeatmapUpdateLabel";
            this.BeatmapUpdateLabel.Size = new System.Drawing.Size(257, 13);
            this.BeatmapUpdateLabel.TabIndex = 3;
            this.BeatmapUpdateLabel.Text = "BeatmapUpdate: False";
            this.BeatmapUpdateLabel.Visible = false;
            // 
            // InMapSelect
            // 
            this.InMapSelect.Location = new System.Drawing.Point(483, 25);
            this.InMapSelect.Name = "InMapSelect";
            this.InMapSelect.Size = new System.Drawing.Size(115, 13);
            this.InMapSelect.TabIndex = 4;
            this.InMapSelect.Text = "SelectingMap: False";
            this.InMapSelect.Visible = false;
            // 
            // AutoUpdateMapCheckbox
            // 
            this.AutoUpdateMapCheckbox.AutoSize = true;
            this.AutoUpdateMapCheckbox.Location = new System.Drawing.Point(15, 24);
            this.AutoUpdateMapCheckbox.Name = "AutoUpdateMapCheckbox";
            this.AutoUpdateMapCheckbox.Size = new System.Drawing.Size(155, 17);
            this.AutoUpdateMapCheckbox.TabIndex = 5;
            this.AutoUpdateMapCheckbox.Text = "Auto Update Selected Map";
            this.AutoUpdateMapCheckbox.UseVisualStyleBackColor = true;
            this.AutoUpdateMapCheckbox.CheckedChanged += new System.EventHandler(this.AutoUpdateCheckbox_CheckedChanged);
            // 
            // AutoUpdateModCheckbox
            // 
            this.AutoUpdateModCheckbox.AutoSize = true;
            this.AutoUpdateModCheckbox.Location = new System.Drawing.Point(327, 5);
            this.AutoUpdateModCheckbox.Name = "AutoUpdateModCheckbox";
            this.AutoUpdateModCheckbox.Size = new System.Drawing.Size(110, 17);
            this.AutoUpdateModCheckbox.TabIndex = 5;
            this.AutoUpdateModCheckbox.Text = "Auto Update Mod";
            this.AutoUpdateModCheckbox.UseVisualStyleBackColor = true;
            this.AutoUpdateModCheckbox.CheckedChanged += new System.EventHandler(this.AutoUpdateModCheckbox_CheckedChanged);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "NM",
            "HR",
            "EZ",
            "AutoUpdate"});
            this.comboBox1.Location = new System.Drawing.Point(224, 21);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(74, 20);
            this.comboBox1.TabIndex = 6;
            // 
            // modLbl
            // 
            this.modLbl.AutoSize = true;
            this.modLbl.Location = new System.Drawing.Point(190, 24);
            this.modLbl.Name = "modLbl";
            this.modLbl.Size = new System.Drawing.Size(28, 13);
            this.modLbl.TabIndex = 7;
            this.modLbl.Text = "Mod";
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(875, 506);
            this.Controls.Add(this.modLbl);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.AutoUpdateModCheckbox);
            this.Controls.Add(this.AutoUpdateMapCheckbox);
            this.Controls.Add(this.InMapSelect);
            this.Controls.Add(this.BeatmapUpdateLabel);
            this.Controls.Add(this.OsuRunningLabel);
            this.Controls.Add(this.SVChart);
            this.Controls.Add(this.menuStrip1);
            this.KeyPreview = true;
            this.Location = new System.Drawing.Point(2000, 0);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Taiko SV Viewer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.LoadMap_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.LoadMap_DrageEnter);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SVChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripFile;
        private System.Windows.Forms.ToolStripMenuItem loadBeatmapToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart SVChart;
        private System.Windows.Forms.ToolStripMenuItem toolStripRefresh;
        private System.Windows.Forms.ToolStripMenuItem updateStripButt;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.Timer BeatmapUpdateTimer;
        private System.Windows.Forms.Timer OsuRunningTimer;
        private System.Windows.Forms.Label OsuRunningLabel;
        private System.Windows.Forms.Label BeatmapUpdateLabel;
        private System.Windows.Forms.Label InMapSelect;
        private System.Windows.Forms.CheckBox AutoUpdateMapCheckbox;
        private System.Windows.Forms.CheckBox AutoUpdateModCheckbox;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label modLbl;
    }
}

