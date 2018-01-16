using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using ExceptionLogger;
using System.Windows.Media;

namespace Fingerprints
{
    class Database
    {
        /// <summary>
        /// Na razie dodaje rzeczy na sztywno do jednej tabeli, potem to trzeba bedzie zmienic jakos ;o
        /// </summary>
        public static int currentProject = 0;

        static public SelfDefinedMinutiae AddNewMinutiae(string name, DrawingType drawType, Brush minutiaColor)
        {
            SelfDefinedMinutiae result = null;
            try
            {
                using (var db = new FingerContext())
                {                    
                    var SelfDefinedMinutiae = new SelfDefinedMinutiae()
                    {
                        Name = name,
                        ProjectId = currentProject,
                        DrawingType = drawType,
                        Color = minutiaColor.ToString(),

                    };
                    db.SelfDefinedMinutiaes.Add(SelfDefinedMinutiae);
                    db.SaveChanges();

                    result = SelfDefinedMinutiae;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
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
                var q = db.SelfDefinedMinutiaes.Where(x => x.ProjectId == currentProject).AsEnumerable().ToList();
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

        static public void DeleteProject(Project project)
        {
            using (var db = new FingerContext())
            {
                db.Projects.Attach(project);
                db.Projects.Remove(project);
                db.SaveChanges();
            }
        }
        static public List<Project> ShowProject()
        {
            using (var db = new FingerContext())
            {
                var q = db.Projects.ToList();
                return q;
            }
        }

        static public string GetProjectName()
        {
            string result = String.Empty;
            try
            {
                using (var db = new FingerContext())
                {
                    result = db.Projects.FirstOrDefault(project => project.ProjectID == currentProject).ToString();                    
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        static public void InitDbIfNoExit()
        {
            try
            {
                using (var db = new FingerContext())
                {
                    db.Database.CreateIfNotExists();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }            
        }

    }
}
