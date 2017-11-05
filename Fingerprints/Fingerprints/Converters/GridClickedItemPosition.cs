using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.Converters
{
    enum Columns
    {
        Index = 0,
        FirstImage = 1,
        SecondImage = 2,
        Delete = 3
    }
    class GridClickedItemPosition
    {
        public Columns CellIndex { get; set; }

        public int RowIndex { get; set; }
    }
}
