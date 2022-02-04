using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Beatmaps.Objects.Taiko;

namespace TaikoMapSVViewer
{
    public class ConvertedTimingPoint
    {
        List<UninheritedTimingPoint> timingPoints = new List<UninheritedTimingPoint>();

        public void AddTimingPoint(TimingPoint toAdd)
        {
            if (toAdd.Inherited)
            {
                timingPoints.Add(new UninheritedTimingPoint(toAdd));
            }
            else
            {
                timingPoints[timingPoints.Count - 1].AddInheritedPoint(toAdd);
            }
        }

        public double GetLowestBPMSV()
        {
            double lowestSV = -1;
            foreach (UninheritedTimingPoint utp in timingPoints)
            {
                if (lowestSV == -1)
                {
                    lowestSV = utp.BPM;
                }
                foreach (InheritedTimingPoint itp in utp.InheritedTimingPoints)
                {
                    if (itp.AdjustedBPM < lowestSV)
                    {
                        lowestSV = itp.AdjustedBPM;
                    }
                }
            }
            return lowestSV;
        }
        public double GetHighestBPMSV()
        {
            double highestSV = -1;
            foreach (UninheritedTimingPoint utp in timingPoints)
            {
                if (highestSV == -1)
                {
                    highestSV = utp.BPM;
                }
                foreach (InheritedTimingPoint itp in utp.InheritedTimingPoints)
                {
                    if (itp.AdjustedBPM > highestSV)
                    {
                        highestSV = itp.AdjustedBPM;
                    }
                }
            }
            return highestSV;
        }
        public int GetLastOffset()
        {
            return timingPoints[timingPoints.Count - 1].GetLastOffset();
        }

        public double GetNoteAdjustedBPM(HitObject toCheck)
        {
            if (timingPoints.Count > 1)
            {
                //Check more uninherited points.
                return GetClosestRedTimingPoint(toCheck).GetClosestGreenTimingPoint(toCheck).AdjustedBPM;
            }
            else
            {
                //Only one uninherited point, search inside that one.
                return timingPoints[0].GetClosestGreenTimingPoint(toCheck).AdjustedBPM;
            }
        }

        public bool IsNoteInKiai(HitObject toCheck)
        {
            bool result = false;

            if (timingPoints.Count > 1)
            {
                return GetClosestRedTimingPoint(toCheck).GetClosestGreenTimingPoint(toCheck).IsKiai;
            }
            else
            {
                return timingPoints[0].GetClosestGreenTimingPoint(toCheck).IsKiai;
            }

            return result;
        }

        UninheritedTimingPoint GetClosestRedTimingPoint(HitObject toCheck)
        {
            UninheritedTimingPoint aux = null;
            int earliestIndex = -1;
            for (int i = timingPoints.Count - 1; i >= 0; i--)
            {
                if (timingPoints[i].TimingPoint.Offset <= toCheck.StartTime)
                {
                    earliestIndex = i;
                    aux = timingPoints[i];
                    break;
                }
            }
            return aux;
        }
        public string PrintTimingPoints()
        {
            string aux = "";
            foreach (UninheritedTimingPoint utp in timingPoints)
            {
                aux += utp + "\r\n";
            }
            return aux;
        }
    }

    public class UninheritedTimingPoint
    {
        double _bpm;
        TimingPoint _point;
        List<InheritedTimingPoint> _inheritedTimingPoints = new List<InheritedTimingPoint>();

        public double BPM
        {
            get { return _bpm; }
        }
        public TimingPoint TimingPoint
        {
            get { return _point; }
        }
        public List<InheritedTimingPoint> InheritedTimingPoints
        {
            get { return _inheritedTimingPoints; }
        }
        public UninheritedTimingPoint(TimingPoint originalPoint)
        {
            _point = originalPoint;
            _bpm = 1 / _point.BeatLength * 1000 * 60;
        }

        public void AddInheritedPoint(TimingPoint timingPoint)
        {
            _inheritedTimingPoints.Add(new InheritedTimingPoint(this, timingPoint));
        }
        public int GetLastOffset()
        {
            if (_inheritedTimingPoints.Count > 0)
                return _inheritedTimingPoints[_inheritedTimingPoints.Count - 1].TimingPoint.Offset;
            return -2;
        }
        public InheritedTimingPoint GetClosestGreenTimingPoint(HitObject toCheck)
        {
            InheritedTimingPoint aux = null;
            int earliestIndex = -1;
            for (int i = _inheritedTimingPoints.Count - 1; i >= 0; i--)
            {
                if (_inheritedTimingPoints[i].TimingPoint.Offset <= toCheck.StartTime)
                {
                    aux = _inheritedTimingPoints[i];
                    break;
                }
            }
            if (aux == null)
            {
                aux = new InheritedTimingPoint(this, _point);
            }
            return aux;
        }

        public override string ToString()
        {
            string aux = "";
            aux += $"Red ({_point.Offset})\r\n";
            foreach (InheritedTimingPoint itp in _inheritedTimingPoints)
            {
                aux += itp + "\r\n";
            }
            return aux;
        }

    }

    public class InheritedTimingPoint
    {
        double _multiplier;
        double _adjustedBPM;
        UninheritedTimingPoint _parentPoint;
        TimingPoint _originalPoint;

        public double Multiplier
        {
            get { return _multiplier; }
        }
        public double AdjustedBPM
        {
            get { return _adjustedBPM; }
        }
        public UninheritedTimingPoint Parent
        {
            get { return _parentPoint; }
        }
        public TimingPoint TimingPoint
        {
            get { return _originalPoint; }
        }
        public bool IsKiai
        {
            get { return _originalPoint.Effects.HasFlag(OsuParsers.Enums.Beatmaps.Effects.Kiai); }
        }

        public InheritedTimingPoint(UninheritedTimingPoint parent, TimingPoint originalPoint)
        {
            this._parentPoint = parent;
            this._originalPoint = originalPoint;
            _multiplier = originalPoint.BeatLength > 0 ? 1 : 1 / -originalPoint.BeatLength * 100;
            _adjustedBPM = _multiplier * parent.BPM;
        }

        public override string ToString()
        {
            return $"  Green ({_originalPoint.Offset})\r\n    Mul: {_multiplier}\r\n    BPM: {_adjustedBPM}";
        }
    }
}
