using System;
using System.Collections.Generic;
using OsuParsers.Beatmaps.Objects;

namespace TaikoMapSVViewer
{
    public class ConvertedTimingPoint
    {
        double sliderMultiplier = -1;
        List<UninheritedTimingPoint> timingPoints = new List<UninheritedTimingPoint>();

        public double SliderMultiplier
        {
            get { return sliderMultiplier; }
            set { sliderMultiplier = value; }
        }

        public void AddTimingPoint(TimingPoint toAdd)
        {
            if (!toAdd.Inherited)
            {
                timingPoints.Add(new UninheritedTimingPoint(toAdd));
            }
            else
            {
                if (timingPoints.Count > 0)
                {
                    timingPoints[timingPoints.Count - 1].AddInheritedPoint(toAdd);
                }
                else
                {
                    Console.WriteLine("Map started with a green line instead of a red line, watch out bro");
                }
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
            return lowestSV * (sliderMultiplier / 1.4);
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
            return highestSV * (sliderMultiplier / 1.4);
        }
        public int GetLastOffset()
        {
            return timingPoints[timingPoints.Count - 1].GetLastOffset();
        }

        //public double GetNoteAdjustedBPM(HitObject toCheck)
        public double GetNoteAdjustedBPM(int offsetToCheck)
        {
            double result = 0;
            double adjustedBPM = 0;
            if (timingPoints.Count > 1)
            {
                //Check more uninherited points.
                adjustedBPM = GetClosestRedTimingPoint(offsetToCheck).GetClosestGreenTimingPoint(offsetToCheck).AdjustedBPM;
            }
            else
            {
                //Only one uninherited point, search inside that one.
                adjustedBPM = timingPoints[0].GetClosestGreenTimingPoint(offsetToCheck).AdjustedBPM;
            }

            result = adjustedBPM * (sliderMultiplier / 1.4);
            return result;
        }

        public bool IsNoteInKiai(HitObject toCheck)
        {
            bool result = false;

            if (timingPoints.Count > 1)
            {
                return GetClosestRedTimingPoint(toCheck.StartTime).GetClosestGreenTimingPoint(toCheck.StartTime).IsKiai;
            }
            else
            {
                return timingPoints[0].GetClosestGreenTimingPoint(toCheck.StartTime).IsKiai;
            }

            return result;
        }

        //UninheritedTimingPoint GetClosestRedTimingPoint(HitObject toCheck)
        UninheritedTimingPoint GetClosestRedTimingPoint(int offsetToCheck)
        {
            UninheritedTimingPoint aux = null;
            int earliestIndex = -1;
            for (int i = timingPoints.Count - 1; i >= 0; i--)
            {
                if (timingPoints[i].TimingPoint.Offset <= offsetToCheck)
                {
                    earliestIndex = i;
                    aux = timingPoints[i];
                    break;
                }
            }
            if (aux == null)
            {
                return timingPoints[0];
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
            else
                return _point.Offset;
        }
        //public InheritedTimingPoint GetClosestGreenTimingPoint(HitObject toCheck)
        public InheritedTimingPoint GetClosestGreenTimingPoint(int offsetToCheck)
        {
            InheritedTimingPoint aux = null;
            int earliestIndex = -1;
            for (int i = _inheritedTimingPoints.Count - 1; i >= 0; i--)
            {
                if (_inheritedTimingPoints[i].TimingPoint.Offset <= offsetToCheck)
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
