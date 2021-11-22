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

namespace TaikoMapSVViewer
{
    public partial class MainForm : Form
    {
        Beatmap currentBeatmap;
        ConvertedTimingPoint ctp = new ConvertedTimingPoint();
        List<double> adjustedSVs = new List<double>();
        List<int> objectTimes = new List<int>();
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
        }
        void LoadHitObjects(Beatmap toLoad)
        {
            adjustedSVs.Clear();
            objectTimes.Clear();
            for (int i = 0; i < toLoad.HitObjects.Count; i++)
            {
                HitObject ho = toLoad.HitObjects[i];
                adjustedSVs.Add(ctp.GetNoteAdjustedBPM(ho));
                objectTimes.Add(ho.StartTime);
                if (ho.GetType() == typeof(TaikoHit))
                {
                    TaikoHit aux = ho as TaikoHit;
                    string type = aux.Color == TaikoColor.Red ? "Red    " : "Blue   ";
                    Console.WriteLine($" {i+1}:{type} - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                }
                else if (ho.GetType() == typeof(TaikoDrumroll))
                {
                    Console.WriteLine($" {i+1}:Slider  - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                }
                else
                {
                    Console.WriteLine($" {i+1}:Spinner - SV: {ctp.GetNoteAdjustedBPM(ho)} BPM");
                }
            }
        }
        void DrawChart()
        {
            SVChart.Series.Clear();
            double minVal = ctp.GetLowestBPMSV();
            double maxVal = ctp.GetHighestBPMSV();
            var series = new Series("SVs");

            //series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
            series.Points.DataBindXY(objectTimes, adjustedSVs);
            series.ChartType = SeriesChartType.FastLine;
            series.BorderWidth = 1;
            series.BorderColor = Color.Black;
            series.Color = Color.Red;
            SVChart.Series.Add(series);
            SVChart.ChartAreas[0].AxisY.Minimum = minVal -10;
            SVChart.ChartAreas[0].AxisY.Maximum = maxVal +10;
            SVChart.ChartAreas[0].AxisY.Interval = (int)Math.Round((maxVal - minVal) / 10);
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
                    ParseBeatmap(openFileDialog.FileName);
                }
            }
        }
        #endregion
    }
}
