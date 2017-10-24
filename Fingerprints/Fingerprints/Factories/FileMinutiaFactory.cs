using Fingerprints.Models;
using Fingerprints.MinutiaeTypes;
using ExceptionLogger;
using System;
using System.Linq;

namespace Fingerprints.Factories
{
    class FileMinutiaFactory 
    {
        public static MinutiaFileState Create(MinutiaStateBase _state)
        {
            MinutiaFileState result = null;
            try
            {
                result = new MinutiaFileState()
                {
                    Angle = _state.Angle,
                    Id = _state.Id,
                    Name = _state.MinutiaName,
                    Points = _state.Points.ToList()
                };
            }
            catch (Exception ex)
            {
                Logger.WriteExceptionLog(ex);
            }
            return result;
        }
    }
}
