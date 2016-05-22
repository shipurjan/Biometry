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
    }
}
