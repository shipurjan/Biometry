using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Importers
{
    public static class ImporterService
    {
        public static ImportResult Import(string _path, DrawingService _drawingService)
        {
            ImportResult result = null;
            List<MinutiaStateBase> parsedData = null;
            ImportTypes? importType;
            try
            {
                importType = PathTool.GetImportTypeFromPath(_path);
                if (!importType.HasValue)
                {
                    string error = String.Format("Extension '{0}' not supported", Path.GetExtension(_path));
                    return new ImportResult(false, null, error);
                }

                if (!File.Exists(_path))
                {
                    string error = String.Format("File {0} not found", Path.GetFileName(_path));
                    return new ImportResult(false, null, error);
                }

                parsedData = ParseFileResult(_path, importType.Value, _drawingService);

                result = new ImportResult(true, parsedData, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        private static List<MinutiaStateBase> ParseFileResult(string _path, ImportTypes _importType, DrawingService _drawingService)
        {
            List<MinutiaStateBase> result = null;
            IDataImporter importer = null;
            try
            {
                importer = ImporterFactory.Create(_importType, _drawingService);

                if (importer != null)
                {
                    importer.Import(_path);
                    result = importer.GetformattedData();
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
