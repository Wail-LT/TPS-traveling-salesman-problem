using System;
using System.Collections.Generic;
using System.Data.Common;
using MySql.Data.MySqlClient;
using PTS.App.DataBase;
using PTS.App.Objects;

namespace PTS.App.Managers
{
    public class CityManager
    {
        private readonly MySqlConnection dbConn;

        public CityManager(MySqlConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public City GetCity(string cityName, string cityZIP)
        {
            //Create a request to get the city
            using MySqlCommand rqst = DataBaseManager.Connection.CreateCommand();

            //Fill the request
            rqst.CommandText = "SELECT ville_nom_reel, " +
            "ville_longitude_deg, " +
            "ville_latitude_deg " +
            "FROM `villes_france_free` " +
            $"WHERE UPPER(ville_nom_simple) = UPPER('{ cityName }')" +
            $"AND ville_code_postal = '{ cityZIP }'";

            City city = null;

            //Running the request  
            using (DbDataReader reader = rqst.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    //Count the number of records get by the request
                    //should be equal to 1
                    int rowCount = 1;
                    while (reader.Read())
                    {
                        //If rowCount >1
                        if (rowCount > 1)
                        {
                            //Free ressources
                            rqst.DisposeAsync();
                            reader.DisposeAsync();

                            //Raise error
                            throw new NotImplementedException("Too much records returned for the same City name and Zip code");
                        }

                        int cityNameOrd = reader.GetOrdinal("ville_nom_simple");
                        int longitudeOrd = reader.GetOrdinal("ville_nom_simple");
                        int latitudeOrd = reader.GetOrdinal("ville_nom_simple");

                        string name = reader.GetString(cityNameOrd);
                        double longitude = reader.GetDouble(longitudeOrd);
                        double latitude = reader.GetDouble(latitudeOrd);

                        city = new City(name, longitude, latitude);

                        rowCount++;
                    }
                }
            }

            return city;
        }

        public List<City> GetCities(Dictionary<string, string> cities)
        {
            //Create a request to get the cities
            using MySqlCommand rqst = DataBaseManager.Connection.CreateCommand();

            //Fill the request
            rqst.CommandText = "SELECT ville_nom_reel, " +
                "ville_longitude_deg, " +
                "ville_latitude_deg " +
                "FROM `villes_france_free` " +
                "WHERE";

            foreach (var item in cities)
            {
                string where = $"ville_code_postal LIKE '%{item.Key}%' " +
                    $"AND UPPER(ville_nom_reel) = UPPER('{item.Value}') " +
                    $"OR";

                rqst.CommandText += " " + where;
            }

            //Remove the last ,
            rqst.CommandText = rqst.CommandText.Remove(rqst.CommandText.Length - 2);

            //End the request
            rqst.CommandText += ";";

            //Create the list of cities
            List<City> citiesList = new List<City>();

            //Running the request  
            using (DbDataReader reader = rqst.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int cityNameOrd = reader.GetOrdinal("ville_nom_reel");
                        int longitudeOrd = reader.GetOrdinal("ville_longitude_deg");
                        int latitudeOrd = reader.GetOrdinal("ville_latitude_deg");

                        string name = reader.GetString(cityNameOrd);
                        double longitude = reader.GetDouble(longitudeOrd);
                        double latitude = reader.GetDouble(latitudeOrd);

                        citiesList.Add(new City(name, longitude, latitude));
                    }
                }
            }
            return citiesList;
        }

        public List<Dictionary<string,string>> GetAllCitiesName()
        {
            //Create a request to get the cities
            using MySqlCommand rqst = DataBaseManager.Connection.CreateCommand();

            //Fill the request
            rqst.CommandText = "SELECT ville_nom_reel, " +
                "ville_code_postal " +
                "FROM `villes_france_free`;";


            //Create the list of cities
            List<Dictionary<string,string>> citiesList = new List<Dictionary<string, string>>();

            //Running the request  
            using (DbDataReader reader = rqst.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int cityNameOrd = reader.GetOrdinal("ville_nom_reel");
                        int zipOrd = reader.GetOrdinal("ville_code_postal");

                        string cityName = reader.GetString(cityNameOrd);
                        string cityZip = reader.GetString(zipOrd);
                        
                        citiesList.Add(new Dictionary<string, string>{ { "name", cityName }, { "zip", cityZip } });
                    }
                }
            }
            return citiesList;
        }
    }
}
