﻿using ExceptionLogger;
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
        /// <summary>
        /// Import data from path and returns ImportResult object
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_drawingService"></param>
        /// <returns></returns>
        public static ImportResult Import(string _path)
        {
            ImportResult result = null;
            List<MinutiaFileState> parsedData = null;
            ImportTypes? importType;
            try
            {
                //parse extension to ImportType enum
                importType = PathTool.GetImportTypeFromPath(_path);

                // if parse fails, show error
                if (!importType.HasValue)
                {
                    string error = string.Format("File format '{0}' not supported", Path.GetExtension(_path));
                    return new ImportResult(false, null, error);
                }

                //if file not exists, show error
                if (!File.Exists(_path))
                {
                    string error = string.Format("File {0} not found", Path.GetFileName(_path));
                    return new ImportResult(false, null, error);
                }

                parsedData = GetImportedData(_path, importType.Value);

                result = new ImportResult(true, parsedData, string.Empty);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }

        /// <summary>
        /// Load file and parse it to MinutiaFileState list
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_importType"></param>
        /// <param name="_drawingService"></param>
        /// <returns></returns>
        private static List<MinutiaFileState> GetImportedData(string _path, ImportTypes _importType)
        {
            List<MinutiaFileState> result = null;
            IDataImporter importer = null;
            try
            {
                //create Import object based on type
                importer = ImporterFactory.Create(_importType);

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
