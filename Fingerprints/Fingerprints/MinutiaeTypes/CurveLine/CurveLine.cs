using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.CurveLine
{
    class CurveLine : Minutiae
    {
        protected Polyline baseLine;
        public CurveLine(MinutiaState state) : base(state)
        {}
        public override string ToString()
        {
            string points = null;
            //if (baseLine != null && baseLine.Points.Count > 0)
            //{
            //    foreach (var point in convertLinesToPoints(baseLine.Points))
            //    {
            //        points += point.X + ";" + point.Y + ";";
            //    }
            //    return state.Id + ";" + state.Minutia.Name + ";" + points;
            //}
            return "";
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;

            JArray pointsArray = new JArray();
            //foreach (var point in convertLinesToPoints(baseLine.Points))
            //{
            //    pointsArray.Add(point.ToJObject());
            //}

            minutiaeJson["points"] = pointsArray;

            return minutiaeJson.ToString();
        }
    }
}
