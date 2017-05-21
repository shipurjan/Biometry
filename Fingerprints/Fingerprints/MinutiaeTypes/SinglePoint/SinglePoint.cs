using Fingerprints.Models;
using Fingerprints.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Fingerprints.MinutiaeTypes.SinglePoint
{
    abstract class SinglePoint : Minutiae
    {
        public SinglePoint(MinutiaState state) : base(state)
        {}

        public override string ToString()
        {
            return state.Id + ";" + state.Minutia.Name + ";" + Math.Floor(state.Points[0].X).ToString() + ";" + Math.Floor(state.Points[0].Y).ToString();
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = state.Id;
            minutiaeJson["name"] = state.Minutia.Name;
            minutiaeJson["points"] = new JArray()
            {
                state.Points[0].ToJObject()
            };

            return minutiaeJson.ToString();
        }
    }
}
