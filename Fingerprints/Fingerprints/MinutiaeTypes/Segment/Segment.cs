using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.MinutiaeTypes.Segment
{
    class Segment : Minutiae
    {
        public Segment(MinutiaState state)
        {
            this.state = state;
            ConvertStateColorToBrush();
        }

        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString() + ";" + Math.Floor(state.Points[1].X).ToString() + ";" + Math.Floor(state.Points[1].Y).ToString();
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;
            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject(),
                state.Points[1].ToJObject()
            };

            return minutiaeJson.ToString();
        }
    }
}
