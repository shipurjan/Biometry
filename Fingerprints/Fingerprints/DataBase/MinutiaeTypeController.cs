using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints
{
    class MinutiaeTypeController
    {
        public List<SelfDefinedMinutiae> Show()
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.ToList();
                return q;
            }
        }
    }
}
