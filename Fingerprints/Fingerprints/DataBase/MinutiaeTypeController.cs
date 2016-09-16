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
                var q = db.SelfDefinedMinutiaes.Where(x => x.ProjectId == Database.currentProject).ToList();
                return q;
            }
        }
    }
}
