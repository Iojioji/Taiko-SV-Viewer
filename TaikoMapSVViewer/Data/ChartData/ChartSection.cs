using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaikoMapSVViewer.Data.ChartData
{
    public class ChartSection
    {
        bool _isKiai = false;
        List<ObjectSV> _objectCollection = new List<ObjectSV>();

        public bool IsKiai
        {
            get { return _isKiai; }
        }
        public List<ObjectSV> ObjectCollection
        {
            get { return _objectCollection; }
        }

        public ChartSection(bool isKiai)
        {
            _objectCollection = new List<ObjectSV>();
            _isKiai = isKiai;
        }

        public List<double> GetSVs()
        {
            List<double> sv = new List<double>();

            foreach (ObjectSV objectSV in _objectCollection)
            {
                sv.Add(objectSV.SV);
            }

            return sv;
        }

        public List<int> GetMillis()
        {
            List<int> millis = new List<int>();

            foreach (ObjectSV objectSV in _objectCollection)
            {
                millis.Add(objectSV.Time);
            }

            return millis;
        }

        public void AddObject(double sv, int millis)
        {
            _objectCollection.Add(new ObjectSV(sv, millis));
        }

        public double GetLowestSV()
        {
            double result = -1;
            if (_objectCollection.Count == 0)
            {
                Console.Write($"Collection was empty wtf???");
                return result;
            }

            result = _objectCollection.OrderBy(x => x.SV).ToList()[0].SV;

            return result;
        }
        public double GetHighestSV()
        {
            double result = -1;
            if (_objectCollection.Count == 0)
            {
                Console.Write($"Collection was empty wtf???");
                return result;
            }

            result = _objectCollection.OrderByDescending(x => x.SV).ToList()[0].SV;

            return result;
        }

        public void UpdateSV(ConvertedTimingPoint ctp)
        {
            foreach (ObjectSV objectSV in _objectCollection)
            {
                objectSV.SV = ctp.GetNoteAdjustedBPM(objectSV.Time);
            }
        }
        public int GetLastMilli()
        {
            return _objectCollection.Last().Time;
        }
    }
}
