using System;
using System.Configuration;
using System.Drawing;

namespace TaikoMapSVViewer.Settings
{
    public static class SettingsManager
    {
        static string _version = "";
        static bool _autoUpdateSelectedMap = false;
        static bool _autoUpdateMod = false;
        static string _songsFolder = "";
        static Colors _colors;

        public static string Version { get => _version; set => _version = value; }
        public static bool AutoUpdateSelectedMap
        {
            get => _autoUpdateSelectedMap;
            set
            {
                _autoUpdateSelectedMap = value;
                Properties.Settings.Default.AutoUpdateSelectedBeatmap = _autoUpdateSelectedMap;
                Properties.Settings.Default.Save();
            }
        }
        public static bool AutoUpdateMod
        {
            get => _autoUpdateMod;
            set
            {
                _autoUpdateMod = value;
                Properties.Settings.Default.AutoUpdateMod = _autoUpdateMod;
                Properties.Settings.Default.Save();
            }
        }
        public static string SongsFolder
        {
            get => _songsFolder;
            set
            {
                _songsFolder = value;
                Properties.Settings.Default.SongsFolder = _songsFolder;
                Properties.Settings.Default.Save();
            }
        }
        public static Colors Colors
        {
            get => _colors;
            set
            {
                _colors = value;
            }
        }

        public static void Init()
        {
            _autoUpdateSelectedMap = Properties.Settings.Default.AutoUpdateSelectedBeatmap;
            _songsFolder = Properties.Settings.Default.SongsFolder;

            _colors = new Colors();
            _colors.GraphLineColor = Properties.Settings.Default.GraphLineColor;
            _colors.GraphLineColorKiai = Properties.Settings.Default.GraphLineColorKiai;
        }
    }

    public class Colors
    {
        Color _graphLineColor = Color.DarkGreen;
        Color _graphLineColorKiai = Color.FromArgb(255, 106, 0);

        public Color GraphLineColor
        {
            get => _graphLineColor;
            set
            {
                _graphLineColor = value;
                Properties.Settings.Default.GraphLineColor = _graphLineColor;
                Properties.Settings.Default.Save();
            }
        }
        public Color GraphLineColorKiai
        {
            get => _graphLineColorKiai;
            set
            {
                _graphLineColorKiai = value;
                Properties.Settings.Default.GraphLineColorKiai = _graphLineColorKiai;
                Properties.Settings.Default.Save();
            }
        }
    }
}
