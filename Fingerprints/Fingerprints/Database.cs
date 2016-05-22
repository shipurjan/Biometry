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
        public SQLiteConnection connection;

        public Database()
        {
            connection = new SQLiteConnection("Data Source=MyDatabase.sqlite");
            connection.Open();
        }
        public void CreateTable()
        {
            using (var cmd = new SQLiteCommand("Create Table Minutiae (Name Varchar(50), DrawingType int, Color Varchar(50))", this.connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        public void InsertData()
        {
            using (var cmd = new SQLiteCommand("Insert Into Minutiae (Name, DrawingType, Color) Values ('Por', 0, 'Czerwony'), ('Zakończenie', 1, 'Zielony'), ('Krzywa', 2, 'Niebieski') ", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }

    }
}
