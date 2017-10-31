using Fingerprints.Factories;
using Fingerprints.Models;
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
        protected override void Seed(FingerContext context)
        {

            IList<Type> types = new List<Type>();

            foreach (DrawingType drawingType in Enum.GetValues(typeof(DrawingType)))
            {
                types.Add(TypeModelFactory.Create(drawingType));
            }
                        

            foreach (Type type in types)
                context.Types.Add(type);
            base.Seed(context);
        }
    }
}
