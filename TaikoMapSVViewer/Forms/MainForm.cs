﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Beatmaps.Sections;
using OsuMemoryDataProvider;
using System.Windows.Forms.DataVisualization.Charting;
using TaikoMapSVViewer.Data.ChartData;
using TaikoMapSVViewer.Data.MemoryReader;
using System.Reflection;
using System.Diagnostics;
using TaikoMapSVViewer.Settings;
using AutoUpdaterDotNET;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace TaikoMapSVViewer
{
    public enum SVMod { AutoUpdate = 0, NM = 1, HD = 2, HR = 3 }
    public partial class MainForm : Form
    {
        Beatmap currentBeatmap;

        string userSongsFolder = null;

        MemoryReaderManager osuReader;

        bool hooked = false;
        bool? gameLoaded = false;
        bool inMapSelectScreen = false;

        ConvertedTimingPoint baseCTP = new ConvertedTimingPoint();

        List<ChartSection> chartSections = new List<ChartSection>();

        Color controlColor;

        string currentLoadedBeatmap = "";

        string currentMod = "NM";

        //int numberOfZoom = 0;
        int maxMarkerSize = 30;

        bool saveToSettings = false;

        bool HasBeatmap
        {
            get { return currentBeatmap != null; }
        }
        public MainForm()
        {
            InitializeComponent();
            Initialize();

            Closing += OnClose;
            //SVChart.MouseWheel += SVChart_MouseWheel;
        }

        public void Initialize()
        {
            SettingsManager.Init();

            var ver = Assembly.GetExecutingAssembly().GetName().Version;

            SettingsManager.Version = ver.ToString();

            LoadSettingsValues();

            saveToSettings = true;

            osuReader = new MemoryReaderManager(this, OnPathChanged, OnModChanged, OnAudioTimeChanged);
            osuReader.OnReadError += OnReadError;

            osuReader.StartReading();

            this.AllowDrop = true;
        }

        private void OnClose(object sender, CancelEventArgs e)
        {
            osuReader.StopReading();
        }

        public void Reset()
        {

        }

        void LoadSettingsValues()
        {
            AutoUpdateMapCheckbox.Checked = SettingsManager.AutoUpdateSelectedMap;
            SVModDropList.SelectedIndex = SettingsManager.SVMod;
        }

        void ParseBeatmap(string beatmapPath)
        {
            try
            {
                Console.WriteLine($"Gonna check this map lmao: '{beatmapPath}'");
                currentBeatmap = BeatmapDecoder.Decode(beatmapPath);
                baseCTP = new ConvertedTimingPoint();

                LoadConvertedTimingPoints(currentBeatmap);
                LoadHitObjects(currentBeatmap);

                //double minVal = Math.Round(baseCTP.GetLowestBPMSV());
                //double maxVal = Math.Round(baseCTP.GetHighestBPMSV());

                SVChart.Series.Clear();

                double baseSliderMultiplier = baseCTP.SliderMultiplier;
                double sliderModMultiplier = currentMod == "HR" ? (1.4 * 4 / 3) : currentMod == "EZ" ? 0.8 : 1;

                baseCTP.SliderMultiplier = baseSliderMultiplier * sliderModMultiplier;
                if (sliderModMultiplier != 1)
                {
                    foreach (ChartSection section in chartSections)
                    {
                        section.UpdateSV(baseCTP);
                    }
                }

                double minVal = GetLowestSV();
                double maxVal = GetHighestSV();

                int lastObject = chartSections.Last().GetLastMilli();

                //DrawChart(Color.DarkGreen, Color.FromArgb(255, 106, 0), currentMod);
                DrawChart(SettingsManager.Colors.GraphLineColor, SettingsManager.Colors.GraphLineColorKiai, currentMod);

                SetupChart(minVal, maxVal, lastObject);

                SetWindowTitle(currentBeatmap.MetadataSection);
                currentLoadedBeatmap = beatmapPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Please send this to Iojioji (along with the exact map that caused it [{beatmapPath}])\r\n\r\n- - - - - - - - - - - - - -\r\n\r\n{ex.Message}", $"An unexpected error has occurred!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                currentLoadedBeatmap = "";
            }
            UpdateRefreshButton();
        }
        void LoadConvertedTimingPoints(Beatmap toLoad)
        {
            baseCTP.SliderMultiplier = toLoad.DifficultySection.SliderMultiplier;
            foreach (TimingPoint tp in toLoad.TimingPoints)
            {
                baseCTP.AddTimingPoint(tp);
            }
        }
        void LoadHitObjects(Beatmap toLoad)
        {
            chartSections.Clear();
            bool previousNoteWasKiai = false;

            for (int i = 0; i < toLoad.HitObjects.Count; i++)
            {
                HitObject ho = toLoad.HitObjects[i];
                bool isKiai = baseCTP.IsNoteInKiai(ho);

                ///If there are no chartSections, create one.
                ///     You gotta check if the first timing point starts with a kiai
                ///     so gotta wait on that one till the first object is gotten
                ///

                if (chartSections.Count == 0)
                {
                    chartSections.Add(new ChartSection(isKiai));
                }
                else if (isKiai != previousNoteWasKiai)
                {
                    chartSections[chartSections.Count - 1].AddObject(baseCTP.GetNoteAdjustedBPM(ho.StartTime), ho.StartTime);
                    chartSections.Add(new ChartSection(isKiai));
                }
                chartSections[chartSections.Count - 1].AddObject(baseCTP.GetNoteAdjustedBPM(ho.StartTime), ho.StartTime);

                previousNoteWasKiai = isKiai;
            }
        }
        List<double> MillisToSeconds(List<int> millis)
        {
            List<double> seconds = new List<double>();

            foreach (int milli in millis)
            {
                //seconds.Add(Math.Round(milli / 1000.0, 5));
                seconds.Add(milli / 1000.0);
            }

            return seconds;
        }

        void DrawChart(Color normalColor, Color kiaiColor, string seriesName)
        {
            for (int i = 0; i < chartSections.Count; i++)
            {
                ChartSection section = chartSections[i];

                var series = new Series($"SVs-{seriesName}{i}");

                series.Points.DataBindXY(MillisToSeconds(section.GetMillis()), section.GetSVs());
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 1;
                series.BorderColor = Color.Gray;

                //TODO: Load markerstyle and line color from settings
                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 0;
                series.MarkerColor = section.IsKiai ? kiaiColor : normalColor;
                //series.Color = section.IsKiai ? Color.Orange : Color.DarkGreen;
                series.Color = section.IsKiai ? kiaiColor : normalColor;
                SVChart.Series.Add(series);
            }
        }
        void SetupChart(double minVal, double maxVal, int lastObject)
        {
            SVChart.ChartAreas[0].AxisX.Minimum = 0;
            //SVChart.ChartAreas[0].AxisX.Maximum = Math.Round(ctp.GetLastOffset() * 1.02 / 1000.0);
            SVChart.ChartAreas[0].AxisX.Maximum = System.Math.Round(lastObject * 1.02 / 1000.0);

            SVChart.ChartAreas[0].AxisY.Minimum = minVal - 10;
            SVChart.ChartAreas[0].AxisY.Maximum = maxVal + 10;
            SVChart.ChartAreas[0].AxisY.Interval = (int)System.Math.Round((maxVal - minVal) / 10);
            double lastObejctSeconds = lastObject / 1000;
            SVChart.ChartAreas[0].AxisX.Interval = lastObejctSeconds / 5 >= 10 ? (lastObejctSeconds / 10 >= 10 ? (lastObejctSeconds / 15 >= 10 ? (lastObejctSeconds / 20 >= 10 ? 30 : 20) : 15) : 10) : 5;


            SVChart.ChartAreas[0].AxisX.LineColor = Color.Black;
            SVChart.ChartAreas[0].AxisY.LineColor = Color.Black;

            SVChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.FromArgb(126, 126, 126, 126);
            SVChart.ChartAreas[0].AxisX.MinorGrid.LineColor = Color.FromArgb(126, 126, 126, 126);
            SVChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.FromArgb(126, 126, 126, 126);
            SVChart.ChartAreas[0].AxisY.MinorGrid.LineColor = Color.FromArgb(126, 126, 126, 126);

            SVChart.ChartAreas[0].AxisX.Name = "Seconds";
            SVChart.ChartAreas[0].AxisY.Name = "BPM SV";

            SVChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            SVChart.ChartAreas[0].CursorX.AutoScroll = true;
            SVChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;

            SVChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            SVChart.ChartAreas[0].CursorY.AutoScroll = true;
            SVChart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            foreach (Series series in SVChart.Series)
            {
                series.ToolTip = $"{"#VALY":F2} BPM, {"#VALX":F2} seconds";
            }
            //SVChart.Series[0].ToolTip = $"{"#VALY":F2} BPM, {"#VALX":F2} seconds";
        }
        double GetLowestSV()
        {
            double result = -1;
            if (chartSections.Count > 0)
            {
                foreach (ChartSection section in chartSections)
                {
                    double sectionLowestSV = section.GetLowestSV();
                    result = result == -1 ? sectionLowestSV : (result > sectionLowestSV ? sectionLowestSV : result);
                }
            }
            return System.Math.Round(result);
        }
        double GetHighestSV()
        {
            double result = -1;
            if (chartSections.Count > 0)
            {
                int index = 0;
                foreach (ChartSection section in chartSections)
                {
                    Console.WriteLine($"Checkign section {index}");
                    double sectionHighestSV = section.GetHighestSV();
                    result = result == -1 ? sectionHighestSV : (result < sectionHighestSV ? sectionHighestSV : result);
                    index++;
                }
            }
            return System.Math.Round(result);
        }
        void SetChartMarkerSize(int size)
        {
            if (SVChart.Series.Count == 0)
            {
                Console.WriteLine($"Can't change series' size, you HAVE no series lmao");
                return;
            }
            for (int i = 0; i < SVChart.Series.Count; i++)
            {
                Console.WriteLine($"ChangedMarkerSize! '{SVChart.Series[i].MarkerSize}' => '{size}'");
                SVChart.Series[i].MarkerSize = size;
            }
        }


        void SetWindowTitle(BeatmapMetadataSection data)
        {
            Text = $"Checking: {data.Title} [{data.Version}] by {data.Creator}";
        }
        string PrintTimingPoints(Beatmap toPrint)
        {
            string aux = "";
            foreach (TimingPoint tp in toPrint.TimingPoints)
            {

            }
            return aux;
        }
        void UpdateRefreshButton()
        {

            if (string.IsNullOrEmpty(currentLoadedBeatmap) || !File.Exists(currentLoadedBeatmap))
            {
                toolStripRefresh.Enabled = false;
                return;
            }

            toolStripRefresh.Enabled = true;
        }
        public void RefreshBeatmap()
        {
            if (string.IsNullOrEmpty(currentLoadedBeatmap))
            {
                //You've got no map already loaded lmao.
                return;
            }

            if (!File.Exists(currentLoadedBeatmap))
            {
                //Uuuh, file no longer exists.
                MessageBox.Show("Couldn't find that beatmap, did you deleted/moved/renamed it?\r\nTry opening it again instead of refreshing", "Uh oh...");
                return;
            }

            Console.WriteLine($"Refreshing beatmap let's gooo");
            ParseBeatmap(currentLoadedBeatmap);
        }

        public void SetSongsFolder()
        {
            var processes = Process.GetProcessesByName("osu!");

            if (processes.Length == 0)
            {
                Debug.WriteLine($"No osu process was found unu");
            }
            else
            {
                //if (processes.Length == 2)
                //{

                //}
                //else if (processes)
                if (processes[0].MainModule.FileVersionInfo.ProductName.Contains("lazer"))
                {
                    string message = processes.Length > 1 ? $"You probably had osu!lazer and osu!opened at the same time, please close lazer and reopen it for the thing to work \r\n\r\n(just opening the osu!, then opening sv viewer, then opening osu!lazer should do the trick next time)\r\n\r\n\r\nPS: I know this is very annoying but I don't think I'll be fixing this because my brain is too smoll" : $"Hey that's osu!lazer and not osu!\r\nThis won't work with that for the forseeable future unu";
                    MessageBox.Show(message, $"Oh we did an oopsie!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (userSongsFolder == null || userSongsFolder == "")
                {
                    string osuExePath = "";

                    osuExePath = processes[0].MainModule.FileName;

                    //foreach (Process process in processes)
                    //{
                    //    if (process)
                    //    //try
                    //    //{
                    //    //    osuExePath = process.MainModule.FileName;
                    //    //    //break;
                    //    //}
                    //    //catch (Exception ex)
                    //    //{
                    //    //    continue;
                    //    //}
                    //}

                    if (string.IsNullOrWhiteSpace(osuExePath))
                    {
                        Debug.WriteLine("Yo wtf osuExePath was empty or whitespace");
                        gameLoaded = false;
                        return;
                    }

                    userSongsFolder = Path.Combine(Path.GetDirectoryName(osuExePath), "Songs");
                    SettingsManager.SongsFolder = userSongsFolder;
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                if (toolStripRefresh.Enabled)
                {
                    RefreshBeatmap();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region MemoryStuff
        public void OnPathChanged(string newBeatmap, string newFolder)
        {
            Debug.WriteLine($"File name changed! ({newBeatmap}, {newFolder})");
            if (!SettingsManager.AutoUpdateSelectedMap) return;

            var invalidChars = Path.GetInvalidPathChars();

            if (string.IsNullOrWhiteSpace(newBeatmap) || newBeatmap.Any(c => invalidChars.Contains(c)))
            {
                Debug.WriteLine($"Error: beatmap had invalid characters");
                return;
            }
            if (string.IsNullOrWhiteSpace(newFolder) || newFolder.Any(c => invalidChars.Contains(c)))
            {
                Debug.WriteLine($"Error: folder had invalid characters");
                return;
            }

            //if (previousLoadedBeatmap != null && previousLoadedBeatmap == newFileName) return;

            if (userSongsFolder == null)
            {
                SetSongsFolder();
                if (userSongsFolder == null)
                {
                    Debug.WriteLine($"Oh shit there are no user songs folders, lemme know where they at pls.");
                    return;
                }
            }

            string absoluteFileName = Path.Combine(userSongsFolder, newFolder.TrimEnd(), newBeatmap);
            if (!File.Exists(absoluteFileName))
            {
                Debug.WriteLine($"AbsoluteFile doesn't exist! ({absoluteFileName})");
            }

            if (InvokeRequired)
            {
                this.Invoke(new Action(() => ParseBeatmap(absoluteFileName)));
                return;
            }
            ParseBeatmap(absoluteFileName);
        }
        private void OnModChanged(ReaderMods newValue)
        {
            Debug.WriteLine($"Mod changed! ({newValue})");
            currentMod = newValue.ToString();

            if (InvokeRequired)
            {
                this.Invoke(new Action(() => RefreshBeatmap()));
                return;
            }
            RefreshBeatmap();
        }
        private void OnModChanged(string value)
        {
            currentMod = value;
            RefreshBeatmap();
        }
        private void OnAudioTimeChanged(int newValue)
        {
            Debug.WriteLine($"Time changed! ({newValue})");
        }

        public void OnReadError(string error)
        {
            Debug.WriteLine(error);
        }
        #endregion

        #region events
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (UpdateManager.HasUpdate())
            {
                updateStripButt.BackColor = Color.LightGreen;
            }
        }

        private void LoadBeatmap_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "osu beatmap files (*.osu)|*.osu";
                openFileDialog.FilterIndex = 0;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    ParseBeatmap(openFileDialog.FileName);
                }
            }
        }

        private void AutoUpdateCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (SettingsManager.AutoUpdateSelectedMap == AutoUpdateMapCheckbox.Checked)
                return;

            SettingsManager.AutoUpdateSelectedMap = AutoUpdateMapCheckbox.Checked;
        }
        private void SVModDropList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SettingsManager.SVMod == SVModDropList.SelectedIndex)
                return;
            SettingsManager.SVMod = SVModDropList.SelectedIndex;
            //TODO: Handle when reader is connected and shit.
            if (!osuReader.CanRead)
            {
                if (SettingsManager.SVMod != 0)
                {
                    OnModChanged(SVModDropList.SelectedItem.ToString());
                }
            }
        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            RefreshBeatmap();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Init(this);
            settingsForm.ShowDialog();
        }
        //private void SVChart_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    var chart = (Chart)sender;
        //    var xAxis = chart.ChartAreas[0].AxisX;
        //    var yAxis = chart.ChartAreas[0].AxisY;

        //    var xMin = xAxis.ScaleView.ViewMinimum;
        //    var xMax = xAxis.ScaleView.ViewMaximum;
        //    var yMin = yAxis.ScaleView.ViewMinimum;
        //    var yMax = yAxis.ScaleView.ViewMaximum;

        //    int intervalX = 1;
        //    int intervalY = 1;

        //    try
        //    {
        //        if (e.Delta < 0 && numberOfZoom > 0) // Scrolled down.
        //        {
        //            var posXStart = xAxis.PixelPositionToValue(e.Location.X) -  intervalX * 2 / Math.Pow(2, numberOfZoom);
        //            var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + intervalX * 2 / Math.Pow(2, numberOfZoom);
        //            var posYStart = yAxis.PixelPositionToValue(e.Location.Y) -  intervalY * 2 / Math.Pow(2, numberOfZoom);
        //            var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + intervalY * 2 / Math.Pow(2, numberOfZoom);

        //            if (posXStart < 0) posXStart = 0;
        //            if (posYStart < 0) posYStart = 0;
        //            if (posYFinish > yAxis.Maximum) posYFinish = yAxis.Maximum;
        //            if (posXFinish > xAxis.Maximum) posYFinish = xAxis.Maximum;
        //            xAxis.ScaleView.Zoom(posXStart, posXFinish);
        //            yAxis.ScaleView.Zoom(posYStart, posYFinish);
        //            numberOfZoom--;
        //        }
        //        else if (e.Delta < 0 && numberOfZoom == 0) //Last scrolled dowm
        //        {
        //            yAxis.ScaleView.ZoomReset();
        //            xAxis.ScaleView.ZoomReset();
        //        }
        //        else if (e.Delta > 0) // Scrolled up.
        //        {

        //            var posXStart = xAxis.PixelPositionToValue(e.Location.X) -  intervalX / Math.Pow(2, numberOfZoom);
        //            var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + intervalX / Math.Pow(2, numberOfZoom);
        //            var posYStart = yAxis.PixelPositionToValue(e.Location.Y) -  intervalY / Math.Pow(2, numberOfZoom);
        //            var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + intervalY / Math.Pow(2, numberOfZoom);

        //            xAxis.ScaleView.Zoom(posXStart, posXFinish);
        //            yAxis.ScaleView.Zoom(posYStart, posYFinish);
        //            numberOfZoom++;
        //        }

        //        if (numberOfZoom < 0) numberOfZoom = 0;
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Error");
        //    }
        //}

        private void SVChartAxisChanged(object sender, ViewEventArgs e)
        {
            if (SVChart.ChartAreas.Count == 0)
            {
                Console.WriteLine($"No charts in area thing!");
                return;
            }

            bool isXZoomed = SVChart.ChartAreas[0].AxisX.ScaleView.IsZoomed;
            bool isYZoomed = SVChart.ChartAreas[0].AxisY.ScaleView.IsZoomed;

            //if (SVChart.ChartAreas[0].CursorX.SelectionStart == double.NaN)
            //{
            //    Console.WriteLine($"CursorX SelectionStart was NaN!");
            //}

            double newXSelStart = isXZoomed ? SVChart.ChartAreas[0].CursorX.SelectionStart : -1;
            double newXSelEnd = isXZoomed ? SVChart.ChartAreas[0].CursorX.SelectionEnd : -1;

            double newYSelStart = isYZoomed ? SVChart.ChartAreas[0].CursorY.SelectionStart : -1;
            double newYSelEnd = isYZoomed ? SVChart.ChartAreas[0].CursorY.SelectionEnd : -1;

            double xAxisSize = isXZoomed ? SVChart.ChartAreas[0].AxisX.ScaleView.Size : 0;
            double yAxisSize = isYZoomed ? SVChart.ChartAreas[0].AxisY.ScaleView.Size : 0;

            Console.WriteLine($"New Selection!: ({isXZoomed}, {isYZoomed}) ({newXSelStart}, {newXSelEnd}), ({newYSelStart}, {newYSelEnd}) ---- ({xAxisSize}, {yAxisSize})");

            if (isXZoomed && !isYZoomed)
            {
                SetChartMarkerSize((int)(maxMarkerSize / xAxisSize));
            }
            else if (!isXZoomed && isYZoomed)
            {
                SetChartMarkerSize((int)(maxMarkerSize / yAxisSize));
            }
            else if (!isXZoomed && !isYZoomed)
            {
                //No zoom;
                SetChartMarkerSize(0);
            }
            else
            {
                int minSize = yAxisSize < xAxisSize ? (int)yAxisSize : (int)xAxisSize;
                SetChartMarkerSize((int)(maxMarkerSize / minSize));
            }
        }

        private void SVChartSelectionRangeChanged(object sender, CursorEventArgs e)
        {
            //Console.WriteLine($"Changed selection range!");
        }

        private void CheckUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateManager.HasUpdate())
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to update this magnificent piece of software and introduce some more bugs into it?", "Update Available", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        Console.WriteLine($"Starting autoupdater");
                        AutoUpdater.Start($"https://raw.githubusercontent.com/Iojioji/Taiko-SV-Viewer/main/AutoUpdater.xml");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Something went wrong, tell Iojioji to get his stuff together!\r\n\r\nMessage: {ex.Message}\r\n\r\nStackTrace: {ex.StackTrace}", "Error while updating!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show($"No updates pending; you're already up to date fam (v{SettingsManager.Version})");
            }
        }

        private void LoadMap_DrageEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }
        private void LoadMap_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                //TODO: Load all files dragged in
                if (files[0].Substring(System.Math.Max(0, files[0].Length - 4)) == ".osu")
                {
                    ParseBeatmap(files[0]);
                }
                else
                {
                    MessageBox.Show($"That doesn't look like an osu beatmap file; I can only read .osu files unu", $"That's not a beatmap lol");
                }
            }
        }
        #endregion

    }
}