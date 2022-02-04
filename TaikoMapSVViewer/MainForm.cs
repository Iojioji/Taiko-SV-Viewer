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

namespace TaikoMapSVViewer
{
    public partial class MainForm : Form
    {
        Beatmap currentBeatmap;
        ConvertedTimingPoint ctp = new ConvertedTimingPoint();
        //List<double> adjustedSVs = new List<double>();
        //List<int> objectTimes = new List<int>();

        List<ChartSection> chartSections = new List<ChartSection>();

        string currentLoadedBeatmap = "";
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
            //Console.WriteLine(ctp.PrintTimingPoints());
            //Console.WriteLine($"\r\n{PrintTimingPoints(currentBeatmap)}");
        }
        void LoadConvertedTimingPoints(Beatmap toLoad)
        {
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
                    //chartSections[chartSections.Count - 1].AddObject(Math.Round(ctp.GetNoteAdjustedBPM(ho), 2), ho.StartTime);
                    chartSections[chartSections.Count - 1].AddObject(ctp.GetNoteAdjustedBPM(ho), ho.StartTime);
                    chartSections.Add(new ChartSection(isKiai));
                }

                //adjustedSVs.Add(Math.Round(ctp.GetNoteAdjustedBPM(ho), 2));
                //objectTimes.Add(ho.StartTime);

                //chartSections[chartSections.Count - 1].AddObject(Math.Round(ctp.GetNoteAdjustedBPM(ho), 2), ho.StartTime);
                chartSections[chartSections.Count - 1].AddObject(ctp.GetNoteAdjustedBPM(ho), ho.StartTime);

                //if (ho.GetType() == typeof(TaikoHit))
                //{
                //    TaikoHit aux = ho as TaikoHit;
                //    string type = aux.Color == TaikoColor.Red ? "Red    " : "Blue   ";
                //    Console.WriteLine($" {i + 1}:{type} - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                //}
                //else if (ho.GetType() == typeof(TaikoDrumroll))
                //{
                //    Console.WriteLine($" {i + 1}:Slider  - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                //}
                //else
                //{
                //    Console.WriteLine($" {i + 1}:Spinner - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                //}
                previousNoteWasKiai = isKiai;
            }
        }
        List<double> MillisToSeconds(List<int> millis)
        {
            List<double> seconds = new List<double>();

            foreach (int milli in millis)
            {
                seconds.Add(Math.Round(milli / 1000.0, 4));
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

                //series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
                //series.Points.DataBindXY(MillisToSeconds(objectTimes), adjustedSVs);
                series.Points.DataBindXY(MillisToSeconds(section.GetMillis()), section.GetSVs());
                series.ChartType = SeriesChartType.FastLine;
                series.BorderWidth = 1;
                series.BorderColor = Color.Gray;
                //series.Color = section.IsKiai ? Color.Orange : Color.DarkGreen;
                series.Color = section.IsKiai ? Color.FromArgb(255, 106, 0) : Color.DarkGreen;
                SVChart.Series.Add(series);
            }

            //SVChart.ChartAreas[0].BackColor = Color.Black;
            SVChart.ChartAreas[0].AxisY.Minimum = minVal - 10;
            SVChart.ChartAreas[0].AxisY.Maximum = maxVal + 10;
            SVChart.ChartAreas[0].AxisY.Interval = (int)Math.Round((maxVal - minVal) / 10);
            SVChart.ChartAreas[0].AxisX.Interval = 30;

            SVChart.ChartAreas[0].AxisX.Name = "Seconds";
            SVChart.ChartAreas[0].AxisY.Name = "BPM SV";

            foreach (ChartArea a in SVChart.ChartAreas)
            {
                //a.AxisX.LineColor = Color.FromArgb(126, 126, 126, 126);
                //a.AxisY.LineColor = Color.FromArgb(126, 126, 126, 126);
                a.AxisX.LineColor = Color.Pink;
                a.AxisY.LineColor = Color.Pink;
            }
        }
        void SetWindowTitle(BeatmapMetadataSection data)
        {
            Text = $"Checking: {data.Title} [{data.Version }] by {data.Creator}";
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
        #endregion

        private void toolStripRefresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(currentLoadedBeatmap))
            {
                ParseBeatmap(currentLoadedBeatmap);
            }
        }
    }
}
