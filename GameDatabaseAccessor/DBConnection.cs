using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameDatabaseAccessor
{
    internal static class DBConnection
    {
        public static SqlConnection GetDBConnection()
        {
            // this should be the only place in your application
            // where your connection string appears, and this
            // should be the only method any class uses to create
            // a database connection object

            //This is where you would update if you were to change the location of the DB
            //if you were change location you will also need to update the batch file located
            //in the project file
            var connString = @"Data Source=localhost;Initial Catalog=GameDatabase;Integrated Security=True";
            var conn = new SqlConnection(connString);
            return conn;
        }
    }
}
