using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fingerprints
{
    public class AppInstance
    {
        public MainWindow window { get; set; }
        public AppInstance()
        {
            window = (MainWindow)Application.Current.MainWindow;
        }
    }
}
