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

namespace Fingerprints.MinutiaeTypes.Triangle
{
    class Triangle : Minutiae
    {
        protected Point tmp1, tmp2;
        protected GeometryGroup group = new GeometryGroup();

        public Triangle(MinutiaState state) : base(state)
        {
            tmp1 = new Point();
            tmp2 = new Point();
        }

        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString() + ";" + Math.Floor(state.Points[1].X).ToString() + ";" + Math.Floor(state.Points[1].Y).ToString() + ";" + Math.Floor(state.Points[2].X).ToString() + ";" + Math.Floor(state.Points[2].Y).ToString();
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;

            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject(),
                state.Points[1].ToJObject(),
                state.Points[2].ToJObject(),
            };

            return minutiaeJson.ToString();
        }
    }
}
