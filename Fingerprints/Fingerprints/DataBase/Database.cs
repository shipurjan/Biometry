using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Fingerprints
{
    class Database
    {
        /// <summary>
        /// Na razie dodaje rzeczy na sztywno do jednej tabeli, potem to trzeba bedzie zmienic jakos ;o
        /// </summary>

        static public void InitialData()
        {
            AddNewMinutiae("Por", 1, 1, "Czerwony", 1);
            AddNewMinutiae("Rozwidlenie",1 ,2 , "Zielony", 1.2);
            AddNewMinutiae("Dowolna",1 ,3 , "Niebieski", 1);
        }

        static public void AddNewMinutiae(string name, int projectid, int drawType, string color, double size)
        {
            using (var db = new FingerContext())
            {
                var q = db.Types.Where(x => x.TypeId == drawType).Select(x => x.TypeId).Single();
                var SelfDefinedMinutiae = new SelfDefinedMinutiae()
                {
                    Name = name,
                    ProjectId = projectid,
                    TypeId = q,
                    Color = color,
                    Size = size
                                    
                };
                db.SelfDefinedMinutiaes.Add(SelfDefinedMinutiae);
                db.SaveChanges();          
            }
        }

    }
}
