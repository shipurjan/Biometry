using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Fingerprints.MinutiaeTypes.Vector
{
    abstract class Vector : Minutiae
    {
        protected Point tmp1, tmp2;
        protected GeometryGroup group = new GeometryGroup();

        public Vector(MinutiaState state) : base(state)
        {
            tmp1 = new Point();
            tmp2 = new Point();
        }
        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString() + ";" + state.Angle.ToString();
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;
            minutiaeJson["angle"] = state.Angle;

            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject()
            };

            return minutiaeJson.ToString();
        }
    }
}
