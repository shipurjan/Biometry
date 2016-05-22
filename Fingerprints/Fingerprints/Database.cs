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
            AddNewMinutiae("Por", 0, "Czerwony");
            AddNewMinutiae("Rozwidlenie", 1, "Zielony");
            AddNewMinutiae("Dowolna", 2, "Niebieski");
        }

        static public void AddNewMinutiae(string name, int drawType, string color)
        {
            using (var db = new FingerContext())
            {
                var MinutiaeType = new MinutiaeType()
                {
                    Name = name,
                    DrawType = drawType,
                    Color = color                 
                };
                db.MinutiaeTypes.Add(MinutiaeType);
                db.SaveChanges();                
            }
        }

    }
}
