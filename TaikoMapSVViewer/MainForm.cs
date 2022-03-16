using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Beatmaps.Sections;
using OsuParsers.Beatmaps.Objects.Taiko;
using OsuParsers.Enums.Beatmaps;
using System.Windows.Forms.DataVisualization.Charting;
using TaikoMapSVViewer.Data.ChartData;
using System.Reflection;
using System.Diagnostics;
using TaikoMapSVViewer.Settings;
using AutoUpdaterDotNET;

namespace TaikoMapSVViewer
{
    public partial class MainForm : Form
    {
        Beatmap currentBeatmap;
        ConvertedTimingPoint ctp = new ConvertedTimingPoint();

        List<ChartSection> chartSections = new List<ChartSection>();

        string currentLoadedBeatmap = "";
        //int numberOfZoom = 0;
        int maxMarkerSize = 30;
        bool hasBeatmap
        {
            get { return currentBeatmap != null; }
        }
        public MainForm()
        {
            //Rectangle thing = new Rectangle(Screen.AllScreens[1].WorkingArea.Location, new Size(Screen.AllScreens[1].WorkingArea.Width, Screen.AllScreens[1].WorkingArea.Height));
            //Location = Screen.AllScreens[1].WorkingArea.Location;
            //Location = thing.Location;
            //Location = new Point((int)(Screen.AllScreens[1].WorkingArea.Location.X * 1.25), (int)(Screen.AllScreens[1].WorkingArea.Location.Y * 1.25));
            //Location.Offset(-this.Width * 3, -this.Height * 3);
            InitializeComponent();
            //SVChart.MouseWheel += SVChart_MouseWheel;
        }

        public void Initialize()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string assemblyVersion = fvi.FileVersion;

            SettingsManager.Version = assemblyVersion;
        }
        public void Reset()
        {

        }

        void ParseBeatmap(string beatmapPath)
        {
            Console.WriteLine($"Gonna check this map lmao: '{beatmapPath}'");
            currentBeatmap = BeatmapDecoder.Decode(beatmapPath);
            ctp = new ConvertedTimingPoint();
            LoadConvertedTimingPoints(currentBeatmap);
            LoadHitObjects(currentBeatmap);
            DrawChart();
            SetWindowTitle(currentBeatmap.MetadataSection);
        }
        void LoadConvertedTimingPoints(Beatmap toLoad)
        {
            ctp.SliderMultiplier = toLoad.DifficultySection.SliderMultiplier;
            foreach (TimingPoint tp in toLoad.TimingPoints)
            {
                ctp.AddTimingPoint(tp);
            }
            Console.WriteLine("Lmao");
        }
        void LoadHitObjects(Beatmap toLoad)
        {
            chartSections.Clear();
            bool previousNoteWasKiai = false;

            for (int i = 0; i < toLoad.HitObjects.Count; i++)
            {
                HitObject ho = toLoad.HitObjects[i];
                bool isKiai = ctp.IsNoteInKiai(ho);

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
                    chartSections[chartSections.Count - 1].AddObject(ctp.GetNoteAdjustedBPM(ho), ho.StartTime);
                    chartSections.Add(new ChartSection(isKiai));
                }
                chartSections[chartSections.Count - 1].AddObject(ctp.GetNoteAdjustedBPM(ho), ho.StartTime);

                ///if (ho.GetType() == typeof(TaikoHit))
                ///{
                ///    TaikoHit aux = ho as TaikoHit;
                ///    string type = aux.Color == TaikoColor.Red ? "Red    " : "Blue   ";
                ///    Console.WriteLine($" {i + 1}:{type} - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                ///}
                ///else if (ho.GetType() == typeof(TaikoDrumroll))
                ///{
                ///    Console.WriteLine($" {i + 1}:Slider  - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                ///}
                ///else
                ///{
                ///    Console.WriteLine($" {i + 1}:Spinner - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                ///}
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

        void DrawChart()
        {
            SVChart.Series.Clear();
            double minVal = Math.Round(ctp.GetLowestBPMSV());
            double maxVal = Math.Round(ctp.GetHighestBPMSV());

            for (int i = 0; i < chartSections.Count; i++)
            {
                ChartSection section = chartSections[i];

                var series = new Series($"SVs-{i}");

                series.Points.DataBindXY(MillisToSeconds(section.GetMillis()), section.GetSVs());
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 1;
                series.BorderColor = Color.Gray;

                series.MarkerStyle = MarkerStyle.Circle;
                series.MarkerSize = 1;
                //series.Color = section.IsKiai ? Color.Orange : Color.DarkGreen;
                series.Color = section.IsKiai ? Color.FromArgb(255, 106, 0) : Color.DarkGreen;
                SVChart.Series.Add(series);
            }

            SVChart.ChartAreas[0].AxisX.Minimum = 0;
            //SVChart.ChartAreas[0].AxisX.Maximum = Math.Round(ctp.GetLastOffset() * 1.02 / 1000.0);
            List<int> lastPointMillis = chartSections[chartSections.Count - 1].GetMillis();
            SVChart.ChartAreas[0].AxisX.Maximum = Math.Round(lastPointMillis[lastPointMillis.Count - 1] * 1.02 / 1000.0);

            SVChart.ChartAreas[0].AxisY.Minimum = minVal - 10;
            SVChart.ChartAreas[0].AxisY.Maximum = maxVal + 10;
            SVChart.ChartAreas[0].AxisY.Interval = (int)Math.Round((maxVal - minVal) / 10);
            SVChart.ChartAreas[0].AxisX.Interval = 30;


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

            SVChart.Series[0].ToolTip = $"{"#VAL":F2} BPM, {"#VALX":F2} seconds, ";

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
                    currentLoadedBeatmap = openFileDialog.FileName;
                    ParseBeatmap(currentLoadedBeatmap);
                }
            }
        }

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentLoadedBeatmap))
            {
                ParseBeatmap(currentLoadedBeatmap);
            }
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
                if (dialogResult == DialogResult.OK)
                {
                    try
                    {
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
                MessageBox.Show($"No updates pending; you're already up to date fam");
            }
        }
        #endregion

    }
}
