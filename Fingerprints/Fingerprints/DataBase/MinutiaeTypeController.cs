using ExceptionLogger;
using Fingerprints.Models;
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
            List<SelfDefinedMinutiae> q = null;
            try
            {
                using (var db = new FingerContext())
                {
                    q = db.SelfDefinedMinutiaes.Where(x => x.ProjectId == Database.currentProject).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return q;

        }

        public List<MinutiaState> getStates()
        {
            List<MinutiaState> states = new List<MinutiaState>();
            try
            {
                using (var db = new FingerContext())
                {
                    var q = db.SelfDefinedMinutiaes.Where(x => x.ProjectId == Database.currentProject).ToList();

                    foreach (var item in q)
                    {
                        states.Add(new MinutiaState() { Minutia = item });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return states;
        }

        public List<SelfDefinedMinutiae> GetAllMinutiaeTypes()
        {
            List<SelfDefinedMinutiae> q = null;
            try
            {
                using (var db = new FingerContext())
                {
                    q = db.SelfDefinedMinutiaes.ToList();
                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return q;

        }

        public DrawingType GetTypeIdOfSelectedMinutiae(string selectedValue)
        {
            DrawingType q = DrawingType.Empty;
            try
            {
                using (var db = new FingerContext())
                {
                    q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.DrawingType).First();                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return q;
        }

        public string GetColorOfSelectedMinutiae(string selectedValue)
        {
            string q = String.Empty;
            try
            {
                using (var db = new FingerContext())
                {
                    q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.Color).First();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return q;
           
        }

        public SelfDefinedMinutiae GetMinutia(string name)
        {
            SelfDefinedMinutiae q = null;
            try
            {
                using (var db = new FingerContext())
                {
                    q = db.SelfDefinedMinutiaes.Where(x => x.Name == name).First();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return q;            
        }
    }
}
