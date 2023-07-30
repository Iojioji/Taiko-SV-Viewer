using System;
using System.Configuration;
using System.Drawing;

namespace TaikoMapSVViewer.Settings
{
    public static class SettingsManager
    {
        static string _version = "";
        static bool _autoUpdateSelectedMap = false;
        static string _songsFolder = "";
        static Colors _colors;
        static int _svMod;

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
        public static int SVMod
        {
            get => _svMod;
            set
            {
                _svMod = value;
                Properties.Settings.Default.SVModSetting = _svMod;
                Properties.Settings.Default.Save();
            }
        }

        public static void Init()
        {
            _autoUpdateSelectedMap = Properties.Settings.Default.AutoUpdateSelectedBeatmap;
            _songsFolder = Properties.Settings.Default.SongsFolder;
            _svMod = Properties.Settings.Default.SVModSetting;

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
