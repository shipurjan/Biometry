using Fingerprints.Models;
using Fingerprints.MinutiaeTypes.Vector;
using Fingerprints.MinutiaeTypes.SinglePoint;
using Fingerprints.MinutiaeTypes.Empty;
using Fingerprints.MinutiaeTypes.Segment;
using Fingerprints.MinutiaeTypes.Peak;

namespace Fingerprints.Factories
{
    class FileMinutiaFactory : MinutiaFactory
    {
        public override IDraw Create(MinutiaState state)
        {
            IDraw draw = null;

            if (state.Minutia == null)
            {
                return new UserEmpty(state);
            }

            switch (state.Minutia.TypeId)
            {
                case 1:
                    draw = new FileSinglePoint(state);
                    break;
                case 2:
                    draw = new FileVector(state);
                    break;
                //case 3:
                //    draw = new CurveLine(state);
                //    break;
                //case 4:
                //    draw = new Triangle(state);
                //    break;
                case 5:
                    draw = new FilePeak(state);
                    break;
                case 6:
                    draw = new FileSegment(state);
                    break;
                default:
                    draw = new FileEmpty(state);
                    break;
            }
            return draw;
        }
    }
}
