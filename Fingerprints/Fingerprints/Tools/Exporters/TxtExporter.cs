using System;
using System.Collections.Generic;
using Fingerprints.MinutiaeTypes;
using ExceptionLogger;
using Fingerprints.Models;
using Newtonsoft.Json;
using Fingerprints.Factories;
using System.Windows;

namespace Fingerprints.Tools.Exporters
{
    class TxtExporter : ExporterBase, IDataExporter
    {
        private List<MinutiaFileState> preparedData;
        public TxtExporter(List<MinutiaStateBase> _data) : base(_data)
        {
            preparedData = new List<MinutiaFileState>();
        }

        public void Export(string _path)
        {
            try
            {
                FileTransfer.SaveFile(_path, JsonConvert.SerializeObject(preparedData, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void FormatData()
        {
            try
            {
                foreach (var item in data)
                {
                    if (item is CurveLineState)
                    {
                        preparedData.Add(ParseCurveLine(item));
                    }
                    else
                    {
                        preparedData.Add(Parse(item));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private MinutiaFileState ParseCurveLine(MinutiaStateBase _state)
        {
            MinutiaFileState result = null;
            try
            {
                result = FileMinutiaFactory.Create(_state);
                result.Points = PrepareCurveLinePoints(result.Points);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private List<Point> PrepareCurveLinePoints(List<Point> points)
        {
            List<Point> result = null;
            try
            {
                result = new List<Point>();

                for (int i = 0; i < points.Count - 1; i++)
                {
                    result.AddRange(LinePoints(points[i], points[i + 1]));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private List<Point> LinePoints(Point point1, Point point2)
        {
            List<Point> result = null;
            try
            {
                result = new List<Point>();
                double steps = 0;
                var dx = point2.X - point1.X;
                var dy = point2.Y - point1.Y;

                if (Math.Abs(dx) > Math.Abs(dy))
                    steps = Math.Abs(dx);

                if (Math.Abs(dy) > Math.Abs(dx))
                    steps = Math.Abs(dy);

                var Xincrement = dx / steps;
                var Yincrement = dy / steps;

                var x = point1.X;
                var y = point1.Y;
                result.Add(new Point(Math.Floor(x), Math.Floor(y)));

                for (int i = 0; i < steps; i++)
                {
                    x = x + Xincrement;
                    y = y + Yincrement;

                    result.Add(new Point(Math.Floor(x), Math.Floor(y)));
                }
            }
            catch
            {

            }
            return result;
        }

        private MinutiaFileState Parse(MinutiaStateBase _state)
        {
            MinutiaFileState result = null;
            try
            {
                result = FileMinutiaFactory.Create(_state);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }
    }
}
