using Fingerprints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Factories
{
    public abstract class MinutiaFactory
    {
        public abstract IDraw Create(MinutiaState state);
    }
}
