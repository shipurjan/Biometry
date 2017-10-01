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
    class ExportService
    {
        /// <summary>
        /// Opens SaveFileDialog and export data based on type and path from dialog
        /// </summary>
        /// <param name="_firstData"></param>
        /// <param name="firstImageName"></param>
        /// <param name="_secondData"></param>
        /// <param name="secondImageName"></param>
        public void SaveAsFileDialog(List<MinutiaStateBase> _firstData, string firstImageName,
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
                        FileTransfer.getPath(saveFileDialog.FileName, firstImageName));

                    Export((ExportTypes)saveFileDialog.FilterIndex, _secondData,
                        FileTransfer.getPath(saveFileDialog.FileName, secondImageName));
                }
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
        public void Export(ExportTypes _type, List<MinutiaStateBase> _data, string _fullPath)
        {
            IDataExporter dataExporter = null;

            dataExporter = ExportFactory.Create(_type, _data);

            if (dataExporter != null)
            {
                dataExporter.FormatData();
                dataExporter.Export(_fullPath);
            }
        }
    }
}
