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
        public EmptyState(DrawingService _oDrawingService, int? _atIndex = null) : base(_oDrawingService, _atIndex)
        {
            Minutia = new SelfDefinedMinutiae() { Name = "Puste", TypeId = 7 };
        }
    }
}
