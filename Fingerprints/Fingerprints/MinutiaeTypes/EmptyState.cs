using ExceptionLogger;
using Fingerprints.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.MinutiaeTypes
{
    class EmptyState : MinutiaStateBase
    {
        public EmptyState(DrawingService _oDrawingService, int? _atIndex = null) : base(_oDrawingService, generateMinutia(), _atIndex)
        {
        }

        private static SelfDefinedMinutiae generateMinutia()
        {
            SelfDefinedMinutiae result = null;
            try
            {
                result = new SelfDefinedMinutiae() { Name = "Puste", TypeId = 7 };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }

            return result;
        }
    }
}
