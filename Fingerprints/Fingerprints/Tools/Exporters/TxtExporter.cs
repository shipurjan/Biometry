using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.MinutiaeTypes;
using ExceptionLogger;
using Newtonsoft.Json.Linq;
using Fingerprints.Resources;
using System.IO;
using Fingerprints.Models;
using Newtonsoft.Json;
using Fingerprints.Factories;

namespace Fingerprints.Tools.Exporters
{
    class TxtExporter : Exporter, DataExporter
    {
        private List<MinutiaFileState> preparedData;
        public TxtExporter(List<MinutiaStateBase> _data) : base(_data)
        {
            preparedData = new List<MinutiaFileState>();
        }

        public void Export(string _path)
        {
            try
            {
                FileTransfer.SaveFile(_path, JsonConvert.SerializeObject(preparedData, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        public void FormatData()
        {
            try
            {
                foreach (var item in data)
                {
                    preparedData.Add(Parse(item));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
        }

        private MinutiaFileState Parse(MinutiaStateBase _state)
        {
            MinutiaFileState result = null;
            try
            {
                result = FileMinutiaFactory.Create(_state);
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }
    }
}
