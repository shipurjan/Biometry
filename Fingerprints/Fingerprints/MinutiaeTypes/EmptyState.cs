using ExceptionLogger;
using Fingerprints.Models;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Fingerprints.MinutiaeTypes
{
    class EmptyState : MinutiaStateBase
    {
        public EmptyState(int? _atIndex = null) : base(generateMinutia(), null, _atIndex)
        {
        }

        private static SelfDefinedMinutiae generateMinutia()
        {
            SelfDefinedMinutiae result = null;
            try
            {
                result = new SelfDefinedMinutiae() { Name = "Puste", DrawingType = DrawingType.Empty };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }
    }
}
