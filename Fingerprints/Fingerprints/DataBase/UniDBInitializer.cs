using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprints.DataBase
{
    class UniDBInitializer<T> : CreateDatabaseIfNotExists<FingerContext>
    {
        
    }
}
