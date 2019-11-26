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
        private MySqlConnection dbConn;

        public CityManager(MySqlConnection dbConn)
        {
            this.dbConn = dbConn;
        }

        public List<City> GetCities(Dictionary<string, string> cities)
        {
            //Create a request to get the city
            MySqlCommand rqst = DataBaseManager.connection.CreateCommand();

            //Fill the request
            rqst.CommandText = "SELECT ville_nom_reel, " +
                "ville_longitude_deg, " +
                "ville_latitude_deg " +
                "FROM `villes_france_free` " +
                $"WHERE UPPER(ville_code_postal, ville_nom_simple) = in (";

            foreach(var item in cities)
            {
                rqst.CommandText += $"(UPPER({item.Key}),{item.Value}),";
            }

            //Remove the last ,
            rqst.CommandText.Remove(rqst.CommandText.Length - 1);

            //End the request
            rqst.CommandText += ");";

            //Create the list of cities
            List<City> citiesList = new List<City>();

            //Running the request  
            using (DbDataReader reader = rqst.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int cityNameOrd = reader.GetOrdinal("ville_nom_simple");
                        int longitudeOrd = reader.GetOrdinal("ville_nom_simple");
                        int latitudeOrd = reader.GetOrdinal("ville_nom_simple");

                        string name = reader.GetString(cityNameOrd);
                        double longitude = reader.GetDouble(longitudeOrd);
                        double latitude = reader.GetDouble(latitudeOrd);

                        citiesList.Add(new City(name, longitude, latitude));
                    }
                }
            }

            return citiesList;
        }

        public City GetCity(string cityName, string cityZIP)
        {
            //Create a request to get the city
            MySqlCommand rqst = DataBaseManager.connection.CreateCommand();

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

    }
}
