using System;
using GeoCoordinatePortable;
using MySql.Data.MySqlClient;

namespace PTS.App.DataBase
{
    public static class DataBaseManager
    {
        private static MySqlConnection connection;

        private static string host = "localhost";
        private static string database = "pts";
        private static string username = "pts";
        private static string password = "y6NOAw5S3FhLHsWe";
        private static int port = 3306;

        public static MySqlConnection Connection => connection;


        /*Setup the database connection*/
        public static void SetupConnection()
        {
            //Close the current database connection
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
                
            //Create new connection to the database
            string connString = $"Server={host};Database={database};port={port};User Id={username};password={password};";
            connection = new MySqlConnection(connString);
            try
            {
                //Open the connection link
                connection.Open();
            }
            catch(Exception e)
            {
                //Close connection
                connection.Close();
                connection.Dispose();
                connection = null;

                //Print error
                throw new NotImplementedException("Error: " + e.Message);

            }

        }

        public static void CloseConnection()
        {
            //Close connection
            connection.Close();
            connection.Dispose();
            connection = null;
        }


    }
}
