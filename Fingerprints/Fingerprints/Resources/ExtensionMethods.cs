using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
