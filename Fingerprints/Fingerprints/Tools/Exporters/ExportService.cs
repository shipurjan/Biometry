using ExceptionLogger;
using Fingerprints.MinutiaeTypes;
using Fingerprints.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Tools.Exporters
{
    public class ExportService
    {
        /// <summary>
        /// Opens SaveFileDialog and export data based on type and path from dialog
        /// </summary>
        /// <param name="_firstData"></param>
        /// <param name="firstImageName"></param>
        /// <param name="_secondData"></param>
        /// <param name="secondImageName"></param>
        public static void SaveAsFileDialog(List<MinutiaStateBase> _firstData, string firstImageName,
            List<MinutiaStateBase> _secondData, string secondImageName)
        {
            SaveFileDialog saveFileDialog = null;
            try
            {
                saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Bozorth (.xyt)|*.xyt|Default (.txt)|*.txt";
                saveFileDialog.Title = "Save an Image File";
                saveFileDialog.ShowDialog();

                if (saveFileDialog.FileName != "")
                {
                    Export((ExportTypes)saveFileDialog.FilterIndex, _firstData,
                        PathTool.CombainePathWithName(saveFileDialog.FileName, firstImageName));

                    Export((ExportTypes)saveFileDialog.FilterIndex, _secondData,
                        PathTool.CombainePathWithName(saveFileDialog.FileName, secondImageName));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Exports data to specific paths
        /// </summary>
        /// <param name="_firstData"></param>
        /// <param name="_leftFullPath"></param>
        /// <param name="_secondData"></param>
        /// <param name="_rightFullPath"></param>
        public static void SaveTxt(List<MinutiaStateBase> _firstData, string _leftFullPath,
            List<MinutiaStateBase> _secondData, string _rightFullPath)
        {
            try
            {
                Export(ExportTypes.Txt, _firstData, _leftFullPath);
                Export(ExportTypes.Txt, _secondData, _rightFullPath);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        /// <summary>
        /// Export Data by ExportType and saves
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_data"></param>
        /// <param name="_fullPath">Full path with extension</param>
        public static void Export(ExportTypes _type, List<MinutiaStateBase> _data, string _fullPath)
        {
            try
            {
                IDataExporter dataExporter = null;

                // get DataExporter object by type
                dataExporter = ExportFactory.Create(_type, _data);

                if (dataExporter != null)
                {
                    dataExporter.FormatData();
                    dataExporter.Export(_fullPath);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }
    }
}
