using OsuMemoryDataProvider;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoMapSVViewer
{
    public class MemoryReaderManager
    {
        readonly StructuredOsuMemoryReader osuReader;

        public MemoryReaderManager()
        {
            osuReader = StructuredOsuMemoryReader.Instance.GetInstanceForWindowTitleHint("osu!");
            //Shown += OnShown;
            if (osuReader.CanRead)
            {
                Debug.WriteLine("\r\n\r\nMemoryReaderManager can read!\r\n\r\n");
            }
            else
            {
                Debug.WriteLine("\r\n\r\nMemoryReaderManager can't read unu\r\n\r\n");
            }
        }

        private async void OnShown(object sender, EventArgs eventArgs)
        {
            await Task.Delay(1);
        }
    }
}