using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fingerprints
{
    class MinutiaeTypes
    {
        public Dictionary<int, string> dic { get; set; }
        public MinutiaeTypes()
        {
            dic = new Dictionary<int, string>();
            Database db = new Database();
            using ()
            {

            }
            //dic.Add(0, "Półprosta skierowana");
            //dic.Add(1, "Por");
            //dic.Add(2, "Krzywa");
        }
    }
}
