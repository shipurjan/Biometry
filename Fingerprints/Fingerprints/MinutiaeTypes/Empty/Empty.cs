using Fingerprints.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fingerprints.MinutiaeTypes.Empty
{
    abstract class Empty : Minutiae
    {
        public Empty(MinutiaState state) : base(state)
        {
            this.state.Minutia = new SelfDefinedMinutiae() { Name = "Puste" };
            this.state.Id = 0;
        }

        public string ToJson()
        {
            JObject minutiaeJson = new JObject();
            minutiaeJson["id"] = 0;
            minutiaeJson["name"] = "Puste";

            return minutiaeJson.ToString();
        }

        public override string ToString()
        {
            return 0 + ";" + "Puste";
        }
    }
}
