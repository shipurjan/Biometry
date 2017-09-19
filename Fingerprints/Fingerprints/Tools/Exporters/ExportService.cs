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

        public void Export(ExportTypes _type, List<MinutiaStateBase> _data, string _path)
        {
            IDataExporter dataExporter = null;

            dataExporter = ExportFactory.Create(_type, _data);

            if (dataExporter != null)
            {
                dataExporter.FormatData();
                dataExporter.Export(_path);
            }
        }
    }
}
