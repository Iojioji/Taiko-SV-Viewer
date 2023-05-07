
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
            this.AutoUpdateCheckbox = new System.Windows.Forms.CheckBox();
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
            this.SVChart.Location = new System.Drawing.Point(12, 57);
            this.SVChart.Name = "SVChart";
            this.SVChart.Size = new System.Drawing.Size(851, 437);
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
            this.OsuRunningLabel.Enabled = false;
            this.OsuRunningLabel.Location = new System.Drawing.Point(289, 24);
            this.OsuRunningLabel.Name = "OsuRunningLabel";
            this.OsuRunningLabel.Size = new System.Drawing.Size(115, 13);
            this.OsuRunningLabel.TabIndex = 2;
            this.OsuRunningLabel.Text = "OsuRunning: False";
            this.OsuRunningLabel.Visible = false;
            // 
            // BeatmapUpdateLabel
            // 
            this.BeatmapUpdateLabel.Enabled = false;
            this.BeatmapUpdateLabel.Location = new System.Drawing.Point(12, 41);
            this.BeatmapUpdateLabel.Name = "BeatmapUpdateLabel";
            this.BeatmapUpdateLabel.Size = new System.Drawing.Size(851, 13);
            this.BeatmapUpdateLabel.TabIndex = 3;
            this.BeatmapUpdateLabel.Text = "BeatmapUpdate: False";
            this.BeatmapUpdateLabel.Visible = false;
            // 
            // InMapSelect
            // 
            this.InMapSelect.Enabled = false;
            this.InMapSelect.Location = new System.Drawing.Point(410, 24);
            this.InMapSelect.Name = "InMapSelect";
            this.InMapSelect.Size = new System.Drawing.Size(115, 13);
            this.InMapSelect.TabIndex = 4;
            this.InMapSelect.Text = "SelectingMap: False";
            this.InMapSelect.Visible = false;
            // 
            // AutoUpdateCheckbox
            // 
            this.AutoUpdateCheckbox.AutoSize = true;
            this.AutoUpdateCheckbox.Location = new System.Drawing.Point(15, 20);
            this.AutoUpdateCheckbox.Name = "AutoUpdateCheckbox";
            this.AutoUpdateCheckbox.Size = new System.Drawing.Size(155, 17);
            this.AutoUpdateCheckbox.TabIndex = 5;
            this.AutoUpdateCheckbox.Text = "Auto Update Selected Map";
            this.AutoUpdateCheckbox.UseVisualStyleBackColor = true;
            this.AutoUpdateCheckbox.CheckedChanged += new System.EventHandler(this.AutoUpdateCheckbox_CheckedChanged);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(875, 506);
            this.Controls.Add(this.AutoUpdateCheckbox);
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
        private System.Windows.Forms.CheckBox AutoUpdateCheckbox;
    }
}

