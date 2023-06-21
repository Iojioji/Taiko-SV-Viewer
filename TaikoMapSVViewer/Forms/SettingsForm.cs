using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaikoMapSVViewer.Settings;

namespace TaikoMapSVViewer
{
    public partial class SettingsForm : Form
    {
        MainForm _mainForm;

        public SettingsForm()
        {
            InitializeComponent();
        }

        public void Init(MainForm mainForm)
        {
            GraphLineColorSample.BackColor = SettingsManager.Colors.GraphLineColor;
            GraphLineColorKiaiSample.BackColor = SettingsManager.Colors.GraphLineColorKiai;

            _mainForm = mainForm;
        }

        private void GraphLineColorSample_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = GraphLineColorSample.BackColor;

            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                GraphLineColorSample.BackColor = ColorDialog.Color;
            }
        }

        private void GraphLineColorKiaiSample_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = GraphLineColorKiaiSample.BackColor;

            if (ColorDialog.ShowDialog() == DialogResult.OK)
            {
                GraphLineColorKiaiSample.BackColor = ColorDialog.Color;
            }
        }

        private void DefaultButton_Click(object sender, EventArgs e)
        {
            GraphLineColorSample.BackColor = Color.DarkGreen;
            GraphLineColorKiaiSample.BackColor = Color.FromArgb(255, 106, 0);
        }
        private void SaveColorsButton_Click(object sender, EventArgs e)
        {
            SettingsManager.Colors.GraphLineColor = GraphLineColorSample.BackColor;
            SettingsManager.Colors.GraphLineColorKiai = GraphLineColorKiaiSample.BackColor;

            _mainForm.RefreshBeatmap();
        }
    }
}