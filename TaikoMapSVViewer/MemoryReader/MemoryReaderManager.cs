using OsuMemoryDataProvider;
using OsuMemoryDataProvider.OsuMemoryModels;
using OsuMemoryDataProvider.OsuMemoryModels.Direct;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using TaikoMapSVViewer.Data.MemoryReader;
using System.Net.Http.Headers;
using System.Windows.Forms;
using TaikoMapSVViewer.Settings;

namespace TaikoMapSVViewer
{
    public delegate void ReadError(string error);
    public delegate void OnStringChanged(string newValue);
    public delegate void OnPathChanged(string newFileName, string newFolderName);
    public delegate void OnFloatChanged(float newValue);
    public delegate void OnIntChanged(int newValue);
    public delegate void OnModChanged(ReaderMods newValue);

    public class MemoryReaderManager
    {
        int _readDelay = 33;
        readonly object _minMaxLock = new object();
        double _memoryReadTimeMin = double.PositiveInfinity;
        double _memoryReadTimeMax = double.NegativeInfinity;

        ReadError _onReadError;

        readonly StructuredOsuMemoryReader _osuReader;
        CancellationTokenSource cts = new CancellationTokenSource();

        OsuReaderValues _osuValues;

        MainForm _form;

        bool _previousCanRead = true;

        public ReadError OnReadError { get => _onReadError; set => _onReadError = value; }

        T ReadProperty<T>(object readObj, string propName, T defaultValue = default) where T : struct
        {
            if (_osuReader.TryReadProperty(readObj, propName, out var readResult))
                return (T)readResult;

            return defaultValue;
        }
        T ReadClassProperty<T>(object readObj, string propName, T defaultValue = default) where T : class
        {
            if (_osuReader.TryReadProperty(readObj, propName, out var readResult))
                return (T)readResult;

            return defaultValue;
        }

        int ReadInt(object readObj, string propName)
            => ReadProperty<int>(readObj, propName, -5);
        short ReadShort(object readObj, string propName)
            => ReadProperty<short>(readObj, propName, -5);
        float ReadFloat(object readObj, string propName)
            => ReadProperty<float>(readObj, propName, -5f);
        string ReadString(object readObj, string propName)
            => ReadClassProperty<string>(readObj, propName, "INVALID_READ");

        public MemoryReaderManager(MainForm form, OnPathChanged onPathChanged, OnModChanged onModChanged, OnIntChanged onAudioTimeChanged, string windowTitleHint = "osu!")
        {
            _form = form;
            _osuReader = StructuredOsuMemoryReader.Instance.GetInstanceForWindowTitleHint(windowTitleHint);
            _osuValues = new OsuReaderValues(onPathChanged, onModChanged, onAudioTimeChanged);
            _osuValues._form = _form;
            _form.SetSongsFolder();
        }

        public async void StartReading()
        {
            _osuReader.InvalidRead += OnInvalidRead;

            await Task.Run(async () =>
            {
                Stopwatch stopwatch;
                double readTimeMs, readTimeMsMin, readTimeMsMax;
                _osuReader.WithTimes = true;
                //var readUsingProperty = false;
                var baseAddresses = new OsuBaseAddresses();
                while (true)
                {
                    if (cts.IsCancellationRequested) return;

                    if (!_osuReader.CanRead)
                    {
                        if (_previousCanRead != _osuReader.CanRead)
                        {
                            _osuValues.Reset();
                            _onReadError?.Invoke($"osu! process not found!");
                        }

                        _previousCanRead = bool.Parse(_osuReader.CanRead.ToString());

                        await Task.Delay(_readDelay);

                        continue;
                    }


                    stopwatch = Stopwatch.StartNew();

                    //if (!_osuReader.TryRead(baseAddresses.BanchoUser))
                    //{
                    //    //continue;
                    //}

                    baseAddresses.Beatmap.OsuFileName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.OsuFileName));
                    baseAddresses.Beatmap.FolderName = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.FolderName));
                    //baseAddresses.Beatmap.MapString = ReadString(baseAddresses.Beatmap, nameof(CurrentBeatmap.MapString));

                    //TODO: Check if current mods are needed.
                    if (SettingsManager.SVMod == (int)SVMod.AutoUpdate)
                    {
                        baseAddresses.GeneralData.Mods = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.Mods));
                    }
                    //TODO: Check if AudioTime is needed.
                    //if (true)
                    //{
                    //    baseAddresses.GeneralData.AudioTime = ReadInt(baseAddresses.GeneralData, nameof(GeneralData.AudioTime));
                    //}

                    //if (baseAddresses.GeneralData.OsuStatus == OsuMemoryStatus.SongSelect)
                    //{
                    //    _osuReader.TryRead(baseAddresses.LeaderBoard);
                    //}

                    stopwatch.Stop();

                    //_osuValues.UpdateFileName("Petetete");

                    readTimeMs = stopwatch.ElapsedTicks / (double)TimeSpan.TicksPerMillisecond;

                    lock (_minMaxLock)
                    {
                        if (readTimeMs < _memoryReadTimeMin) _memoryReadTimeMin = readTimeMs;
                        if (readTimeMs > _memoryReadTimeMax) _memoryReadTimeMax = readTimeMs;

                        readTimeMsMin = _memoryReadTimeMin;
                        readTimeMsMax = _memoryReadTimeMax;
                    }

                    try
                    {
                        _osuValues.UpdatePath(baseAddresses.Beatmap.OsuFileName, baseAddresses.Beatmap.FolderName);
                        //_osuValues.UpdateFileName(baseAddresses.Beatmap.OsuFileName);
                        //_osuValues.UpdateMapString(baseAddresses.Beatmap.MapString);
                        //TODO: Check if should auto update mods, else just set a default value.


                        _osuValues.UpdateMods(GetMods(baseAddresses.GeneralData.Mods));
                        //TODO: Check if should auto update audoTime, else just set a default value.
                        //_osuValues.UpdateAudioTime(baseAddresses.GeneralData.AudioTime);

                        //string timeText = $"         ReadTimeMS: {readTimeMs}{Environment.NewLine}" +
                        //                  $" Min ReadTimeMS: {readTimeMsMin}{Environment.NewLine}" +
                        //                  $"Max ReadTimeMS: {readTimeMsMax}{Environment.NewLine}";

                        //_onReadError?.Invoke($"\r\n- - - - - - - -\r\n- - - - - - - -\r\n" +
                        //                     $"         ReadTimeMS: {readTimeMs}{Environment.NewLine}\r\n" +
                        //                     $"{_osuValues}\r\n- - - - - - - -\r\n- - - - - - - -\r\n");
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }

                    _previousCanRead = bool.Parse(_osuReader.CanRead.ToString());
                    _osuReader.ReadTimes.Clear();
                    await Task.Delay(_readDelay);
                }
            }, cts.Token);
        }
        public void StopReading()
        {
            Debug.WriteLine($"Stopping the reads");
            cts.Cancel();
        }

        void OnInvalidRead(object sender, (object readObject, string propPath) propPath)
        {
            //if (_form.InvokeRequired)
            //{
            //    _form.BeginInvoke((MethodInvoker)(() => OnInvalidRead(sender, propPath)));
            //    return;
            //}


            _form.OnReadError($"{DateTime.Now:T} Error reading {propPath.propPath}");

            try
            {
                //if (_form.InvokeRequired)
                //{
                //    //_form.BeginInvoke((MethodInvoker)(() => OnInvalidRead(sender, propPath)));
                //    //_form.BeginInvoke((MethodInvoker)(() => OnInvalidRead(sender, propPath)));
                //    //_form.OnReadError($"{DateTime.Now:T} Error reading {propPath.propPath}");
                //    return;
                //}

                //_form.OnReadError($"{DateTime.Now:T} Error reading {propPath.propPath}");
                //_onReadError?.Invoke($"{DateTime.Now:T} Error reading {propPath.propPath}");
            }
            catch (ObjectDisposedException)
            {
                //_onReadError?.Invoke($"[EXCEPTION] {DateTime.Now:T} Error reading {propPath.propPath}");
            }
        }

        int GetMods(int readMod)
        {
            int result = readMod;
            switch (SettingsManager.SVMod)
            {
                case 0:
                    result = readMod;
                    break;
                case 1:
                    result = (int)ReaderMods.NoMod;
                    break;
                case 2:
                    result = (int)ReaderMods.HR;
                    break;
                case 3:
                    result = (int)ReaderMods.EZ;
                    break;
            }
            return result;
        }
    }

    public class OsuReaderValues
    {
        internal MainForm _form;

        internal string _fileName = "INVALID_STRING";
        internal string _folderName = "INVALID_STRING";
        internal int _mods = -1;
        internal ReaderMods _activeMods = ReaderMods.NoMod;
        internal int _audioTime = -1;

        internal OnPathChanged _onPathChanged;
        internal OnModChanged _onModChanged;
        internal OnIntChanged _onAudioTimeChanged;

        public OsuReaderValues(OnPathChanged onPathChanged, OnModChanged onModChanged, OnIntChanged onAudioTimeChanged)
        {
            _onPathChanged = onPathChanged;
            _onModChanged = onModChanged;
            _onAudioTimeChanged = onAudioTimeChanged;
        }
        //public OsuReaderValues(string osuFileName, string mapString, int activeMods, int audioTime)
        //{

        //}

        internal void UpdatePath(string newFileName, string newFolderName)
        {
            if (newFileName == _fileName && newFolderName == _folderName) return;
            _fileName = newFileName;
            _folderName = newFolderName;
            _onPathChanged?.Invoke(_fileName, _folderName);
        }
        internal void UpdateMods(int newValue)
        {
            if (newValue == _mods) return;
            _mods = newValue;

            string auxMods = ((ReaderMods)_mods).ToString();
            if (auxMods.Contains("HR"))
            {
                _activeMods = ReaderMods.HR;
            }
            else if (auxMods.Contains("EZ"))
            {
                _activeMods = ReaderMods.EZ;
            }
            else
            {
                _activeMods = ReaderMods.NoMod;
            }

            _onModChanged?.Invoke(_activeMods);
        }
        internal void UpdateAudioTime(int newValue)
        {
            if (newValue == _audioTime) return;
            _audioTime = newValue;
            _onAudioTimeChanged?.Invoke(_audioTime);
        }
        internal void Reset()
        {
            _fileName = "INVALID_STRING";
            _folderName = "INVALID_STRING";
            _mods = -1;
            _activeMods = ReaderMods.NoMod;
            _audioTime = -1;
        }

        public override string ToString()
        {
            return  $"  File: {_fileName}\r\n" +
                    $"  Folder: {_folderName}\r\n" +
                    $"  Mods: ({_activeMods})\r\n" +
                    $"  Audio: ({_audioTime})";
        }
    }
}