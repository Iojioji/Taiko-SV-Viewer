using System;

namespace TaikoMapSVViewer.Settings
{
    public static class SettingsManager
    {
        static string _version = "";
        static bool _autoUpdateSelectedMap = false;
        static bool _autoUpdateMod = false;
        static string _songsFolder = "";

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

        public static void Init()
        {
            _autoUpdateSelectedMap = Properties.Settings.Default.AutoUpdateSelectedBeatmap;
            _songsFolder = Properties.Settings.Default.SongsFolder;
        }
    }
}
