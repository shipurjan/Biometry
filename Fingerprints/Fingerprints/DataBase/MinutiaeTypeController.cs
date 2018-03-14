﻿using Fingerprints.Models;
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

        public List<MinutiaState> getStates()
        {
            List<MinutiaState> states = new List<MinutiaState>();
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.ProjectId == Database.currentProject).ToList();

                foreach (var item in q)
                {
                    states.Add(new MinutiaState() { Minutia = item });
                }

                return states;
            }
        }

        public List<SelfDefinedMinutiae> GetAllMinutiaeTypes()
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.ToList();
                return q;
            }
        }

        public DrawingType GetTypeIdOfSelectedMinutiae(string selectedValue)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == selectedValue).Select(y => y.DrawingType).First();
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

        public SelfDefinedMinutiae GetMinutia(string name)
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.Where(x => x.Name == name).First();
                return q;
            }
        }
    }
}
