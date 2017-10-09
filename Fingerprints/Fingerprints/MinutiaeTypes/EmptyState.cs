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
        public EmptyState(DrawingService _oDrawingService) : base(_oDrawingService)
        {
            Minutia = new SelfDefinedMinutiae() { Name = "Puste" };
        }
    }
}
