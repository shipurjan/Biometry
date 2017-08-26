using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static Point ToFloorPoint(this Point point)
        {
            Point floorPoint = new Point();
            floorPoint.X = Math.Floor(point.X);
            floorPoint.Y = Math.Floor(point.Y);

            return floorPoint;
        }

        public static bool AnyOrNotNull<T>(this IEnumerable<T> source)
        {
            if (source != null && source.Any())
                return true;
            else
                return false;
        }

        public static void ReplaceOrAddOnLastIndex<T>(this List<T> source, int index, T item)
        {
            if(source.Count() > index)
            {
                source.RemoveAt(index);
                source.Insert(index, item);
            }
            else
            {
                source.Add(item);
            }
        }
    }
}
