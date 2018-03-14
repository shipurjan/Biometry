using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    public abstract class ImporterBase
    {

        /// <summary>
        /// File content in string
        /// </summary>
        protected string fileContent;

        public ImporterBase()
        {
        }

        public SelfDefinedMinutiae GetSelfDefinedMinutiaOrCreate(string _minutiaName = "mindtct")
        {
            SelfDefinedMinutiae result = null;
            try
            {
                using (var db = new FingerContext())
                {
                    result = db.SelfDefinedMinutiaes.Where(x => x.DrawingType == DrawingType.Vector && x.Name == _minutiaName).FirstOrDefault();

                    if (result == null)
                    {
                        result = new SelfDefinedMinutiae()
                        {
                            ProjectId = Database.currentProject,
                            DrawingType = DrawingType.Vector,
                            Name = _minutiaName,
                            Color = "#00a9ff"
                        };

                        db.SelfDefinedMinutiaes.Add(result);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
