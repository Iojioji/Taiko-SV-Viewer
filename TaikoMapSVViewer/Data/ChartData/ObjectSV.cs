using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoMapSVViewer.Data.ChartData
{
    //TODO: Find better name for class lmao
    public class ObjectSV
    {
        double _SV;
        int _time;
        
        public double SV
        {
            get { return _SV; }
            set { _SV = value; }
        }
        public int Time
        {
            get { return _time; }
        }

        public ObjectSV(double sv, int time)
        {
            _time = time;
            _SV = sv;
        }
    }
}
