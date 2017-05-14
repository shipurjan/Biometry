﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fingerprints.Models;
using Fingerprints.MinutiaeTypes;
using Fingerprints.MinutiaeTypes.Vector;
using Fingerprints.MinutiaeTypes.SinglePoint;
using Fingerprints.MinutiaeTypes.Empty;
using Fingerprints.MinutiaeTypes.Segment;

namespace Fingerprints.Factories
{
    class UserMinutiaFactory : MinutiaFactory
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
                    draw = new UserSinglePoint(state);
                    break;
                case 2:
                    draw = new UserVector(state);
                    break;
                //case 3:
                //    draw = new CurveLine(state);
                //    break;
                //case 4:
                //    draw = new Triangle(state);
                //    break;
                //case 5:
                //    draw = new Peak(state);
                //    break;
                case 6:
                    draw = new UserSegment(state);
                    break;
                case 7:
                    draw = new UserEmpty(state);
                    break;
            }
            return draw;
        }
    }
}
