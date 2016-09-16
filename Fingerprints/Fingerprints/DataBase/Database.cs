using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Fingerprints
{
    class Database
    {
        /// <summary>
        /// Na razie dodaje rzeczy na sztywno do jednej tabeli, potem to trzeba bedzie zmienic jakos ;o
        /// </summary>
        public static int currentProject = 0;
        static public void InitialData()
        {
            AddNewMinutiae("Por", 1, "Czerwony", 1, 1);
            AddNewMinutiae("Rozwidlenie", 2 , "Zielony", 1.2, 1);
            AddNewMinutiae("Dowolna", 3, "Niebieski", 1, 1);
        }

        static public void AddNewMinutiae(string name, int drawType, string color, double size, double thickness)
        {
            using (var db = new FingerContext())
            {
                var q = db.Types.Where(x => x.TypeId == drawType).Select(x => x.TypeId).Single();
                var SelfDefinedMinutiae = new SelfDefinedMinutiae()
                {
                    Name = name,
                    ProjectId = currentProject,
                    TypeId = q,
                    Color = color,
                    Size = size,
                    Thickness = thickness
                                    
                };
                db.SelfDefinedMinutiaes.Add(SelfDefinedMinutiae);
                db.SaveChanges();          
            }
        }
        static public void DeleteMinutiae(SelfDefinedMinutiae minutiae)
        {
            using (var db = new FingerContext())
            { 
                db.SelfDefinedMinutiaes.Attach(minutiae);
                db.SelfDefinedMinutiaes.Remove(minutiae);
                db.SaveChanges();
            }
        }

        static public List<SelfDefinedMinutiae> ShowSelfDefinedMinutiae()
        {
            using (var db = new FingerContext())
            {
                var q = db.SelfDefinedMinutiaes.AsEnumerable().ToList();
                return q;
            }
        }
        static public void AddNewProject(string name)
        {
            using (var db = new FingerContext())
            {
                var Project = new Project()
                {
                    Name = name
                };
                db.Projects.Add(Project);
                db.SaveChanges();
            }
        }
        static public List<Project> ShowProject()
        {
            using (var db = new FingerContext())
            {
                var q = db.Projects.AsEnumerable().ToList();
                return q;
            }
        }

    }
}
