using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints.Resources
{
    static class ExtensionMethods
    {
        public static void SelectionChangedEvent(this System.Windows.Controls.ListBox listbox, Action method)
        {
            listbox.SelectionChanged += (ss, ee) =>
            {
                method();
            };
        }

        public static JObject ToJObject(this Point point)
        {
            JObject jObjectPoint = new JObject();
            jObjectPoint["x"] = Math.Floor(point.X);
            jObjectPoint["y"] = Math.Floor(point.Y);

            return jObjectPoint;
        }
    }
}
