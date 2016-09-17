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

        public int GetTypeIdOfSelectedMinutiae(string selectedValue)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.TypeId).First();
                return q;
            }
        }

        public string GetColorOfSelectedMinutiae(string selectedValue)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.Color).First();
                return q;
            }
        }

        public double GetThicknessOfSelectedMinutiae(string selectedValue)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.Thickness).First();
                return q;
            }
        }
        public double GetSizeOfSelectedMinutiae(string selectedValue)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.Size).First();
                return q;
            }
        }
    }
}
